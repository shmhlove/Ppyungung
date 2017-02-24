using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType2 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private SHUIJoystick m_pJoyStick_Left  = null;
    [SerializeField] private SHUIJoystick m_pJoyStick_Right = null;
    [SerializeField] private float        m_fShootDelay     = 0.2f;
    #endregion


    #region Members : Event
    private Action<Vector3> m_pEventMove      = null;
    private Action<Vector3> m_pEventDirection = null;
    private Action          m_pEventShoot     = null;
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

#if UNITY_EDITOR
        SHHard.m_fPlayerAutoShoot = m_fShootDelay;
#else
        SHHard.m_fPlayerAutoShoot = SHPlayerPrefs.GetFloat("Player_ShootSpeed", m_fShootDelay);
#endif
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


#region Coroutine Functions
    IEnumerator CoroutineToShoot()
    {
        while (true)
        {
            if (null != m_pEventShoot)
                m_pEventShoot();

            yield return new WaitForSeconds(SHHard.m_fPlayerAutoShoot);
        }
    }
#endregion


#region UI Event Functions
    public void OnEventToDragLeft(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        if (null != m_pEventMove)
            m_pEventMove(vDirection);
    }

    public void OnEventToDragRight(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        if (null != m_pEventDirection)
            m_pEventDirection(vDirection);
    }

    public void OnEventToPressOnRight()
    {
        StartCoroutine(CoroutineToShoot());
    }
    public void OnEventToPressOffRight()
    {
        StopAllCoroutines();
    }
#endregion
}