using UnityEngine;
using System;
using System.Collections;

/* partial Information
 * 
 * SHMonPopolo
 * SHMonPopolo_State
 * SHMonPopolo_Utility
 * 
*/

public partial class SHMonMouse : SHState
{
    #region Members : Inspector
    [Header("Monster State")]
    [ReadOnlyField]  public int            m_iMonsterID    = 0;
    [ReadOnlyField]  public string         m_strState      = string.Empty;
    [Header("Monster Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos     = null;
    [SerializeField] private float         m_fMoveSpeed    = 30.0f;
    #endregion


    #region Members : AI Data
    private int      m_iAttackCount = 0;
    #endregion


    #region Members : Constants
    private int      MAX_ATTACK_COUNT  = 5;
    private float    DELAY_TIME_ATTACK = 3.0f;
    #endregion


    #region System Functions
    public override void Start()
    {
        base.Start();
        SHHard.m_fMonMoveSpeed = m_fMoveSpeed;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void FixedUpdate()
    {
        base.FrameMove();
        m_strState = ((eState)m_iCurrentStateID).ToString();
    }
    #endregion


    #region Interface Functions
    public void Initialize(int iMonID)
    {
        m_iMonsterID = iMonID;
    }
    #endregion


    #region Dev Functions
    [FuncButton] void PlayIdleState()
    {
        ChangeState(eState.Idle);
    }
    [FuncButton] void PlayMoveState()
    {
        ChangeState(eState.Move);
    }
    [FuncButton] void PlayAttackState()
    {
        ChangeState(eState.Attack);
    }
    [FuncButton] void PlayDieState()
    {
        ChangeState(eState.Die);
    }
    #endregion
}