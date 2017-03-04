using UnityEngine;
using System;
using System.Collections;

using LunarConsolePlugin;

public class SHUIPanel_Development : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Cheat Open")]
    [SerializeField] private Animation      m_pAnimOpen              = null;
    [Header("Information")]
    [SerializeField] private GameObject     m_pInfoRoot              = null;
    [SerializeField] private UILabel        m_pLabelToID             = null;
    [SerializeField] private UILabel        m_pLabelToName           = null;
    [Header("Player")]
    [SerializeField] private UIPopupList    m_pPopupListToCtrl       = null;
    [SerializeField] private UIInput        m_pInputToCharMoveSpeed  = null;
    [SerializeField] private UIInput        m_pInputToCharShootSpeed = null;
    [SerializeField] private UIInput        m_pInputToCharDMGSpeed   = null;
    [Header("Monster")]
    [SerializeField] private UIInput        m_pInputToMonMaxSummon   = null;
    [SerializeField] private UIInput        m_pInputToOneTimeSummon  = null;
    [SerializeField] private UIInput        m_pInputToMonMoveSpeed   = null;
    [SerializeField] private UIInput        m_pInputToMonDMGSpeed    = null;
    [SerializeField] private UIInput        m_pInputToMonShootSpeed  = null;
    [Header("ETC")]
    [SerializeField] private UIInput        m_pInputToUnitScale      = null;
    [SerializeField] private UIInput        m_pInputToCameraSpeed    = null;
    [SerializeField] private UIInput        m_pInputToCameraOffsetX  = null;
    [SerializeField] private UIInput        m_pInputToCameraOffsetY  = null;
    #endregion


    #region Members : Info
    private bool m_bIsUpdated = false;
    #endregion


    #region System Functions
    public override void Start()
    {
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
        if (null == m_pLabelToID)
            return;

        var strID = Single.Google.GetUserID();
        if (true == string.IsNullOrEmpty(strID))
            m_pLabelToID.text = "Not Login";
        else
            m_pLabelToID.text = strID;
    }
    void SetLogToGoogleUserName()
    {
        if (null == m_pLabelToName)
            return;

        var strUserName = Single.Google.GetUserName();
        if (true == string.IsNullOrEmpty(strUserName))
            m_pLabelToName.text = "Not Login";
        else
            m_pLabelToName.text = strUserName;
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
    void SetInputInfo()
    {
        if ((null == m_pInputToCharMoveSpeed)  ||
            (null == m_pInputToCharShootSpeed) ||
            (null == m_pInputToCharDMGSpeed)   ||
            (null == m_pInputToMonMaxSummon)   ||
            (null == m_pInputToOneTimeSummon)  ||
            (null == m_pInputToMonMoveSpeed)   ||
            (null == m_pInputToMonDMGSpeed)    ||
            (null == m_pInputToUnitScale)      ||
            (null == m_pInputToCameraSpeed)    ||
            (null == m_pInputToCameraOffsetX)  ||
            (null == m_pInputToCameraOffsetY) ||
            (null == m_pInputToMonShootSpeed))
            return;

        m_pInputToCharMoveSpeed.value   = SHHard.m_fCharMoveSpeed.ToString();
        m_pInputToCharShootSpeed.value  = SHHard.m_fCharShootDelay.ToString();
        m_pInputToCharDMGSpeed.value    = SHHard.m_fCharDamageSpeed.ToString();
        m_pInputToMonMaxSummon.value    = SHHard.m_iMonMaxCount.ToString();
        m_pInputToOneTimeSummon.value   = SHHard.m_iMonMaxGen.ToString();
        m_pInputToMonMoveSpeed.value    = SHHard.m_fMonMoveSpeed.ToString();
        m_pInputToMonDMGSpeed.value     = SHHard.m_fMonDamageSpeed.ToString();
        m_pInputToMonShootSpeed.value   = SHHard.m_fMonShootDelay.ToString();
        m_pInputToUnitScale.value       = SHHard.m_fUnitScale.ToString();
        m_pInputToCameraSpeed.value     = SHHard.m_fCameraMoveSpeed.ToString();
        m_pInputToCameraOffsetX.value   = SHHard.m_fCameraLimitOffsetX.ToString();
        m_pInputToCameraOffsetY.value   = SHHard.m_fCameraLimitOffsetY.ToString();
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
        SetInputInfo();
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
        SHHard.m_fCharMoveSpeed = float.Parse(strValue);
    }
    public void OnSubmitToDamageSpeed(string strValue)
    {
        SHHard.m_fCharDamageSpeed = float.Parse(strValue);
    }
    public void OnSubmitToShootSpeed(string strValue)
    {
        SHHard.m_fCharShootDelay = float.Parse(strValue);
    }
    public void OnSelectToCtrlType(string strType)
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        var eType = SHUtils.GetStringToEnum<eControlType>(strType);
        pCtrlUI.SetCtrlType(eType);
    }
    #endregion


    #region Event : Monster
    public void OnClickToStartMonster()
    {
        Single.Monster.StartMonster();
    }
    public void OnClickToStopMonster()
    {
        Single.Monster.StopMonster();
    }
    public void OnClickToAllKillMonster()
    {
        Single.Monster.AllKillMonster();
    }
    public void OnSubmitToMonMaxSummon(string strValue)
    {
        SHHard.m_iMonMaxCount = int.Parse(strValue);
    }
    public void OnSubmitToMonOneTimeSummon(string strValue)
    {
        SHHard.m_iMonMaxGen = int.Parse(strValue);
    }
    public void OnSubmitToMonMoveSpeed(string strValue)
    {
        SHHard.m_fMonMoveSpeed = float.Parse(strValue);
    }
    public void OnSubmitToMonDamageSpeed(string strValue)
    {
        SHHard.m_fMonDamageSpeed = float.Parse(strValue);
    }
    public void OnSubmitToMonShootSpeed(string strValue)
    {
        SHHard.m_fMonShootDelay = float.Parse(strValue);
    }
    #endregion


    #region Event : ETC
    public void OnSubmitToUnitScale(string strValue)
    {
        SHHard.m_fUnitScale = float.Parse(strValue);

        var pPlayer = Single.Player.m_pCharacter;
        if (null != pPlayer)
        {
            pPlayer.SetLocalScale(pPlayer.m_vStartScale * SHHard.m_fUnitScale);
        }

        var pMosnters = Single.Monster.m_pMonsters;
        if (null != pMosnters)
        {
            foreach(var pMonster in pMosnters)
            {
                pMonster.SetLocalScale(pMonster.m_vStartScale * SHHard.m_fUnitScale);
            }
        }

        var dicDamages = Single.Damage.m_dicDamages;
        if (null != dicDamages)
        {
            foreach(var kvp in dicDamages)
            {
                kvp.Value.SetLocalScale(kvp.Value.m_vStartScale * SHHard.m_fUnitScale);
            }
        }
    }
    public void OnSubmitToCameraSpeed(string strValue)
    {
        SHHard.m_fCameraMoveSpeed = float.Parse(strValue);
    }
    public void OnSubmitToCameraOffsetX(string strValue)
    {
        SHHard.m_fCameraLimitOffsetX = float.Parse(strValue);
    }
    public void OnSubmitToCameraOffsetY(string strValue)
    {
        SHHard.m_fCameraLimitOffsetY = float.Parse(strValue);
    }
    #endregion
}
