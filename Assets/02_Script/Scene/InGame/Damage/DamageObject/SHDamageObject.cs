using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SHAddDamageParam
{
    [ReadOnlyField]   public SHMonoWrapper                m_pWho              = null;   // 데미지 발생자
    [ReadOnlyField]   public GameObject                   m_pGuideTarget      = null;   // 쫓아갈 타켓 ( 유도 옵션이 있을경우 )
    [HideInInspector] public List<Action<SHDamageObject>> m_pEventToDelete    = new List<Action<SHDamageObject>>();
    [HideInInspector] public List<Action<SHDamageObject>> m_pEventToCollision = new List<Action<SHDamageObject>>();

    public SHAddDamageParam() { }
    public SHAddDamageParam(SHMonoWrapper pWho, GameObject pGuideTarget, 
        Action<SHDamageObject> pEventDelete, Action<SHDamageObject> pEventCollision)
    {
        m_pWho           = pWho;
        m_pGuideTarget   = pGuideTarget;
        AddEventToDelete(pEventDelete);
        AddEventToCollision(pEventCollision);
    }

    public void AddEventToDelete(Action<SHDamageObject> pEvent)
    {
        if (null == pEvent)
            return;

        m_pEventToDelete.Add(pEvent);
    }

    public void AddEventToCollision(Action<SHDamageObject> pEvent)
    {
        if (null == pEvent)
            return;

        m_pEventToCollision.Add(pEvent);
    }

    public void SendEventToDelete(SHDamageObject pDamage)
    {
        SHUtils.ForToList(m_pEventToDelete, (pCallback) => pCallback(pDamage));
    }

    public void SendEventToCollision(SHDamageObject pDamage)
    {
        SHUtils.ForToList(m_pEventToCollision, (pCallback) => pCallback(pDamage));
    }
}

public partial class SHDamageObject : SHMonoWrapper
{
    #region Members : ParamInfo
    [SerializeField]  public SHAddDamageParam m_pParam = null;
    #endregion


    #region Members : Inspector
    [SerializeField]  private SHDamageObjectInfo m_pSettingInfo = new SHDamageObjectInfo();
    [SerializeField]  public  SHDamageObjectInfo m_pInfo        = new SHDamageObjectInfo();
    #endregion


    #region Members : ETC
    [HideInInspector] public bool   m_bIsDieDamage = false;   // 데미지 라이프가 끝난 상태
    [HideInInspector] public int    m_iCrashHitTick = 0;      // 충돌 후 시간체크 (다단히트)
    [HideInInspector] public Bounds m_pBeforeBounds;          // 이전 위치의 Bounds
    #endregion
    

    #region System Functions
    public void OnInitialize(string strID, SHAddDamageParam pParam)
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

        SetupTransform();
        SetupPhysicsValue();
        ClearEffect();

		SetActive (false);
		SetActive (true);
        PlayAnimation();
        PlaySound(eDamageEvent.Start);
        PlayEffect(eDamageEvent.Start);
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
        PlaySound(eDamageEvent.Tick);
        PlayEffect(eDamageEvent.Tick);
    }
    #endregion


    #region Virtual Functions
    public override void OnCrashDamage(SHMonoWrapper pCrashObject)
    {
        m_iCrashHitTick = m_pInfo.m_iCheckDelayTickToCrash;

        PlaySound(eDamageEvent.Crash);
        PlayEffect(eDamageEvent.Crash);

        if (null != m_pParam)
            m_pParam.SendEventToCollision(this);
        
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
    public bool IsCheckCrash()
    {
        if (0 < m_iCrashHitTick)
            return false;

        if ((0 != m_pInfo.m_iCheckDelayTickToStart) && 
            (GetLeftTick() < m_pInfo.m_iCheckDelayTickToStart))
            return false;

        if ((0 != m_pInfo.m_iCheckDelayTickToLate) &&
            (GetLeftTick() > m_pInfo.m_iCheckDelayTickToLate))
            return false;
            
        return true;
    }
    public void DeleteDamage()
    {
        PlaySound(eDamageEvent.Delete);
        PlayEffect(eDamageEvent.Delete);

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
