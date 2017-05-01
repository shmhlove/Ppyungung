using UnityEngine;
using System;
using System.Collections;

public enum eControlType
{
    Type_0,
    Type_1,
    Type_2,
    Type_3,
    Type_4,
    Type_5,
    Type_6,
}

public class SHUIPanel_CtrlPad : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Controller")]
    [SerializeField] public eControlType         m_eCtrlType  = eControlType.Type_1;
    [SerializeField] public SHUIWidget_CtrlType0 m_pCtrlType0 = null;
    [SerializeField] public SHUIWidget_CtrlType1 m_pCtrlType1 = null;
    [SerializeField] public SHUIWidget_CtrlType2 m_pCtrlType2 = null;
    [SerializeField] public SHUIWidget_CtrlType3 m_pCtrlType3 = null;
    [SerializeField] public SHUIWidget_CtrlType4 m_pCtrlType4 = null;
    [SerializeField] public SHUIWidget_CtrlType5 m_pCtrlType5 = null;
    [SerializeField] public SHUIWidget_CtrlType6 m_pCtrlType6 = null;
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
#if UNITY_EDITOR
#else
        m_eCtrlType = (eControlType)SHPlayerPrefs.GetInt("Player_CtrlType", (int)m_eCtrlType);
#endif
    }
    public override void OnEnable()
    {
        SetCtrlType(m_eCtrlType);
    }
    #endregion


    #region Interface Functions
    [FuncButton] public void SetCtrlType(eControlType eType)
    {
        SettingController(m_eCtrlType = eType);
    }
    public bool IsCtrlType(eControlType eType)
    {
        return (eType == m_eCtrlType);
    }
    public void AddEventToMove(Action<Vector3> pEvent)
    {
        m_pEventMove = pEvent;
    }
    public void AddEventToDirection(Action<Vector3> pEvent)
    {
        m_pEventDirection = pEvent;
    }
    public void AddEventToShoot(Action<bool> pEvent)
    {
        m_pEventShoot = pEvent;
    }
    public void AddEventToDash(Action<bool> pEvent)
    {
        m_pEventDash = pEvent;
    }
    public void Clear()
    {
        m_pEventMove      = null;
        m_pEventDirection = null;
        m_pEventShoot     = null;
        m_pEventDash      = null;

        m_pCtrlType0.Clear();
        m_pCtrlType1.Clear();
        m_pCtrlType2.Clear();
        m_pCtrlType3.Clear();
        m_pCtrlType4.Clear();
        m_pCtrlType5.Clear();
        m_pCtrlType6.Clear();
    }
    #endregion


    #region Utility Functions
    void SettingController(eControlType eType)
    {
        Activer(m_pCtrlType0, false);
        Activer(m_pCtrlType1, false);
        Activer(m_pCtrlType2, false);
        Activer(m_pCtrlType3, false);
        Activer(m_pCtrlType4, false);
        Activer(m_pCtrlType5, false);
        Activer(m_pCtrlType6, false);
        
        switch (eType)
        {
            case eControlType.Type_0:
                m_pCtrlType0.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType0, true);
                break;
            case eControlType.Type_1:
                m_pCtrlType1.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType1, true);
                break;
            case eControlType.Type_2:
                m_pCtrlType2.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType2, true);
                break;
            case eControlType.Type_3:
                m_pCtrlType3.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType3, true);
                break;
            case eControlType.Type_4:
                m_pCtrlType4.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType4, true);
                break;
            case eControlType.Type_5:
                m_pCtrlType5.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType5, true);
                break;
            case eControlType.Type_6:
                m_pCtrlType6.Initialize(OnEventToMove, OnEventToDirection, OnEventToShoot, OnEventToDash);
                Activer(m_pCtrlType6, true);
                break;
        }
    }
    void Activer(SHMonoWrapper pObject, bool bIsActive)
    {
        if (null == pObject)
            return;

        pObject.SetActive(bIsActive);
    }
    #endregion


    #region Event Handler
    public void OnEventToMove(Vector3 vDirection)
    {
        if (null == m_pEventMove)
            return;

        m_pEventMove(vDirection);
    }
    public void OnEventToDirection(Vector3 vDirection)
    {
        if (null == m_pEventDirection)
            return;

        m_pEventDirection(vDirection);
    }
    public void OnEventToShoot(bool bIsOn)
    {
        if (null == m_pEventShoot)
            return;

        m_pEventShoot(bIsOn);
    }
    public void OnEventToDash(bool bIsOn)
    {
        if (null == m_pEventDash)
            return;

        m_pEventDash(bIsOn);
    }
    #endregion
}
