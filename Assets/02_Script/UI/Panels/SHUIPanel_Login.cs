using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_Login : SHUIBasePanel
{
    #region Members : Inspector
    #endregion


    #region Members : Event
    private Action<bool> m_pEventToLogin = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 > pArgs.Length))
            return;

        m_pEventToLogin = ((Action<bool>)pArgs[0]);
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    void SendEventToLogin(bool bIsSuccess)
    {
        if (null == m_pEventToLogin)
            return;

        m_pEventToLogin(bIsSuccess);
    }
    #endregion


    #region Event Handler
    public void OnClickToGoggle()
    {
        Single.UI.ShowNotice_NoMake();
    }
    public void OnClickToFacebook()
    {
        Single.UI.ShowNotice_NoMake();
    }
    public void OnClickToGuast()
    {
        SendEventToLogin(true);
    }
    #endregion
}
