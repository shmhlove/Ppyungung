using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType0 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private SHUIJoystick m_pJoyStick   = null;
    #endregion


    #region Members : Event
    private Action<Vector3> m_pEventMove      = null;
    private Action<Vector3> m_pEventDirection = null;
    private Action          m_pEventShoot     = null;
    private Action<bool>    m_pEventDash      = null;
    #endregion


    #region System Functions
    public override void Start()
    {
        if (null != m_pJoyStick)
        {
            m_pJoyStick.m_pEventToDrag = OnEventToDrag;
        }
    }
    #endregion


    #region Interface Functions
    public void Initialize(Action<Vector3> pMove, Action<Vector3> pDirection, Action pShoot, Action<bool> pDash)
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
    public void OnEventToDrag(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        if (null != m_pEventMove)
            m_pEventMove(vDirection);

        if (null != m_pEventDirection)
            m_pEventDirection(vDirection);
    }
    public void OnPressOnShoot()
    {
        StartCoroutine(CoroutineToShoot());
    }
    public void OnPressOffShoot()
    {
        StopAllCoroutines();
    }
    public void OnPressOnDash()
    {
        if (null != m_pEventDash)
            m_pEventDash(true);
    }
    public void OnPressOffDash()
    {
        if (null != m_pEventDash)
            m_pEventDash(true);
    }
    #endregion
}