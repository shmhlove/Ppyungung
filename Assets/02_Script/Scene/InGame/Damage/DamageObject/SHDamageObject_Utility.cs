using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamageObject : SHMonoWrapper
{
    #region Utility : Transform
    void SetupParent()
    {
        var pParentObject  = (true == m_pInfo.m_bIsParentToUI) ? 
            Single.UI.GetRootToScene() : Single.Root3D.GetRootToDMG();
        
        if (null != GetWho())
        {
            if ((true == m_pInfo.m_bIsParentToCreator) ||
                (true == m_pInfo.m_bIsTraceToCreator))
            {
                pParentObject  = GetWho().GetTransform();
            }
        }

        SHGameObject.SetParent(GetTransform(), pParentObject);
    }
    void SetupTransform()
    {
        var vPosition = m_pInfo.m_vStaticStartPosition;

        if (null != GetWho())
        {
            if ((true == m_pInfo.m_bIsParentToCreator) ||
                (true == m_pInfo.m_bIsTraceToCreator))
            {
                vPosition = Vector3.zero;
            }
            else if (true == m_pInfo.m_bIsStartPosToCreator)
            {
                vPosition = GetWho().GetPosition();
            }
        }
        
        SetStartTransform();
        SetPosition(vPosition + m_pInfo.m_vPositionOffset);
        SetupScaleInfo();
    }
    void SetupPhysics()
    {
        InitPhysics();

        m_fDMGSpeed = m_pInfo.m_fStartSpeed;

        if (true == m_pInfo.m_bIsStartDirectionToCreator)
            m_vDMGDirection = GetWho().GetDirection();
        else if (true == m_pInfo.m_bIsRandomStartDirection)
            m_vDMGDirection = SHMath.RandomDirection();
        else
            m_vDMGDirection = m_pInfo.m_vStartDirection;

        m_vBeforePosition = GetPosition();
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

        m_pInfo.m_vScaleSpeed.x = m_pInfo.m_vScaleSpeed.x / m_pInfo.m_iScaleLifeTic;
        m_pInfo.m_vScaleSpeed.y = m_pInfo.m_vScaleSpeed.y / m_pInfo.m_iScaleLifeTic;
        m_pInfo.m_vScaleSpeed.z = m_pInfo.m_vScaleSpeed.z / m_pInfo.m_iScaleLifeTic;
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

        var fAngle = m_pInfo.m_fGuideAngleSpeed;
        if (true == m_pInfo.m_bIsUseCuvGuideAngleSpeed)
        {
            fAngle = m_pInfo.m_pGuideCuvAngleSpeed.Evaluate(GetLeftTimer());
        }

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
            switch (eEvent)
            {
                case eDamageEvent.Tick:
                    if ((0 != pInfo.m_iPlayToLifeTick) &&
                        (m_pInfo.m_iLifeTick == pInfo.m_iPlayToLifeTick))
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
            SetupEffectTransform(pInfo, pEffect);
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
        return (m_fDMGSpeed + m_pInfo.m_fAddSpeed);
    }
    #endregion
}