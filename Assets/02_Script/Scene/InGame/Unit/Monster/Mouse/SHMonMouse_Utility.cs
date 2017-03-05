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
        SetLocalLookY(m_vDirection);
    }
    void SetMove()
    {
        AddLocalPositionX((SHHard.m_fMonMoveSpeed + SHHard.m_fBasicMoveSpeed) * m_vDirection.x);

        if (0.0f < m_vDirection.z)
            AddLocalPositionZ((SHHard.m_fMonMoveSpeed + SHHard.m_fBasicMoveSpeed) * m_vDirection.z);
        else
            AddLocalPositionZ(SHHard.m_fMonMoveSpeed * m_vDirection.z);

        // 캐릭터와 너무 멀리 떨어지면 죽이자
        if (20000.0f < Vector3.Distance(Single.Player.GetLocalPosition(), GetLocalPosition()))
            ChangeState(eState.Die);
    }
    #endregion


    #region Utility : AttackState
    void SetAttack(Vector3 vDirection)
    {
        var pDamage = Single.Damage.AddDamage("Dmg_Mon_Bullet",
                        new SHAddDamageParam(m_pShootPos, null, null, null));
        
        pDamage.SetDMGSpeed(SHHard.m_fMonDamageSpeed);
        
        if (Vector3.zero != vDirection)
            pDamage.m_vDMGDirection = vDirection;
    }
    bool IsAttackDelay()
    {
        return SHHard.m_fMonAttackDelay >
            Single.Timer.GetDeltaTimeToSecond(GetAttackKey());
    }
    string GetAttackKey()
    {
        return string.Format("MonPopolo_{0}", m_iMonsterID);
    }
    #endregion
}