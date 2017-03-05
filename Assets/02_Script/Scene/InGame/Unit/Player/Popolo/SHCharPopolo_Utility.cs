﻿using UnityEngine;
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
    void SetLookRotation()
    {
        if (true == Single.Player.m_bIsAttacking)
        {
            if (true == SetLookNearMonster())
                return;
        }

        if (false == Single.Player.m_bIsMoving)
            m_vLookDirection.y = 1.0f;

        if (Vector3.zero == m_vLookDirection)
            return;

        m_vLookDirection.z = m_vLookDirection.y;
        m_vLookDirection.y = 0.0f;
        m_vDashDirection   = m_vLookDirection;

        SetLocalLookY(m_vLookDirection);
        m_vLookDirection = Vector3.zero;
    }
    bool SetLookNearMonster()
    {
        var pNearMon = Single.Monster.GetNearMonster(GetLocalPosition());
        if (null == pNearMon)
            return false;

        var vDirection   = (pNearMon.GetLocalPosition() - GetLocalPosition()).normalized;
        m_vDashDirection = vDirection;

        SetLocalLookY(vDirection);
        return true;
    }
    bool SetMove()
    {
        AddLocalPositionX((SHHard.m_fBasicMoveSpeed + SHHard.m_fCharMoveSpeed) * m_vMoveDirection.x);

        if (0.0f == m_vMoveDirection.y)
            AddLocalPositionZ(SHHard.m_fBasicMoveSpeed);
        else
            AddLocalPositionZ((SHHard.m_fBasicMoveSpeed + SHHard.m_fCharMoveSpeed) * m_vMoveDirection.y);

        LimitInCamera();
        
        m_vMoveDirection = Vector3.zero;
        return true;
    }
    void SetDash()
    {
        SetLocalLookY(m_vDashDirection);
        AddLocalPositionX(SHHard.m_fCharDashSpeed * m_vDashDirection.x);
        AddLocalPositionZ(SHHard.m_fCharDashSpeed * m_vDashDirection.z);
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

        pDamage.SetDMGSpeed(SHHard.m_fCharDamageSpeed);
        m_bIsShoot = false;

        return true;
    }
    #endregion
}