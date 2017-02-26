using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHState : SHMonoWrapper
{
    #region Utility : AutoFlow
    void CallToAutoFlow()
    {
        if (false == IsExistAutoFlowState())
            return;

        var pFunc = m_pAutoFlowState[0];
        switch(pFunc(m_iCurrentStateID))
        {
            case eReturnAutoFlow.Next:
                m_pAutoFlowState.Remove(pFunc);
                break;
        }
    }
    #endregion


    #region Utility : State
    void CallToFixedUpdate()
    {
        var pState = GetCurrentState();
        if (null == pState)
            return;

        pState.m_iFixedTick = ++m_iFixedTick;
        pState.OnFixedUpdate();
    }   
    SHStateInfo GetCurrentState()
    {
        return GetStateInfo(m_iCurrentStateID);
    }
    #endregion
}