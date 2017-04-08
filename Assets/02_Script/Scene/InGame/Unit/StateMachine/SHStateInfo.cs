using UnityEngine;
using System;
using System.Collections;

public class SHStateInfo
{
    #region Members : Basic
    public int              m_iStateID      = -1;
    public int              m_iFixedTick    = -1;
    public string           m_strAnimClip   = string.Empty;
    #endregion


    #region Members : Callback
    public Action<int> m_OnEnter       = null; // (int iBeforeState)
    public Action<int> m_OnExit        = null; // (int iAfterState)
    public Action<int> m_OnFixedUpdate = null; // (int iFixedTick)
    public Action      m_OnEndAnim     = null; // ()
    #endregion


    #region Interface Functions
    public void OnEnterState(int iBeforeState)
    {
        if (null == m_OnEnter)
            return;

        m_OnEnter(iBeforeState);
    }
    public void OnExitState(int iAfterState)
    {
        if (null == m_OnExit)
            return;

        m_OnExit(iAfterState);
    }
    public void OnFixedUpdate()
    {
        if (null == m_OnFixedUpdate)
            return;

        m_OnFixedUpdate(m_iFixedTick);
    }
    public void OnEndAnimation()
    {
        if (null == m_OnEndAnim)
            return;

        m_OnEndAnim();
    }
    #endregion
}