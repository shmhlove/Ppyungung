using UnityEngine;
using System;
using System.Collections;

public partial class SHPopolo : SHState
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


    #region Utility : Behaviour
    bool SetLookDirection()
    {
        if (Vector3.zero == m_vLookDirection)
            return false;

        m_vDirection = m_vLookDirection;
        SetLocalRotateY(SHMath.GetAngleToPosition(Vector3.forward, -1.0f, Vector3.up, m_vLookDirection));
        m_vLookDirection = Vector3.zero;
        return true;
    }
    bool SetMove()
    {
        if (Vector3.zero == m_vMoveDirection)
            return false;

        AddLocalPositionX(SHHard.m_fPlayerMoveSpeed * m_vMoveDirection.x);
        AddLocalPositionZ(SHHard.m_fPlayerMoveSpeed * m_vMoveDirection.y);
        m_vMoveDirection = Vector3.zero;
        return true;
    }
    void SetDash()
    {
        AddLocalPositionX(m_fDashSpeed * m_vDashDirection.x);
        AddLocalPositionZ(m_fDashSpeed * m_vDashDirection.y);
    }
    bool SetAttack()
    {
        if (false == m_bIsShoot)
            return false;

        SH3DRoot.PlayCameraShake();
        Single.Damage.AddDamage("Dmg_Bullet",
                        new SHAddDamageParam(m_pShootPos, null, null, null));

        m_bIsShoot = false;
        return true;
    }
    #endregion
}