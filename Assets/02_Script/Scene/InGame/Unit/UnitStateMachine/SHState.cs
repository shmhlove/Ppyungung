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
    #region Members : Basic
    [Header("State Info")]
    [ReadOnlyField] public  int        m_iCurrentStateID  = -1;
    [ReadOnlyField] public  int        m_iBeforeStateID   = -1;
    [ReadOnlyField] public  int        m_iFixedTick       = -1;
                    private DicState   m_dicState         = new DicState();
    [ReadOnlyField] public  bool       m_bIsStop          = false;
    #endregion


    #region Members : Animation
    [Header("Animation Info")]
    [SerializeField] public GameObject m_pAnimRoot        = null;
    #endregion


    // AutoFlow : 등록한 함수를 순차적으로 호출해주는 기능
    // AddAutoFlowState()를 호출한 순서대로 함수를 호출해주며,
    // ReturnValue의 타입에 따라 다음으로 넘길지 말지를 결정.
    #region Members : AutoFlowState
    private ListAutoFlow m_pAutoFlowState = new ListAutoFlow();
    #endregion


    #region Virtual Functions
    public virtual void RegisterState() { }
    #endregion


    #region System Functions
    public override void Awake()
    {
        base.Awake();
        RegisterState();
    }
    public override void OnEnable()
    {
        m_bIsStop = false;
    }
    public void FrameMove()
    {
        if (false == IsActive())
            return;

        if (true == m_bIsStop)
            return;

        CallToAutoFlow();
        CallToFixedUpdate();
    }
    #endregion


    #region Interface : State
    public SHStateInfo CreateState(int iStateID)
    {
        AddStateInfo(iStateID, new SHStateInfo()
        {
            m_iStateID = iStateID,
        });
        return GetStateInfo(iStateID);
    }
    public void AddStateInfo(int iStateID, SHStateInfo pInfo)
    {
        if (null == pInfo)
        {
            Debug.LogErrorFormat("SHState::AddState - StateInfo Is Null!! : {0}", iStateID);
            return;
        }

        if (true == m_dicState.ContainsKey(iStateID))
            m_dicState[iStateID] = pInfo;
        else
            m_dicState.Add(iStateID, pInfo);
    }
    public SHStateInfo GetStateInfo(int iStateID)
    {
        if (false == m_dicState.ContainsKey(iStateID))
            return null;

        return m_dicState[iStateID];
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
    public bool IsState(int iStateID)
    {
        return (m_iCurrentStateID == iStateID);
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