using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
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
        
        AddLocalPositionX(SHHard.m_fCharMoveSpeed * m_vMoveDirection.x);
        AddLocalPositionZ(SHHard.m_fCharMoveSpeed * m_vMoveDirection.y);
        m_vMoveDirection = Vector3.zero;
        return true;
    }
    void SetDash()
    {
        AddLocalPositionX(SHHard.m_fCharDashSpeed * m_vDashDirection.x);
        AddLocalPositionZ(SHHard.m_fCharDashSpeed * m_vDashDirection.y);
    }
    bool SetAttack()
    {
        if (false == m_bIsShoot)
            return false;

        SH3DRoot.PlayCameraShake();
        var pDamage = Single.Damage.AddDamage("Dmg_Char_Bullet",
                        new SHAddDamageParam(m_pShootPos, null, null, (pTarget) => 
                        {
                            Single.ScoreBoard.AddScore(1);
                        }));

        pDamage.SetSpeed(SHHard.m_fCharDamageSpeed);
        m_bIsShoot = false;

        return true;
    }
    #endregion
}