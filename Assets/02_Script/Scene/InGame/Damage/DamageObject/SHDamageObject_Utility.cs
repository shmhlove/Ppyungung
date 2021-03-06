﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamageObject : SHMonoWrapper
{
    #region Utility : Transform
    void SetupParent()
    {
        var pParentObject = Single.Root3D.GetRootToDMG();

        if (true == m_pInfo.m_bIsParentToUI)
        {
            pParentObject = Single.UI.GetRootToScene();
        }
        
        if (true == m_pInfo.m_bIsTraceToCreator)
        {
            pParentObject = GetTraceTarget().GetTransform();
        }

        SetParent(pParentObject);
    }
    void SetupPhysics()
    {
        InitDamageValue();

        m_fDMGSpeed     = m_pInfo.m_fStartSpeed;
        m_vDMGDirection = m_pInfo.m_vStartDirection;

        if (true == m_pInfo.m_bIsRandomStartDirection)
        {
            m_vDMGDirection = SHMath.RandomDirection();
        }

        if (true == m_pInfo.m_bIsStartDirectionToCreator)
        {
            m_vDMGDirection = GetTraceTarget().GetDirection();
        }

        if (0.0f != m_pInfo.m_fOffsetAngle)
        {
            m_vDMGDirection = (Quaternion.AngleAxis(m_pInfo.m_fOffsetAngle, Vector3.forward) * m_vDMGDirection).normalized;
        }
    }
    void SetupTransform()
    {
        SetStartTransform();

        var vPosition = Vector3.zero;
        {
            if (Vector3.zero != m_pInfo.m_vStaticStartPosition)
            {
                vPosition = m_pInfo.m_vStaticStartPosition;
            }

            if (true == m_pInfo.m_bIsStartPosToCreator)
            {
                vPosition = GetTraceTarget().GetPosition();
            }

            if (Vector3.zero != m_pParam.m_pStartPosition)
            {
                vPosition = m_pParam.m_pStartPosition;
            }

            if (true == m_pInfo.m_bIsTraceToCreator)
            {
                vPosition = m_vStartPosition;
            }

            var fAngle = SHMath.GetAngleToPosition(Vector3.forward, 1.0f, Vector3.up, m_vDMGDirection);
            vPosition += SHMath.GetRotationOfVector(fAngle, Vector3.up, m_pInfo.m_vRotationOffset);
        }
        
        SetupScaleInfo();
        SetLocalPosition(vPosition + m_pInfo.m_vPositionOffset);
        SetLookZ(m_vDMGDirection);
    }
    void SetupScaleInfo()
    {
        if (null == m_pInfo)
            return;

        if ((Vector3.zero == m_pInfo.m_vStartScale) || 
            (Vector3.zero == m_pInfo.m_vEndScale))
            return;

        SetLocalScale(m_pInfo.m_vStartScale);
        m_pInfo.m_vScaleValue = m_pInfo.m_vStartScale;
        m_pInfo.m_vScaleSpeed = (m_pInfo.m_vEndScale - m_pInfo.m_vStartScale);

        if (0 == m_pInfo.m_iScaleLifeTic)
            m_pInfo.m_iScaleLifeTic = m_pInfo.m_iLifeTick;
        if (0 == m_pInfo.m_iScaleLifeTic)
            m_pInfo.m_iScaleLifeTic = 1;

        m_pInfo.m_vScaleSpeed.x /= m_pInfo.m_iScaleLifeTic;
        m_pInfo.m_vScaleSpeed.y /= m_pInfo.m_iScaleLifeTic;
        m_pInfo.m_vScaleSpeed.z /= m_pInfo.m_iScaleLifeTic;
    }
    void MovePosition()
    {
        m_vBeforePosition = GetPosition();

        if (true == m_pInfo.m_bIsTraceToCreator)
            return;
        
        if ((true == m_pInfo.m_bIsUseGuideSystem) && 
            (GetLeftTick() > m_pInfo.m_iNotGuideTick))
            MoveToGuide();
        else
            MoveToNormal();
    }
    void MoveToNormal()
    {
        var vSpeed = m_vDMGDirection * GetMoveSpeed();
        {
            SetPosition(
                SHPhysics.CalculationEuler(
                    m_pInfo.m_vForce, GetPosition(), ref vSpeed, m_pInfo.m_fMass));
        }
        SetDMGSpeed(vSpeed);
    }
    void MoveToGuide()
    {
        var pTarget = GetGuideTarget();
        if (null == pTarget)
            return;

        var fAngle = m_pInfo.m_pGuideAngleSpeed.Evaluate(GetLeftTimer());
        var fSpeed = GetMoveSpeed();
        {
            SetPosition(
                SHPhysics.GuidedMissile(
                    GetPosition(), ref m_vDMGDirection, pTarget.transform.position, fAngle, fSpeed));
        }
        SetDMGSpeed(fSpeed);
    }
    void MoveScale()
    {
        if (0.1f > (m_pInfo.m_vEndScale - m_pInfo.m_vScaleValue).magnitude)
            return;

        m_pInfo.m_vScaleValue = m_pInfo.m_vScaleValue + m_pInfo.m_vScaleSpeed;
        SetLocalScale(m_pInfo.m_vScaleValue);
    }
    #endregion


    #region Utility : Surport
    void PlayAnimation()
    {
        if (null == m_pInfo.m_pAnimationClip)
            return;

        PlayAnim(eDirection.Front, m_pInfo.m_pAnimTarget, m_pInfo.m_pAnimationClip, null);
    }
    void PlaySound(eDamageEvent eEvent)
    {
        SHUtils.ForToList(m_pInfo.m_pSoundInfo, (pInfo) =>
        {
            if (false == pInfo.m_pTimming.IsTimming(m_pInfo, eEvent))
                return;

            Single.Sound.PlayEffect(pInfo.m_strClipName);
        });
    }
    void PlayEffect(eDamageEvent eEvent)
    {
        SHUtils.ForToList(m_pInfo.m_pEffectInfo, (pInfo) =>
        {
            if (false == pInfo.m_pTimming.IsTimming(m_pInfo, eEvent))
                return;

            if (eDamageEvent.Delete == eEvent)
            {
                if (true == pInfo.m_bIsDeleteWithDamage)
                    pInfo.SetDisableObject();

                return;
            }

            var pEffect = CreateEffect(pInfo.m_strPrefabName);
            if (null == pEffect)
                return;

            pInfo.SetEffectObject(pEffect);
            SetupEffectTransform(pInfo, pEffect);
            pEffect.SetActive(true);
        });
    }
    void AddDamage(eDamageEvent eEvent)
    {
        foreach(var pInfo in m_pInfo.m_pAddDamageInfo)
        {
            if (false == pInfo.m_pTimming.IsTimming(m_pInfo, eEvent))
                return;

            var pParam = new SHDamageParam(m_pParam);
            pParam.m_pStartPosition = Single.Damage.GetDamagePosition(m_pInfo.m_strID);
            Single.Damage.AddDamage(pInfo.m_strPrefabName, pParam);
        }
    }
    void ClearEffect()
    {
        SHUtils.ForToList(m_pInfo.m_pEffectInfo, (pInfo) =>
        {
            pInfo.ClearEffectObject();
        });
    }
    #endregion


    #region Utility : Timer
    bool CheckDeleteWithCreator()
    {
        if (false == m_pInfo.m_bIsDeleteWithCreator)
            return false;

        if (null == GetWho())
            return true;

        return (0 >= GetWho().m_fHealthPoint);
    }
    bool DecreaseLifeTick()
    {
        if (0 >= --m_pInfo.m_iLifeTick)
        {
            if (true == m_pInfo.m_bIsLoopLifeTick)
                m_pInfo.m_iLifeTick = m_pSettingInfo.m_iLifeTick;
        }

        return (0 >= m_pInfo.m_iLifeTick);
    }
    #endregion


    #region Utility : Helpper
    SHMonoWrapper GetWho()
    {
        if (null == m_pParam)
            return this;

        return m_pParam.m_pWho;
    }
    SHMonoWrapper GetTraceTarget()
    {
        if (null == m_pParam)
            return this;

        if (null == m_pParam.m_pTraceTarget)
            return GetWho();
        else
            return m_pParam.m_pTraceTarget;
    }
    GameObject GetGuideTarget()
    {
        if (null == m_pParam)
            return null;

        if (null != m_pParam.m_pGuideTarget)
            return m_pParam.m_pGuideTarget;

        float      fMinDist    = float.MaxValue;
        GameObject pNearObject = null;
        foreach(var strTag in m_pInfo.m_pTargetUnitTags)
        {
            var pObjects = GameObject.FindGameObjectsWithTag(strTag);
            foreach(var pObject in pObjects)
            {
                var fDist = Vector3.Distance(GetPosition(), pObject.transform.position);
                if (fDist < fMinDist)
                {
                    fMinDist    = fDist;
                    pNearObject = pObject;
                }
            }
        }

        return pNearObject;
    }
    float GetLeftTimer()
    {
        return Single.Timer.GetSecToFixedTic(GetLeftTick());
    }
    int GetLeftTick()
    {
        return (m_pSettingInfo.m_iLifeTick - m_pInfo.m_iLifeTick);
    }
    float GetMoveSpeed()
    {
        return (m_fDMGSpeed + m_pInfo.m_fAddSpeed);
    }
    #endregion
}