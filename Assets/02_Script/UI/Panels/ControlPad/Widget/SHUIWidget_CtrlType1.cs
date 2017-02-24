using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType1 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] public SHUIJoystick m_pJoyStick  = null;
    #endregion


    #region Members : Event
    private Action<Vector3> m_pEventMove      = null;
    private Action<Vector3> m_pEventDirection = null;
    private Action          m_pEventShoot     = null;
    #endregion


    #region System Functions
    public override void Start()
    {
        if (null != m_pJoyStick)
            m_pJoyStick.m_pEventToDrag = OnEventToDrag;
    }
    #endregion


    #region Interface Functions
    public void Initialize(Action<Vector3> m_pMove, Action<Vector3> m_pDirection, Action m_pShoot)
    {
        m_pEventMove      = m_pMove;
        m_pEventDirection = m_pDirection;
        m_pEventShoot     = m_pShoot;
    }
    public void Clear()
    {
        m_pEventMove      = null;
        m_pEventDirection = null;
        m_pEventShoot     = null;
    }
    #endregion


    #region UI Event Functions
    public void OnEventToDrag(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        if (null != m_pEventMove)
            m_pEventMove(vDirection);

        if (null != m_pEventDirection)
            m_pEventDirection(vDirection);
    }

    public void OnClickToRightButton()
    {
        if (null != m_pEventShoot)
            m_pEventShoot();
    }
    #endregion
}