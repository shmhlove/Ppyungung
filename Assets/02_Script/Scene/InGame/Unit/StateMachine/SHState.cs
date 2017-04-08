using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicState     = System.Collections.Generic.Dictionary<int, SHStateInfo>;
using ListAutoFlow = System.Collections.Generic.List<System.Func<int, eReturnAutoFlow>>;

public enum eReturnAutoFlow
{
    Keep,
    Next,
}

public partial class SHState : SHMonoWrapper
{
    #region Members : Inpector
    [Header("[State Info]")]
    [ReadOnlyField] public  int        m_iCurrentStateID  = -1;
    [ReadOnlyField] public  int        m_iBeforeStateID   = -1;
    [ReadOnlyField] public  int        m_iFixedTick       = -1;
    [Header("[Animation Info]")]
    [SerializeField] public GameObject m_pAnimRoot        = null;
    #endregion


    #region Members : Info
    private DicState     m_dicState       = new DicState();
    private ListAutoFlow m_pAutoFlowState = new ListAutoFlow();
    #endregion
    

    #region Virtual Functions
    public override void Awake()
    {
        base.Awake();
        RegisterState();
    }
    public virtual void FrameMove()
    {
        if (false == IsActive())
            return;

        if (true == m_bIsPause)
            return;

        CallToAutoFlow();
        CallToFixedUpdate();
    }
    public virtual void RegisterState() { }
    #endregion


    #region Interface : State
    public SHStateInfo CreateState<T>(T pState)
    {
        return CreateState(Convert.ToInt32(pState));
    }
    public SHStateInfo CreateState(int iStateID)
    {
        return AddStateInfo(iStateID, new SHStateInfo()
        {
            m_iStateID = iStateID,
        });
    }
    public void ChangeState<T>(T pState)
    {
        ChangeState(Convert.ToInt32(pState));
    }
    public void ChangeState(int iChangeStateID)
    {
        var pChangeState = GetStateInfo(iChangeStateID);
        if (null == pChangeState)
            return;

        var pCurrentState = GetStateInfo(m_iCurrentStateID);
        if (null != pCurrentState)
            pCurrentState.OnExitState(iChangeStateID);
        
        m_iBeforeStateID          = m_iCurrentStateID;
        m_iCurrentStateID         = iChangeStateID;
        pChangeState.m_iFixedTick = (m_iFixedTick = -1);
        pChangeState.OnEnterState(m_iBeforeStateID);

        PlayAnimation(pChangeState);
    }
    public bool IsState<T>(T pState)
    {
        return (m_iCurrentStateID == Convert.ToInt32(pState));
    }
    #endregion


    #region Interface : AutoFlow
    public bool IsExistAutoFlowState()
    {
        return (0 != m_pAutoFlowState.Count);
    }
    public void AddAutoFlowState(Func<int, eReturnAutoFlow> pStateFunc)
    {
        m_pAutoFlowState.Add(pStateFunc);
    }
    #endregion
}