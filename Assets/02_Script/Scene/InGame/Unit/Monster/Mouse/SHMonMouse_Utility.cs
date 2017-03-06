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
        // var vPCPos   = Single.Player.GetLocalPosition();
        // m_vDirection = (vPCPos - GetLocalPosition()).normalized;
        // SetLocalLookY(m_vDirection);
    }
    void SetMove()
    {
        if (Vector3.zero == m_vDirection)
        {
            var vPCPos   = Single.Player.GetLocalPosition();
            m_vDirection = (vPCPos - GetLocalPosition()).normalized;
            SetLocalLookY(m_vDirection);
        }

        var vPos = SHPhysics.GuidedMissile(GetLocalPosition(), ref m_vDirection, Single.Player.GetLocalPosition(),
            m_fHommingAngle, SHHard.m_fMonMoveSpeed);

        SetLocalLookY(m_vDirection);
        SetLocalPositionX(vPos.x);
        SetLocalPositionZ(vPos.z);
        //AddLocalPositionX(SHHard.m_fMonMoveSpeed * m_vDirection.x);
        //AddLocalPositionZ(SHHard.m_fMonMoveSpeed * m_vDirection.z);

        // 개체 충돌체크
        // var iLayerMask      = (1 << LayerMask.NameToLayer("Monster"));
        // var vPosition       = GetLocalPosition();
        // var vBeforePosition = m_vBeforePosition;
        // var vExtents        = GetDMGCollider().bounds.extents;
        // var vDist           = Vector3.Distance(vPosition, vBeforePosition);
        // var vDirection      = (vBeforePosition - vPosition).normalized;
        // vDirection          = (Vector3.zero == vDirection) ? Vector3.forward : vDirection;
        // var pHits           = Physics.BoxCastAll(vPosition, vExtents, vDirection, Quaternion.identity, vDist, iLayerMask);

        // AddLocalPositionX((SHHard.m_fMonMoveSpeed + SHHard.m_fBasicMoveSpeed) * m_vDirection.x);
        // 
        // if (0.0f < m_vDirection.z)
        //     AddLocalPositionZ((SHHard.m_fMonMoveSpeed + SHHard.m_fBasicMoveSpeed) * m_vDirection.z);
        // else
        //     AddLocalPositionZ(SHHard.m_fMonMoveSpeed * m_vDirection.z);

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