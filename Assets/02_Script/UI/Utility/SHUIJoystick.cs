using UnityEngine;
using System;
using System.Collections;

public class SHUIJoystick : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private Transform     m_pThumb             = null;
    [SerializeField] private float         m_fMoveRadius        = 80.0f;
    [SerializeField] private bool          m_bIsCenterOnToPress = false;
    #endregion


    #region Members : Info
    private bool        m_bIsPressOn     = false;
    private Vector3     m_vBeforePos     = Vector2.zero;
    private Vector3     m_vCurrentPos    = Vector2.zero;
    private Vector3     m_vSpringSpeed   = Vector3.zero;
    #endregion


    #region Members : Event
    public Action                            m_pEventToPressOn  = null;
    public Action                            m_pEventToPressOff = null;
    public Action<Vector3, Vector3, Vector3> m_pEventToDrag     = null;
    #endregion
    

    #region System Functions
    public override void Start()
    {
    }
    public override void Update()
    {
        UpdateSpring();
        UpdateDrag();
    }
    public override void OnEnable()
    {
        SetThumbLocalPos(Vector3.zero);
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if (true == SHApplicationInfo.m_bIsAppQuit)
            return;

        SetThumbLocalPos(Vector3.zero);
    }
    #endregion


    #region Utility Functions
    void UpdateDrag()
    {
        if (false == m_bIsPressOn)
            return;

        if (Vector3.zero == GetThumbDirection())
            return;

        CallEventToDrag(transform.position, GetThumbWorldPos(), GetThumbDirection());
    }
    void UpdateSpring()
    {
        if (true == m_bIsPressOn)
            return;

        if (SHMath.EPSILON > SHMath.GetMagnitude(GetPosition(), GetThumbWorldPos()))
            return;
        
        SetThumbWorldPos(SHPhysics.CalculationSpring(GetPosition(), GetThumbWorldPos(), ref m_vSpringSpeed, 1000.0f, 20.0f));
    }
    void SetThumbWorldPos(Vector3 vPos)
    {
        if (null == m_pThumb)
            return;

        m_pThumb.position = vPos;
    }
    Vector3 GetThumbWorldPos()
    {
        if (null == m_pThumb)
            return Vector3.zero;

        return m_pThumb.position;
    }
    void SetThumbLocalPos(Vector3 vPos)
    {
        if (null == m_pThumb)
            return;

        m_pThumb.localPosition = vPos;
    }
    Vector3 GetThumbLocalPos()
    {
        if (null == m_pThumb)
            return Vector3.zero;

        return m_pThumb.localPosition;
    }
    void SetThumbDragPos(Vector3 vMovePos)
    {
        if (null == m_pThumb)
            return;

        SetThumbWorldPos(GetThumbWorldPos() + vMovePos);

        var vLocalPos = m_pThumb.localPosition;
        if (vLocalPos.magnitude > m_fMoveRadius)
            vLocalPos = Vector3.ClampMagnitude(vLocalPos, m_fMoveRadius);

        SetThumbLocalPos(vLocalPos);
    }
    Vector3 GetThumbDirection()
    {
        if (null == m_pThumb)
            return Vector3.zero;

        var pGap = (GetThumbWorldPos() - transform.position);
        if (0.01f >= pGap.magnitude)
            return Vector3.zero;

        return (GetThumbWorldPos() - transform.position).normalized;
    }
    Vector3 GetTouchPos()
    {
        var pRay      = UICamera.currentCamera.ScreenPointToRay(UICamera.lastEventPosition);
        var vTouchPos = pRay.GetPoint(0.0f);
        vTouchPos.z   = 0.0f;
        return vTouchPos;
    }
    Vector3 GetConvertPosToTouchPos(Vector3 vOldPos, Vector3 vCurPos)
    {
        if (null == m_pThumb)
            return Vector3.zero;

        var vOffset = (vCurPos - vOldPos);
        vOffset.z = 0.0f;

        if (false == Vector3.zero.Equals(vOffset))
        {
            vOffset = m_pThumb.InverseTransformDirection(vOffset);
            vOffset = m_pThumb.TransformDirection(vOffset);
        }
        
        return vOffset;
    }
    #endregion


    #region UI Event Functions
    public void OnPress(bool bPressed)
    {
        m_vBeforePos   = m_vCurrentPos = GetTouchPos();
        m_bIsPressOn   = bPressed;
        m_vSpringSpeed = Vector3.zero;

        if (true == bPressed)
        {
            if (true == m_bIsCenterOnToPress)
                SetPosition(m_vCurrentPos);
            
            CallEventToPressOn();
        }   
        else
        {
            CallEventToPressOff();
        }
    }
    public void OnDrag(Vector2 vDelta)
    {
        if (true == Vector2.zero.Equals(vDelta))
            return;
        
        m_vBeforePos  = m_vCurrentPos;
        m_vCurrentPos = GetTouchPos();
        
        SetThumbDragPos(GetConvertPosToTouchPos(m_vBeforePos, m_vCurrentPos));
    }
    void CallEventToPressOn()
    {
        if (null == m_pEventToPressOn)
            return;

        m_pEventToPressOn();
    }
    void CallEventToPressOff()
    {
        if (null == m_pEventToPressOff)
            return;

        m_pEventToPressOff();
    }
    void CallEventToDrag(Vector3 vCenterPos, Vector3 vThumbPos, Vector3 vDirection)
    {
        if (null == m_pEventToDrag)
            return;

        m_pEventToDrag(vCenterPos, vThumbPos, vDirection);
    }
    #endregion
}