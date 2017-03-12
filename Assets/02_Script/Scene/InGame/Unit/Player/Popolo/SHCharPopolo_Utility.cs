using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
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

    
    #region Utility : State Attack
    void SetAttack()
    {
        if (false == m_bIsShoot)
            return;

        // 컨트롤러 5번과 6번은 자동 타켓팅이다.
        var pCtrlPad = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if ((true == pCtrlPad.IsCtrlType(eControlType.Type_5)) ||
            (true == pCtrlPad.IsCtrlType(eControlType.Type_6)))
        {
            SetLookNearMonster();
        }

        SH3DRoot.PlayCameraShake();
        var pDamage = Single.Damage.AddDamage("Dmg_Char_Bullet",
                        new SHAddDamageParam(m_pShootPos, null, null, (pTarget) => 
                        {
                            Single.GameState.AddScore(1);
                            AddDashGauge();
                        }));

        pDamage.SetDMGSpeed(SHHard.m_fCharDamageSpeed);
        m_bIsShoot = false;
    }
    #endregion


    #region Utility : State Move
    bool SetMove()
    {
        if (Vector3.zero == m_vMoveDirection)
            return false;

        AddLocalPositionX(SHHard.m_fCharMoveSpeed * m_vMoveDirection.x);
        AddLocalPositionZ(SHHard.m_fCharMoveSpeed * m_vMoveDirection.y);

        LimitInGround();

        m_vMoveDirection = Vector3.zero;
        return true;
    }
    #endregion


    #region Utility : State Dash
    void SetDashMove()
    {
        if (Vector3.zero == m_vMoveDirection)
            return;

        AddLocalPositionX(SHHard.m_fCharDashSpeed * m_vMoveDirection.x);
        AddLocalPositionZ(SHHard.m_fCharDashSpeed * m_vMoveDirection.y);

        LimitInGround();

        m_vMoveDirection = Vector3.zero;
    }
    void AddDashGauge()
    {
        m_fDashPoint += SHHard.m_fCharAddDashPoint;
        m_fDashPoint = Mathf.Clamp(m_fDashPoint, 0.0f, SHHard.m_fCharMaxDashPoint);
    }
    void DecreaseDashGauge()
    {
        m_fDashPoint -= SHHard.m_fCharDecDashPoint;
        m_fDashPoint = Mathf.Clamp(m_fDashPoint, 0.0f, m_fDashPoint);
    }
    bool IsRemainDashGauge()
    {
        return ((0.0f < m_fDashPoint) && (SHHard.m_fCharDecDashPoint < m_fDashPoint));
    }
    bool IsPossibleDash()
    {
        return (true == m_bIsDash) && (true == IsRemainDashGauge());
    }
    #endregion


    #region Utility : Body Damage
    void AddBodyDamage()
    {
        DelBodyDamage();
        m_pBodyDamage = Single.Damage.AddDamage("Dmg_Char_Body", 
            new SHAddDamageParam(this, null, null, (pObject) => { OnCrashDamage(pObject); }));
    }
    void DelBodyDamage()
    {
        if (true == SHApplicationInfo.m_bIsAppQuit)
            return;

        Single.Damage.DelDamage(m_pBodyDamage);
        m_pBodyDamage = null;
    }
    void SetBodyDamageLock(bool bIsLock)
    {
        if (null == m_pBodyDamage)
            return;

        m_pBodyDamage.m_bIsCrashLock = bIsLock;
    }
    #endregion


    #region Utility : HealthPoint
    void AddHP(float fAddValue)
    {
        m_fHealthPoint += fAddValue;
        m_fHealthPoint = Mathf.Clamp(m_fHealthPoint, 0.0f, SHHard.m_iCharMaxHealthPoint);
    }
    bool IsRemainHP()
    {
        return (0.0f < m_fHealthPoint);
    }
    #endregion


    #region Utility : Helper
    bool SetLookNearMonster()
    {
        var pNearMon = Single.Monster.GetNearMonster(GetLocalPosition());
        if (null == pNearMon)
            return false;
        
        SetLocalLookY((pNearMon.GetLocalPosition() - GetLocalPosition()).normalized);
        return true;
    }
    void SetLookRotation()
    {
        if (Vector3.zero == m_vLookDirection)
            return;

        m_vLookDirection.z = m_vLookDirection.y;
        m_vLookDirection.y = 0.0f;

        SetLocalLookY(m_vLookDirection);
        m_vLookDirection = Vector3.zero;
    }
    public void LimitInGround()
    {
        var vRect = new Vector4(
            -12000.0f, -7200.0f, 12000.0f, 7200.0f);

        SetLocalPosition(SHPhysics.IncludePointInRect(vRect, GetLocalPosition()));
    }
    #endregion
}