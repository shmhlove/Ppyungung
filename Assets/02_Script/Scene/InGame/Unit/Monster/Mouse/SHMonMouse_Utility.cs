using UnityEngine;
using System;
using System.Collections;

public partial class SHMonMouse : SHState
{
    #region Utility : State Common
    SHStateInfo CreateState(eState eType)
    {
        return base.CreateState((int)eType);
    }
    void ChangeState(eState eType)
    {
        base.ChangeState((int)eType);
    }
    #endregion


    #region Utility : State Move
    void SetMove()
    {
        if (Vector3.zero == m_vDirection)
        {
            m_vDirection = (Single.Player.GetLocalPosition() - GetLocalPosition()).normalized;
        }

        var vPos = SHPhysics.GuidedMissile(GetLocalPosition(), ref m_vDirection, Single.Player.GetLocalPosition(),
            m_fHommingAngle, SHHard.m_fMonMoveSpeed);

        SetLocalLookY(m_vDirection);
        SetLocalPositionX(vPos.x);
        SetLocalPositionZ(vPos.z);
    }
    #endregion


    #region Utility : Helper
    void SetAttack(Vector3 vDirection)
    {
        var pDamage = Single.Damage.AddDamage("Dmg_Mon_Bullet", new SHAddDamageParam(m_pShootPos));
        {
            pDamage.SetDMGSpeed(SHHard.m_fMonDamageSpeed);
            pDamage.SetDMGDirection(vDirection);
        }
    }
    #endregion
}