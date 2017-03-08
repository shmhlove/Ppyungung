using UnityEngine;
using System;
using System.Collections;

/* partial Information
 * 
 * SHMonMouse
 * SHMonMouse_State
 * SHMonMouse_Utility
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
    #endregion


    #region Members : AI Data
    private Vector3        m_vDirection     = Vector3.zero;
    private SHDamageObject m_pMonDamage     = null;
    private float          m_fHommingAngle  = 1.0f;
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
    }
    public override void FrameMove()
    {
        base.FrameMove();
        m_strState = ((eState)m_iCurrentStateID).ToString();
    }
    public override bool IsPassDMGCollision()
    {
        return IsState((int)eState.Die);
    }
    public override void OnCrashDamage(SHMonoWrapper pDamage)
    {
        if (null == pDamage)
            return;

        ChangeState(eState.Die);
    }
    #endregion


    #region Interface Functions
    public void InitMonster(int iMonID)
    {
        m_iMonsterID    = iMonID;
        m_fHommingAngle = SHMath.Random(1.0f, 2.0f);
    }
    #endregion
}