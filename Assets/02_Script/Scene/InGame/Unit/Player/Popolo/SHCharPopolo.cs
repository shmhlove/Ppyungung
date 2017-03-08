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
    private bool    m_bIsDashReady   = true;
    private bool    m_bIsShoot       = false;
    private Vector3 m_vMoveDirection = Vector3.zero;
    private Vector3 m_vLookDirection = Vector3.zero;
    private Vector3 m_vDashDirection = Vector3.zero;
    #endregion


    #region Members : ETC
    private SHDamageObject m_pCharDamage = null;
    #endregion


    #region System Functions
    public override void OnEnable()
    {
        base.OnEnable();
        SetBeginDamage();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SetEndDamage();
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
    public override void OnCrashDamage(SHMonoWrapper pDamage)
    {
        if (null == pDamage)
            return;

        PlayParticle("Particle_Crash_Dust_Big");
        // ChangeState(eState.Die);
    }
    #endregion


    #region Interface Functions
    public void StartCharacter()
    {
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