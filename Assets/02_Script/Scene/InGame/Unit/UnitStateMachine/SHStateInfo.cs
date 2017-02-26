using UnityEngine;
using System;
using System.Collections;

public class SHStateInfo
{
    #region Members
    public int              m_iStateID      = -1;
    public int              m_iFixedTick    = -1;

    public string           m_strAnimClip   = string.Empty;
    public Action<int, int> m_OnEnter       = null; // <iBeforeState,  iCurrentState>
    public Action<int, int> m_OnExit        = null; // <iBeforeState,  iCurrentState>
    public Action<int, int> m_OnFixedUpdate = null; // <iCurrentState, iFixedTick>
    public Action<int>      m_OnEndAnim     = null; // <iCurrentState>
    #endregion


    #region Interface Functions
    public void OnEnterState(int iBeforeState)
    {
        if (null == m_OnEnter)
            return;

        m_OnEnter(iBeforeState, m_iStateID);
    }
    public void OnExitState(int iAfterState)
    {
        if (null == m_OnExit)
            return;

        m_OnExit(m_iStateID, iAfterState);
    }
    public void OnFixedUpdate()
    {
        if (null == m_OnFixedUpdate)
            return;

        m_OnFixedUpdate(m_iStateID, m_iFixedTick);
    }
    public void OnEndAnimation()
    {
        if (null == m_OnEndAnim)
            return;

        m_OnEndAnim(m_iStateID);
    }
    #endregion
}