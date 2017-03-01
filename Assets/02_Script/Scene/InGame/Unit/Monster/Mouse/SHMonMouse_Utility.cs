using UnityEngine;
using System;
using System.Collections;

public partial class SHMonMouse : SHState
{
    #region Utility : State
    SHStateInfo CreateState(eState eType)
    {
        return base.CreateState((int)eType);
    }
    void ChangeState(eState eType)
    {
        base.ChangeState((int)eType);
    }
    #endregion


    #region Utility : State
    void SetLookPC()
    {
        var vPCPos   = Single.Player.GetLocalPosition();
        m_vDirection = (vPCPos - GetLocalPosition()).normalized;
        SetLocalRotateY(SHMath.GetAngleToPosition(Vector3.up, 1.0f, Vector3.forward, m_vDirection));
    }
    void SetMove()
    {
        AddLocalPositionX(SHHard.m_fMonMoveSpeed * m_vDirection.x);
        AddLocalPositionZ(SHHard.m_fMonMoveSpeed * m_vDirection.z);
    }
    #endregion


    #region Utility : AttackState
    void SetAttack(Vector3 vDirection)
    {
        var pDamage = Single.Damage.AddDamage("Dmg_Mon_Bullet",
                        new SHAddDamageParam(m_pShootPos, null, null, null));

        if (Vector3.zero != vDirection)
            pDamage.m_vDirection = vDirection;
    }
    bool IsAttackDelay()
    {
        return DELAY_TIME_ATTACK >
            Single.Timer.GetDeltaTimeToSecond(GetAttackKey());
    }
    string GetAttackKey()
    {
        return string.Format("MonPopolo_{0}", m_iMonsterID);
    }
    #endregion
}