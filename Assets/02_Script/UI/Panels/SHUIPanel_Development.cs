using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_Development : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Login Info")]
    [SerializeField] private UILabel m_pLabel_ID         = null;
    [SerializeField] private UILabel m_pLabel_UserName   = null;
    [SerializeField] private UILabel m_pLabel_SpeedValue = null;
    #endregion


    #region Members : Event
    #endregion


    #region System Functions
    public override void Update()
    {
        SetGoogleID();
        SetGoogleUserName();
    }
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    #endregion


    # region Utility Functions
    void SetGoogleID()
    {
        if (null == m_pLabel_ID)
            return;

        var strID = Single.Google.GetUserID();
        if (true == string.IsNullOrEmpty(strID))
            m_pLabel_ID.text = "Not Login";
        else
            m_pLabel_ID.text = strID;
    }
    void SetGoogleUserName()
    {
        if (null == m_pLabel_UserName)
            return;

        var strUserName = Single.Google.GetUserName();
        if (true == string.IsNullOrEmpty(strUserName))
            m_pLabel_UserName.text = "Not Login";
        else
            m_pLabel_UserName.text = strUserName;
    }
    void SetSpeedValue()
    {
        if (null == m_pLabel_SpeedValue)
            return;

        m_pLabel_SpeedValue.text = string.Format("Player Move Speed : {0}", SHHard.m_fPlayerMoveSpeed);
    }
    #endregion


    #region Event Handler
    public void OnClickToConsole()
    {
#if UNITY_EDITOR
        return;
#else
        LunarConsolePlugin.LunarConsole.Show();
#endif
    }
    public void OnClickToLogin()
    {
        Single.Google.Login((bIsSuccess) => { });
    }
    public void OnClickToLogout()
    {
        Single.Google.Logout();
    }
    public void OnClickToSpeedUp()
    {
        SHHard.m_fPlayerMoveSpeed += 1.0f;
        SetSpeedValue();
    }
    public void OnClickToSpeedDown()
    {
        SHHard.m_fPlayerMoveSpeed -= 1.0f;
        SetSpeedValue();
    }
    #endregion
}
