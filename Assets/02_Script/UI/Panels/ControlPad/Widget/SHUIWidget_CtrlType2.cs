using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType2 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private SHUIJoystick m_pJoyStick_Left  = null;
    [SerializeField] private SHUIJoystick m_pJoyStick_Right = null;
    #endregion


    #region Members : Info
    private bool m_bIsRightDrag  = false;
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
        if (null != m_pJoyStick_Left)
        {
            m_pJoyStick_Left.m_pEventToDrag      = OnEventToDragLeft;
        }

        if (null != m_pJoyStick_Right)
        {
            m_pJoyStick_Right.m_pEventToDrag     = OnEventToDragRight;
            m_pJoyStick_Right.m_pEventToPressOn  = OnEventToPressOnRight;
            m_pJoyStick_Right.m_pEventToPressOff = OnEventToPressOffRight;
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


    #region Coroutine Functions
    IEnumerator CoroutineToShoot()
    {
        while (true)
        {
            if (null != m_pEventShoot)
                m_pEventShoot();

            yield return new WaitForSeconds(SHHard.m_fCharShootDelay);
        }
    }
    #endregion


    #region UI Event Functions
    public void OnEventToDragLeft(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        if (null != m_pEventMove)
            m_pEventMove(vDirection);

        if (false == m_bIsRightDrag)
        {
            if (null != m_pEventDirection)
                m_pEventDirection(vDirection);
        }
    }
    public void OnEventToPressOnLeft()
    {
        Single.Player.m_bIsMoving = true;
    }
    public void OnEventToPressOffLeft()
    {
        Single.Player.m_bIsMoving = false;
    }
    public void OnEventToDragRight(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        m_bIsRightDrag = true;

        if (null != m_pEventDirection)
            m_pEventDirection(vDirection);
    }
    public void OnEventToPressOnRight()
    {
        m_bIsRightDrag = false;

        StartCoroutine(CoroutineToShoot());
    }
    public void OnEventToPressOffRight()
    {
        m_bIsRightDrag = false;

        StopAllCoroutines();
    }
    public void OnEventToDash()
    {
        if (null == m_pEventDash)
            return;

        m_pEventDash();
    }
    #endregion
}