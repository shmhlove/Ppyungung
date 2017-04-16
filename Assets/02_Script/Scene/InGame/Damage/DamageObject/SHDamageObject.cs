using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public partial class SHDamageObject : SHMonoWrapper
{
    #region Members : ParamInfo
    [SerializeField]  private SHDamageParam     m_pParam        = null;
    #endregion


    #region Members : Inspector
    [SerializeField]  private SHDamageObjectInfo m_pSettingInfo = new SHDamageObjectInfo();
    [HideInInspector] public  SHDamageObjectInfo m_pInfo        = new SHDamageObjectInfo();
    #endregion


    #region Members : ETC
    [HideInInspector] public bool    m_bIsDieDamage  = false;  // 데미지 라이프가 끝난 상태
    [HideInInspector] public bool    m_bIsCrashLock  = false;  // 데미지 충돌체크를 하지 않을 상태
    [HideInInspector] public int     m_iCrashHitTick = 0;      // 충돌 후 시간체크 (다단히트)
    #endregion
    

    #region System Functions
    public void OnInitialize(string strID, SHDamageParam pParam)
    {
        if (null == pParam)
        {
            Debug.LogErrorFormat("SHDamageObject::OnInitialize - Param Is Null!!");
            return;
        }
        
        m_pSettingInfo.m_strID = strID;
        m_pInfo         = new SHDamageObjectInfo(m_pSettingInfo);
        m_pParam        = pParam;
        m_bIsDieDamage  = false;

        SetupParent();
        SetupPhysics();
        SetupTransform();
        
        ClearEffect();
        
		SetActive (true);
        PlayAnimation();
        PlaySound(eDamageEvent.Start);
        PlayEffect(eDamageEvent.Start);
        AddDamage(eDamageEvent.Start);
    }
    public void OnFrameMove()
    {
        if (true == m_bIsDieDamage)
            return;

        if ((true == CheckDeleteWithCreator()) || 
            (true == DecreaseLifeTick()))
        {
            DeleteDamage();
            return;
        }

        DecreaseCrashHitTick();
        MovePosition();
        MoveScale();
        PlaySound(eDamageEvent.Tick);
        PlayEffect(eDamageEvent.Tick);
        AddDamage(eDamageEvent.Tick);
    }
    #endregion


    #region Virtual Functions
    public override void OnDisable()
    {
        base.OnDisable();

        if (true == SHApplicationInfo.m_bIsAppQuit)
            return;

        DeleteDamage();
    }
    public override void OnCrashDamage(SHMonoWrapper pCrashObject)
    {
        if (true == m_bIsDieDamage)
            return;

        m_iCrashHitTick = m_pInfo.m_iCheckDelayTickToCrash;

        PlaySound(eDamageEvent.Crash);
        PlayEffect(eDamageEvent.Crash);
        AddDamage(eDamageEvent.Crash);

        if (null != m_pParam)
            m_pParam.SendEventToCollision(this, pCrashObject);
        
        if (true == m_pInfo.m_bIsDeleteToCrash)
            DeleteDamage();
    }
    #endregion


    #region Interface Functions
    public bool IsTarget(string strTag)
    {
        if (true == string.IsNullOrEmpty(strTag))
            return false;

        foreach(var strTarget in m_pInfo.m_pTargetUnitTags)
        {
            if (true == strTarget.Equals(strTag))
                return true;
        }

        return false;
    }
    public int GetTargetLayerMask()
    {
        int iLayerMask = 0;
        foreach (var strTarget in m_pInfo.m_pTargetUnitTags)
        {
            iLayerMask |= (1 << LayerMask.NameToLayer(strTarget));
        }
        return iLayerMask;
    }
    public bool IsCheckCrash()
    {
        if (true == m_bIsCrashLock)
            return false;

        if (0 < m_iCrashHitTick)
            return false;

        // if ((0 != m_pInfo.m_iCheckDelayTickToStart) && 
        //     (GetLeftTick() < m_pInfo.m_iCheckDelayTickToStart))
        //     return false;

        // if ((0 != m_pInfo.m_iCheckDelayTickToLate) &&
        //     (GetLeftTick() > m_pInfo.m_iCheckDelayTickToLate))
        //     return false;

        return true;
    }
    public void DeleteDamage()
    {
        PlaySound(eDamageEvent.Delete);
        PlayEffect(eDamageEvent.Delete);
        AddDamage(eDamageEvent.Delete);

        m_bIsDieDamage = true;

        if (null != m_pParam)
            m_pParam.SendEventToDelete(this);
    }
    #endregion


    #region Event Handler
    #endregion


    #region Editor Functions
    public void AddTarget(int iIndex, string strTag)
    {
        m_pSettingInfo.m_pTargetUnitTags[iIndex] = strTag;
    }
    #endregion
}
