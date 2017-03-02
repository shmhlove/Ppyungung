using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_ResultMenu : SHUIBasePanel
{
    #region Members : Inspector
    #endregion


    #region Members : Event
    private Action m_pEventToRestart = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 > pArgs.Length))
            return;

        m_pEventToRestart = (Action)pArgs[0];
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnClickToStartGame()
    {
        if (null == m_pEventToRestart)
            return;

        m_pEventToRestart();
        Close();
    }
    #endregion
}