using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_Store : SHUIBasePanel
{
    #region Members : Inspector
    [SerializeField] private UIToggle           m_pToggleStick   = null;
    [SerializeField] private UIToggle           m_pToggleMonster = null;
    [SerializeField] private SHUIScroll_Stick   m_pTabStick      = null;
    [SerializeField] private SHUIScroll_Monster m_pTabMonster    = null;
    #endregion


    #region Members : Event
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if (null != m_pToggleStick)
            m_pToggleStick.value   = true;

        if (null != m_pToggleMonster)
            m_pToggleMonster.value = false;
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnToggleToStick(bool bIsOn)
    {
        if (null == m_pTabStick)
            return;

        m_pTabStick.Initialize();
        m_pTabStick.gameObject.SetActive(bIsOn);
    } 
	public void OnToggleToMonster(bool bIsOn)
	{
        if (null == m_pTabMonster)
            return;

        m_pTabMonster.Initialize();
        m_pTabMonster.gameObject.SetActive(bIsOn);
    }
    #endregion
}
