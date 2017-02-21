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
    #endregion


    #region Event Handler
    #endregion
}
