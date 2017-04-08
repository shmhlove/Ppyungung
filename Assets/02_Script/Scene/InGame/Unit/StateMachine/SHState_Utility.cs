using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHState : SHMonoWrapper
{
    #region Utility : State
    SHStateInfo AddStateInfo(int iStateID, SHStateInfo pInfo)
    {
        if (null == pInfo)
        {
            Debug.LogErrorFormat("SHState::AddState - StateInfo Is Null!! : {0}", iStateID);
            return pInfo;
        }

        if (true == m_dicState.ContainsKey(iStateID))
            m_dicState[iStateID] = pInfo;
        else
            m_dicState.Add(iStateID, pInfo);

        return pInfo;
    }
    SHStateInfo GetStateInfo(int iStateID)
    {
        if (false == m_dicState.ContainsKey(iStateID))
            return null;

        return m_dicState[iStateID];
    }
    SHStateInfo GetCurrentState()
    {
        return GetStateInfo(m_iCurrentStateID);
    }
    void CallToFixedUpdate()
    {
        var pState = GetCurrentState();
        if (null == pState)
            return;

        pState.m_iFixedTick = ++m_iFixedTick;
        pState.OnFixedUpdate();
    }   
    #endregion


    #region Utility : AutoFlow
    void CallToAutoFlow()
    {
        if (false == IsExistAutoFlowState())
            return;

        var pFunc = m_pAutoFlowState[0];
        switch (pFunc(m_iCurrentStateID))
        {
            case eReturnAutoFlow.Next:
                m_pAutoFlowState.Remove(pFunc);
                break;
        }
    }
    #endregion
}