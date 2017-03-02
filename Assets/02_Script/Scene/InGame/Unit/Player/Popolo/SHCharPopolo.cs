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
    [ReadOnlyField]  public  string        m_strState      = string.Empty;
    [Header("Character Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos     = null;
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
    public override void Start()
    {
        base.Start();
    }
    public override void OnEnable()
    {
        Single.Damage.AddUnit(this);

        Single.Damage.DelDamage(m_pCharDamage);
        m_pCharDamage = Single.Damage.AddDamage("Dmg_Char",new SHAddDamageParam(this, null, null, null));
        
        base.OnEnable();
    }
    public override void OnDisable()
    {
        if (true == SHInGame.IsExists)
            Single.Damage.DelUnit(this);

        base.OnDisable();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        StopCharacter();
    }
    public override void FixedUpdate()
    {
        base.FrameMove();
        m_strState = ((eState)m_iCurrentStateID).ToString();
    }
    public override bool IsOffDamage()
    {
        return IsState((int)eState.Die);
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
    #endregion
}