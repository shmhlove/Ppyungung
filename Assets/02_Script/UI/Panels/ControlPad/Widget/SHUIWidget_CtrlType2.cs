﻿using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_CtrlType2 : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private SHUIJoystick m_pJoyStick_Left  = null;
    [SerializeField] private SHUIJoystick m_pJoyStick_Right = null;
    #endregion


    #region Members : Info
    private bool m_bIsLeftDrag   = false;
    private bool m_bIsRightDrag  = false;
    #endregion


    #region Members : Event
    private Action<Vector3> m_pEventMove      = null;
    private Action<Vector3> m_pEventDirection = null;
    private Action<bool>    m_pEventShoot     = null;
    private Action<bool>    m_pEventDash      = null;
    #endregion


    #region System Functions
    public override void Start()
    {
        if (null != m_pJoyStick_Left)
        {
            m_pJoyStick_Left.m_pEventToDrag      = OnEventToDragLeft;
            m_pJoyStick_Left.m_pEventToPressOn   = OnEventToPressOnLeft;
            m_pJoyStick_Left.m_pEventToPressOff  = OnEventToPressOffLeft;
        }

        if (null != m_pJoyStick_Right)
        {
            m_pJoyStick_Right.m_pEventToDrag     = OnEventToDragRight;
            m_pJoyStick_Right.m_pEventToPressOn  = OnEventToPressOnRight;
            m_pJoyStick_Right.m_pEventToPressOff = OnEventToPressOffRight;
        }
    }
    
    bool[] m_bIsLeftKeyDown  = new bool[4];
    bool[] m_bIsRightKeyDown = new bool[4];
    public override void Update()
    {
#if UNITY_EDITOR
        if (true == Single.Input.IsTouch())
            return;

        // Right 조작
        if (true == Input.GetKeyDown(KeyCode.Keypad8))
            m_bIsRightKeyDown[0] = true;
        if (true == Input.GetKeyUp(KeyCode.Keypad8))
            m_bIsRightKeyDown[0] = false;
        if (true == Input.GetKeyDown(KeyCode.Keypad5))
            m_bIsRightKeyDown[1] = true;
        if (true == Input.GetKeyUp(KeyCode.Keypad5))
            m_bIsRightKeyDown[1] = false;
        if (true == Input.GetKeyDown(KeyCode.Keypad4))
            m_bIsRightKeyDown[2] = true;
        if (true == Input.GetKeyUp(KeyCode.Keypad4))
            m_bIsRightKeyDown[2] = false;
        if (true == Input.GetKeyDown(KeyCode.Keypad6))
            m_bIsRightKeyDown[3] = true;
        if (true == Input.GetKeyUp(KeyCode.Keypad6))
            m_bIsRightKeyDown[3] = false;
        
        var vRightDirection = Vector3.zero;
        if (true == m_bIsRightKeyDown[0]) vRightDirection.y = 1.0f;
        if (true == m_bIsRightKeyDown[1]) vRightDirection.y = -1.0f;
        if (true == m_bIsRightKeyDown[2]) vRightDirection.x = -1.0f;
        if (true == m_bIsRightKeyDown[3]) vRightDirection.x = 1.0f;
        
        if (Vector3.zero != vRightDirection)
        {
            if (false == m_bIsRightDrag)
                OnEventToPressOnRight();
        
            OnEventToDragRight(Vector3.zero, Vector3.zero, vRightDirection.normalized);
        }
        else
        {
            if (true == m_bIsRightDrag)
                OnEventToPressOffRight();
        }

        // Left 조작
        if (true == Input.GetKeyDown(KeyCode.W))    m_bIsLeftKeyDown[0] = true;
        if (true == Input.GetKeyUp(KeyCode.W))      m_bIsLeftKeyDown[0] = false;
        if (true == Input.GetKeyDown(KeyCode.S))    m_bIsLeftKeyDown[1] = true;
        if (true == Input.GetKeyUp(KeyCode.S))      m_bIsLeftKeyDown[1] = false;
        if (true == Input.GetKeyDown(KeyCode.A))    m_bIsLeftKeyDown[2] = true;
        if (true == Input.GetKeyUp(KeyCode.A))      m_bIsLeftKeyDown[2] = false;
        if (true == Input.GetKeyDown(KeyCode.D))    m_bIsLeftKeyDown[3] = true;
        if (true == Input.GetKeyUp(KeyCode.D))      m_bIsLeftKeyDown[3] = false;
        
        var vLeftDirection = Vector3.zero;
        if (true == m_bIsLeftKeyDown[0]) vLeftDirection.y = 1.0f;
        if (true == m_bIsLeftKeyDown[1]) vLeftDirection.y = -1.0f;
        if (true == m_bIsLeftKeyDown[2]) vLeftDirection.x = -1.0f;
        if (true == m_bIsLeftKeyDown[3]) vLeftDirection.x = 1.0f;
        
        if (Vector3.zero != vLeftDirection)
        {
            if (false == m_bIsLeftDrag)
                OnEventToPressOnLeft();

            OnEventToDragLeft(Vector3.zero, Vector3.zero, vLeftDirection.normalized);
        }
        else
        {
            if (true == m_bIsLeftDrag)
                OnEventToPressOffLeft();
        }

        // Dash조작
        {
            if (true == Input.GetKeyDown(KeyCode.Space))
                OnPressOnDash();
            if (true == Input.GetKeyUp(KeyCode.Space))
                OnPressOffDash();
        }
#endif
    }
    #endregion


    #region Interface Functions
    public void Initialize(Action<Vector3> pMove, Action<Vector3> pDirection, Action<bool> pShoot, Action<bool> pDash)
    {
        m_pEventMove      = pMove;
        m_pEventDirection = pDirection;
        m_pEventShoot     = pShoot;
        m_pEventDash      = pDash;
    }
    public void Clear()
    {
        m_bIsLeftDrag     = false;
        m_bIsRightDrag    = false;

        m_bIsLeftKeyDown  = new bool[4];
        m_bIsRightKeyDown = new bool[4];
    }
    #endregion


    #region UI Event Functions
    public void OnEventToDragLeft(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        m_bIsLeftDrag = true;

        if (null != m_pEventMove)
            m_pEventMove(vDirection);

        if (false == m_bIsRightDrag)
        {
            if (null != m_pEventDirection)
                m_pEventDirection(vDirection);
        }
    }
    public void OnEventToDragRight(Vector3 vCenter, Vector3 vThumb, Vector3 vDirection)
    {
        m_bIsRightDrag = true;

        if (null != m_pEventDirection)
            m_pEventDirection(vDirection);
    }
    public void OnEventToPressOnLeft()
    {
        m_bIsLeftDrag  = false;
        m_pEventMove(Vector3.zero);
    }
    public void OnEventToPressOffLeft()
    {
        m_bIsLeftDrag  = false;
        m_pEventMove(Vector3.zero);
    }
    public void OnEventToPressOnRight()
    {
        m_bIsRightDrag  = false;
        m_pEventDirection(Vector3.zero);

        if (null != m_pEventShoot)
            m_pEventShoot(true);
    }
    public void OnEventToPressOffRight()
    {
        m_bIsRightDrag  = false;
        m_pEventDirection(Vector3.zero);

        if (null != m_pEventShoot)
            m_pEventShoot(false);
    }
    public void OnPressOnDash()
    {
        if (null != m_pEventDash)
            m_pEventDash(true);
    }
    public void OnPressOffDash()
    {
        if (null != m_pEventDash)
            m_pEventDash(false);
    }
    #endregion
}