using UnityEngine;
using System;
using System.Collections;

using LunarConsolePlugin;

public class SHUIPanel_Development : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Cheat Open")]
    [SerializeField] private Animation      m_pAnimOpen         = null;
    [Header("Information")]
    [SerializeField] private GameObject     m_pInfoRoot         = null;
    [SerializeField] private UILabel        m_pLabel_ID         = null;
    [SerializeField] private UILabel        m_pLabel_Name       = null;
    [Header("Player")]
    [SerializeField] private UIPopupList    m_pPopupListToCtrl  = null;
    #endregion


    #region Members : Info
    private bool m_bIsUpdated = false;
    #endregion


    #region System Functions
    public override void Start()
    {
        SetPlayerCtrlType();
    }
    public override void Update()
    {
        m_bIsUpdated = true;
        SetLogToGoogleID();
        SetLogToGoogleUserName();
    }
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    #endregion


    # region Utility Functions
    void SetLogToGoogleID()
    {
        if (null == m_pLabel_ID)
            return;

        var strID = Single.Google.GetUserID();
        if (true == string.IsNullOrEmpty(strID))
            m_pLabel_ID.text = "Not Login";
        else
            m_pLabel_ID.text = strID;
    }
    void SetLogToGoogleUserName()
    {
        if (null == m_pLabel_Name)
            return;

        var strUserName = Single.Google.GetUserName();
        if (true == string.IsNullOrEmpty(strUserName))
            m_pLabel_Name.text = "Not Login";
        else
            m_pLabel_Name.text = strUserName;
    }
    void SetPlayerCtrlType()
    {
        if (null == m_pPopupListToCtrl)
            return;

        m_pPopupListToCtrl.Clear();
        SHUtils.ForToEnum<eControlType>((eType) => m_pPopupListToCtrl.AddItem(eType.ToString()));

        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        m_pPopupListToCtrl.Set(pCtrlUI.m_eCtrlType.ToString());
    }
    #endregion


    #region Event : Opener
    public void OnClickToOpener(bool bIsOpen)
    {
        if (null == m_pAnimOpen)
            return;

        if (false == m_bIsUpdated)
            return;

        if (true == bIsOpen)
            m_pAnimOpen.Play("Anim_Cheat_Open");
        else
            m_pAnimOpen.Play("Anim_Cheat_Close");

        SetPlayerCtrlType();
    }
    #endregion


    #region Event : Log
    public void OnClickToConsole()
    {
        LunarConsole.Show();
    }
    public void OnClickToShowLogInfo()
    {
        if (null == m_pInfoRoot)
            return;

        m_pInfoRoot.SetActive(false == m_pInfoRoot.activeInHierarchy);
    }
    #endregion


    #region Event : Google
    public void OnClickToLogin()
    {
        Single.Google.Login((bIsSuccess) => { });
    }
    public void OnClickToLogout()
    {
        Single.Google.Logout();
    }
    #endregion


    #region Event : Player
    public void OnSubmitToMoveSpeed(string strValue)
    {
        SHHard.m_fPlayerMoveSpeed = float.Parse(strValue);

#if UNITY_EDITOR
#else
        SHPlayerPrefs.SetFloat("Player_MoveSpeed", SHHard.m_fPlayerMoveSpeed);
#endif
    }
    public void OnSubmitToShootSpeed(string strValue)
    {
        SHHard.m_fPlayerAutoShoot = float.Parse(strValue);

#if UNITY_EDITOR
#else
        SHPlayerPrefs.SetFloat("Player_ShootSpeed", SHHard.m_fPlayerAutoShoot);
#endif
    }
    public void OnSelectToCtrlType(string strType)
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        var eType = SHUtils.GetStringToEnum<eControlType>(strType);
        pCtrlUI.SetCtrlType(eType);

#if UNITY_EDITOR
#else
        SHPlayerPrefs.SetInt("Player_CtrlType", (int)eType);
#endif
    }
    #endregion
}
