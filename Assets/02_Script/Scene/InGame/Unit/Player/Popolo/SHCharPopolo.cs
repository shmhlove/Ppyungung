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
    [Header("[Character State]")]
    [ReadOnlyField]  private string        m_strState  = string.Empty;
    [Header("[Character Info]")]
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
    #endregion


    #region Members : ETC
    private SHDamageObject m_pBodyDamage = null;
    #endregion


    #region System Functions
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
        {
            AddHP(-pDamage.m_pInfo.m_fDamageValue);
            PlayParticle("Particle_Crash_Dust_Big");
        }

        if (false == IsRemainHP())
        {
            ChangeState(eState.Die);
        }
    }
    #endregion


    #region Interface Functions
    public void OnInitialize()
    {
        InitControlValue();
        InitPointValue();
    }
    public void InitControlValue()
    {
        m_bIsDash        = false;
        m_bIsShoot       = false;
        m_vMoveDirection = Vector3.zero;
        m_vLookDirection = Vector3.zero;
    }
    public void InitPointValue()
    {
        m_fDashPoint   = 0.0f;
        ResetHP();
    }
    public void ResetHP()
    {
        m_fHealthPoint = SHHard.m_iCharMaxHealthPoint;
    }
    public void StartCharacter()
    {
        ChangeState(eState.Idle);

        InitControlValue();
        ConnectControllerUI();

        AddBodyDamage();
    }
    public void StopCharacter()
    {
        InitControlValue();
        DeConnectControllerUI();
        DelBodyDamage();
    }
    public bool IsDie()
    {
        return IsState((int)eState.Die);
    }
    #endregion
}