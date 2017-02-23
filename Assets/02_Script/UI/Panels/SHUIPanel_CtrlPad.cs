using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_CtrlPad : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Ctrl Widgets")]
    [SerializeField] public SHUIJoystick m_pJoystick = null;
    #endregion


    #region Virtual Functions
    private Action m_pEventShoot = null;
    #endregion


    #region Utility : Event
    public void AddEventToDrag(Action<Vector3,Vector3,Vector3> pEvent)
    {
        if (null == m_pJoystick)
            return;

        m_pJoystick.m_pEventToDrag = pEvent;
    }
    public void DelEventToDrag()
    {
        if (null == m_pJoystick)
            return;

        m_pJoystick.m_pEventToDrag = null;
    }
    public void AddEventToShoot(Action pEvent)
    {
        m_pEventShoot = pEvent;
    }
    public void DelEventToShoot()
    {
        m_pEventShoot = null;
    }
    #endregion


    #region Event Handler
    public void OnClickToShoot()
    {
        if (null == m_pEventShoot)
            return;

        m_pEventShoot();
    }
    #endregion
}
