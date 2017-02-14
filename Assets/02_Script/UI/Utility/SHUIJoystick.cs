using UnityEngine;
using System;
using System.Collections;

public class SHUIJoystick : MonoBehaviour
{
    //#region Members : Inspector
    //[SerializeField] private Transform    m_pStick             = null;
    //[SerializeField] private Vector3      m_vScale             = Vector3.one;
    //[SerializeField] private float        m_fm_fRadius         = 40.0f;
    //[SerializeField] private bool         m_bIsCenterOnToPress = true;

    ////Joystick vars
    //public bool normalize = false; 							// Normalize output after the dead-zone?
    //public float deadZone = 2f;								// Control when position is output
    //public float fadeOutAlpha = 0.2f;
    //public float fadeOutDelay = 1f;
    //public UIWidget[] widgetsToFade;						// UIWidgets that should fadeIn/Out when centerOnPress = true
    //public Transform[] widgetsToCenter;						// GameObjects to Center under users thumb when centerOnPress = true
    //public float doubleTapTimeWindow = 0.5f;				// time in Seconds to recognize a double tab
    //public GameObject doubleTapMessageTarget;
    //public string doubleTabMethodeName;

    //// by blueasa
    //public float damping = 1.0f;
    //public Vector3 m_vOldPos = Vector2.zero;
    //public Vector3 m_vCurrentPos = Vector2.zero;

    //// by shmhlove
    //private Vector3 m_vSpringSpeed = Vector3.zero;
    //public UIWidget m_pWidget = null;
    //#endregion


    //#region Members : Info
    //private Vector3 m_vStartTouchPos = Vector3.zero;
    //#endregion


    //#region System Functions
    //void Start()
    //{
    //    m_vStartTouchPos = m_pStick.position;
    //}

    //void Update()
    //{
    //    SetSpringStick();
    //}

    //void OnEnable()
    //{
    //    if (false == this.gameObject.activeInHierarchy)
    //        return;

    //    m_pStick.position = m_vStartTouchPos;
    //    SetPosToWidget(m_vStartTouchPos);

    //    m_vOldPos = Vector2.zero;
    //    m_vCurrentPos = Vector2.zero;
    //}

    //void OnDisable()
    //{
    //    m_pStick.position = m_vStartTouchPos;
    //    SetPosToWidget(m_vStartTouchPos);
    //}

    //public void OnPress(bool bPressed)
    //{
    //    if (m_pStick == null)
    //        return;

    //    if (true == bPressed)
    //        SetPressToOn();
    //    else
    //        SetPressToOff();
    //}

    //void OnDrag(Vector2 delta)
    //{
    //    if (m_pStick == null)
    //        return;

    //    if (0.0f == delta.magnitude)
    //        return;

    //    m_vOldPos = m_vCurrentPos;
    //    m_vCurrentPos = GetPosToTouch();

    //    // OldPos가 Zero(초기화 상태)이면 갑자기 조이스틱이 확 튀어가기 때문에 아래처럼 보완해줌
    //    if (Vector3.zero == m_vOldPos)
    //        m_vOldPos = m_vCurrentPos;

    //    // SGUtil.Log("Delta : {0}, Magnitude : {1}, OldPos : {2}, CurPos : {3}", delta, delta.magnitude, m_vOldPos, m_vCurrentPos);

    //    Vector3 vGap = GetConvertPosToTouchPos(m_vOldPos, m_vCurrentPos);
    //    SetTargetDragging(vGap);
    //}


    //// ------------------------------------------------------------
    //// 내부 유틸함수들
    //void SetPressToOn()
    //{
    //    StopAllCoroutines();
    //    SendMsgToDoubleTabMethode();
    //    TweenColorToFadeWidgets(Color.white, 1.0f, 0.1f, UITweener.Method.EaseIn);

    //    Vector3 vTouchPos = GetPosToTouch();

    //    if (true == m_bIsCenterOnToPress)
    //        m_vStartTouchPos = vTouchPos;

    //    m_vOldPos = vTouchPos;
    //    m_vCurrentPos = vTouchPos;

    //    SetPosToWidget(m_vStartTouchPos);
    //}

    //void SetPressToOff()
    //{
    //    // Release the finger control and set the joystick back to the default position
    //    // target.position = userInitTouchPos;
    //    SetPosToWidget(m_vStartTouchPos);

    //    m_vOldPos = Vector3.zero;
    //    m_vCurrentPos = Vector3.zero;
    //}

    //void SetTargetDragging(Vector3 vGap)
    //{
    //    m_pStick.position += vGap;

    //    // Calculate the length. This involves a squareroot operation,
    //    // so it's slightly expensive. We re-use this length for multiple
    //    // things below to avoid doing the square-root more than one.
    //    Vector3 vLocalPos = m_pStick.localPosition;
    //    float fLength = vLocalPos.magnitude;

    //    // If the length of the vector is smaller than the deadZone m_fRadius,
    //    // set the position to the origin.
    //    if (fLength < deadZone)
    //        vLocalPos = Vector2.zero;
    //    else if (fLength > m_fRadius)
    //        vLocalPos = Vector3.ClampMagnitude(vLocalPos, m_fRadius);

    //    m_pStick.localPosition = vLocalPos;
    //}

    //void SetSpringStick()
    //{
    //    if (false == IsZeroToTouchPos())
    //        return;

    //    Vector3 vCenterPos = m_vStartTouchPos;
    //    Vector3 vTargetPos = m_pStick.position;
    //    m_pStick.position = SHPhysics.CalculationSpring(vCenterPos, vTargetPos, ref m_vSpringSpeed, 1000.0f, 20.0f);
    //}

    //Vector3 GetConvertPosToTouchPos(Vector3 vOldPos, Vector3 vPos)
    //{
    //    Vector3 vOffset = (vPos - vOldPos);

    //    if (vOffset.x != 0f || vOffset.y != 0f)
    //    {
    //        vOffset = m_pStick.InverseTransformDirection(vOffset);
    //        vOffset.Scale(m_vScale);
    //        vOffset = m_pStick.TransformDirection(vOffset);
    //    }

    //    vOffset.z = 0.0f;
    //    return vOffset;
    //}

    //Vector3 GetTargetNormalize()
    //{
    //    Vector3 vLocalPos = m_pStick.localPosition;

    //    if (false == normalize)
    //        return vLocalPos;
    //    else
    //        return vLocalPos / m_fRadius * Mathf.InverseLerp(m_fRadius, deadZone, 1);
    //}

    //Vector3 GetPosToTouch()
    //{
    //    //set Joystick to finger touchposition
    //    Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastEventPosition);
    //    Vector3 vTouchPos = ray.GetPoint(0.0f);
    //    vTouchPos.z = 0.0f;
    //    return vTouchPos;
    //}

    //void SetPosToWidget(Vector3 vPos)
    //{
    //    foreach (Transform pWidget in widgetsToCenter)
    //    {
    //        pWidget.position = vPos;
    //    }
    //}

    //bool IsZeroToTouchPos()
    //{
    //    return (Vector3.zero == m_vOldPos && Vector3.zero == m_vCurrentPos);
    //}

    //void SendMsgToDoubleTabMethode()
    //{
    //    if (Time.deltaTime >= doubleTapTimeWindow)
    //        return;

    //    if (null == doubleTapMessageTarget)
    //        return;

    //    if ("" == doubleTabMethodeName)
    //        return;

    //    doubleTapMessageTarget.SendMessage(doubleTabMethodeName, SendMessageOptions.DontRequireReceiver);
    //}
}
