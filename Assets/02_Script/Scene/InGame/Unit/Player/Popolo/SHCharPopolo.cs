using UnityEngine;
using System;
using System.Collections;

/* partial Information
 * 
 * SHCharPopolo
 * SHCharPopolo_State
 * SHCharPopolo_UI
 * SHCharPopolo_Utility
 * 
*/

public partial class SHCharPopolo : SHState
{
    #region Members : Inspector
    [Header("Character State")]
    [ReadOnlyField]  private string        m_strState  = string.Empty;
    [Header("Character Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos = null;
    #endregion


    #region Members : User Interaction Data
    private bool    m_bIsDash        = false;
    private bool    m_bIsShoot       = false;
    private Vector3 m_vMoveDirection = Vector3.zero;
    private Vector3 m_vLookDirection = Vector3.zero;
    #endregion


    #region Members : Character Status Data
    public float    m_fDashPoint     = 0.0f;
    public float    m_fHealthPoint   = 0.0f;
    #endregion


    #region Members : ETC
    private SHDamageObject m_pBodyDamage = null;
    #endregion


    #region System Functions
    public override void OnEnable()
    {
        base.OnEnable();
        AddBodyDamage();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        DelBodyDamage();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        StopCharacter();
    }
    public override void FrameMove()
    {
        base.FrameMove();
        m_strState = ((eState)m_iCurrentStateID).ToString();
    }
    public override bool IsPassDMGCollision()
    {
        return ((true == IsState((int)eState.Die)) ||
                (true == IsState((int)eState.Dash)));
    }
    public override void OnCrashDamage(SHMonoWrapper pObject)
    {
        if (null == pObject)
            return;

        var pDamage = pObject as SHDamageObject;
        AddHP(-pDamage.m_pInfo.m_fDamageValue);

        if (false == IsRemainHP())
        {
            ChangeState(eState.Die);
            PlayParticle("Particle_Crash_Dust_Big");
        }
        else
        {
            PlayParticle("Particle_Crash_Dust_Small");
        }
    }
    #endregion


    #region Interface Functions
    public void OnInitialize()
    {
        m_bIsDash        = false;
        m_bIsShoot       = false;
        m_vMoveDirection = Vector3.zero;
        m_vLookDirection = Vector3.zero;

        m_fDashPoint     = 0.0f;
        m_fHealthPoint   = SHHard.m_iCharMaxHealthPoint;
    }
    public void StartCharacter()
    {
        ChangeState(eState.Idle);
        ConnectControllerUI();
    }
    public void StopCharacter()
    {
        DeConnectControllerUI();
    }
    public bool IsDie()
    {
        return IsState((int)eState.Die);
    }
    #endregion
}