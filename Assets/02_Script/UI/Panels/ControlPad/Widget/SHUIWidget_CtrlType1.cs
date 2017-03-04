using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType1 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] public SHUIJoystick m_pJoyStick  = null;
    #endregion


    #region Members : Info
    private DateTime        m_pPressTime;
    #endregion


    #region Members : Event
    private Action<Vector3> m_pEventMove      = null;
    private Action<Vector3> m_pEventDirection = null;
    private Action          m_pEventShoot     = null;
    private Action          m_pEventDash      = null;
    #endregion


    #region System Functions
    public override void Start()
    {
        if (null != m_pJoyStick)
        {
            m_pJoyStick.m_pEventToDrag    = OnEventToDrag;
            m_pJoyStick.m_pEventToPressOn = OnEventToPressOn;
        }
    }
    #endregion


    #region Interface Functions
    public void Initialize(Action<Vector3> pMove, Action<Vector3> pDirection, Action pShoot, Action pDash)
    {
        m_pEventMove      = pMove;
        m_pEventDirection = pDirection;
        m_pEventShoot     = pShoot;
        m_pEventDash      = pDash;
    }
    public void Clear()
    {
        m_pEventMove      = null;
        m_pEventDirection = null;
        m_pEventShoot     = null;
        m_pEventDash      = null;
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
    public void OnEventToPressOn()
    {
        float fTimeGap = (float)(DateTime.Now - m_pPressTime).TotalMilliseconds / 1000.0f;
        if (fTimeGap < 0.5f)
        {
            if (null != m_pEventDash)
                m_pEventDash();
        }

        m_pPressTime = DateTime.Now;
    }
    public void OnClickToRightButton()
    {
        Single.Player.m_bIsAttacking = true;

        if (null != m_pEventShoot)
            m_pEventShoot();

        Single.Player.m_bIsAttacking = false;
    }
    #endregion
}