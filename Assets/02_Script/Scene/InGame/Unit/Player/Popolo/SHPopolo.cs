using UnityEngine;
using System;
using System.Collections;

/* partial Information
 * 
 * SHPopolo
 * SHPopolo_State
 * SHPopolo_UI
 * SHPopolo_Utility
 * 
*/

public partial class SHPopolo : SHState
{
    #region Members : Inspector
    [Header("Player Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos     = null;
    [SerializeField] private float         m_fMoveSpeed    = 50.0f;
    [SerializeField] private float         m_fDashSpeed    = 150.0f;
    [SerializeField] private float         m_fDashTime     = 1.0f;
    [SerializeField] private float         m_fDashCoolTime = 3.0f;
    #endregion


    #region Members : User Interaction Data
    private bool    m_bIsDash        = false;
    private bool    m_bIsDashReady   = true;
    private bool    m_bIsShoot       = false;
    private Vector3 m_vMoveDirection = Vector3.zero;
    private Vector3 m_vLookDirection = Vector3.zero;
    private Vector3 m_vDashDirection = Vector3.zero;
    #endregion


    #region System Functions
    public override void Start()
    {
#if UNITY_EDITOR
        SHHard.m_fPlayerMoveSpeed = m_fMoveSpeed;
#else
        SHHard.m_fPlayerMoveSpeed = SHPlayerPrefs.GetFloat("Player_MoveSpeed", m_fMoveSpeed);
#endif
        base.Start();
        ConnectControllerUI();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        DeConnectControllerUI();
    }
    public override void FixedUpdate()
    {
        base.FrameMove();
    }
    #endregion


    #region Interface Functions
    #endregion
}