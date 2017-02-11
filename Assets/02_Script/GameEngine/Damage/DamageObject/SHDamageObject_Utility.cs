using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamageObject : SHMonoWrapper
{
    #region Utility : Position
    void SetupTransform()
    {
        var vLocalPosition = m_pInfo.m_vStaticStartPosition;
        var pParentObject  = Single.UI.GetRootToScene();

        if (null != GetWho())
        {
            if (true == m_pInfo.m_bIsTraceToCreator)
            {
                pParentObject  = GetWho().GetTransform();
                vLocalPosition = Vector3.zero;
            }
            else if (true == m_pInfo.m_bIsStartPosToCreator)
            {
                vLocalPosition = GetWho().GetLocalPosition();
            }
        }

        SHGameObject.SetParent(GetTransform(), pParentObject);
        SetStartTransform();
        SetLocalPosition(vLocalPosition + m_pInfo.m_vPositionOffset);
    }
    void SetupPhysicsValue()
    {
        InitPhysicsValue();

        m_fSpeed = m_pInfo.m_fStartSpeed;

        if (true == m_pInfo.m_bIsRandomStartDirection)
            m_vDirection = SHMath.RandomDirection();
        else
            m_vDirection = m_pInfo.m_vStartDirection;

        m_pBeforeBounds = GetCollider().bounds;
    }
    void MovePosition()
    {
        m_pBeforeBounds = GetCollider().bounds;

        if (false == m_pInfo.m_bIsTraceToCreator)
        {
            if ((true == m_pInfo.m_bIsUseGuideSystem) && 
                (GetLeftTick() > m_pInfo.m_iNotGuideTick))
                MoveToGuide();
            else
                MoveToNormal();
        }
    }
    void MoveToNormal()
    {
        var vSpeed = m_vDirection * GetMoveSpeed();
        {
            SetLocalPosition(
                SHPhysics.CalculationEuler(
                    m_pInfo.m_vForce, GetLocalPosition(), ref vSpeed, m_pInfo.m_fMass));
        }
        SetSpeed(vSpeed);
    }
    void MoveToGuide()
    {
        var pTarget = GetGuideTarget();
        if (null == pTarget)
            return;

        var fAngle = m_pInfo.m_fGuideAngleSpeed;
        if (true == m_pInfo.m_bIsUseCuvGuideAngleSpeed)
        {
            fAngle = m_pInfo.m_pGuideCuvAngleSpeed.Evaluate(GetLeftTimer());
        }

        var fSpeed = GetMoveSpeed();
        {
            SetLocalPosition(
                SHPhysics.GuidedMissile(
                    GetLocalPosition(), ref m_vDirection, pTarget.transform.localPosition, fAngle, fSpeed));
        }
        SetSpeed(fSpeed);
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
            switch (eEvent)
            {
                case eDamageEvent.Tick:
                    if (m_pInfo.m_iLifeTick == pInfo.m_iPlayToLifeTick)
                        Single.Sound.PlayEffect(pInfo.m_strClipName);
                    break;
                case eDamageEvent.Start:
                    if (true == pInfo.m_bIsPlayToStart)
                        Single.Sound.PlayEffect(pInfo.m_strClipName);
                    break;
                case eDamageEvent.Delete:
                    if (true == pInfo.m_bIsPlayToDelete)
                        Single.Sound.PlayEffect(pInfo.m_strClipName);
                    break;
                case eDamageEvent.Crash:
                    if (true == pInfo.m_bIsPlayToCrash)
                        Single.Sound.PlayEffect(pInfo.m_strClipName);
                    break;
            }
        });
    }
    void PlayEffect(eDamageEvent eEvent)
    {
        SHUtils.ForToList(m_pInfo.m_pEffectInfo, (pInfo) =>
        {
            var strPrefabName = string.Empty;
            switch (eEvent)
            {
                case eDamageEvent.Tick:
                    if (m_pInfo.m_iLifeTick == pInfo.m_iPlayToLifeTick)
                        strPrefabName = pInfo.m_strPrefabName;
                    break;
                case eDamageEvent.Start:
                    if (true == pInfo.m_bIsPlayToStart)
                        strPrefabName = pInfo.m_strPrefabName;
                    break;
                case eDamageEvent.Delete:
                    if (true == pInfo.m_bIsPlayToDelete)
                        strPrefabName = pInfo.m_strPrefabName;
                    if (true == pInfo.m_bIsDeleteWithDamage)
                        pInfo.SetDisableObject();
                    break;
                case eDamageEvent.Crash:
                    if (true == pInfo.m_bIsPlayToCrash)
                        strPrefabName = pInfo.m_strPrefabName;    
                    break;
            }

            if (true == string.IsNullOrEmpty(strPrefabName))
                return;

            var pEffect = CreateEffect(strPrefabName);
            if (null == pEffect)
                return;

            pInfo.SetEffectObject(pEffect);
            SetupEffectTransform(pEffect, pInfo);
            pEffect.SetActive(true);
        });
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

        return (false == GetWho().IsActive());
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
    void DecreaseCrashHitTick()
    {
        if (0 == m_iCrashHitTick)
            return;

        --m_iCrashHitTick;
    }
    #endregion


    #region Utility : Helpper
    SHMonoWrapper GetWho()
    {
        if (null == m_pParam)
            return null;

        return m_pParam.m_pWho;
    }
    GameObject GetGuideTarget()
    {
        if (null == m_pParam)
            return null;
        
        return m_pParam.m_pGuideTarget;
    }
    float GetLeftTimer()
    {
        return Single.Timer.GetSecToFixedTic(m_pSettingInfo.m_iLifeTick - m_pInfo.m_iLifeTick);
    }
    int GetLeftTick()
    {
        return (m_pSettingInfo.m_iLifeTick - m_pInfo.m_iLifeTick);
    }
    float GetMoveSpeed()
    {
        return (m_fSpeed + m_pInfo.m_fAddSpeed);
    }
    #endregion
}