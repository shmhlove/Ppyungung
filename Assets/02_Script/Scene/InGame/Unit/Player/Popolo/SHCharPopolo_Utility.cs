using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
{
    #region Utility : Action
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
        
        // 데미지 생성
        var pAddDamage = Single.Damage.AddDamage("Dmg_Char_Bullet",
                        new SHDamageParam(m_pShootPos, null, null, (pDamage, pTarget) => 
                        {
                            if (0.0f == pTarget.m_fHealthPoint)
                            {
                                Single.GameState.AddScore(1);
                                AddDashGauge();
                            }
                        }));
        pAddDamage.SetDMGSpeed(SHHard.m_fCharDamageSpeed);

        // 카메라 흔들기
        SH3DRoot.PlayCameraShake();

        m_bIsShoot = false;
    }
    void SetMove(float fSpeed)
    {
        if (false == IsPossibleMove())
            return;
        
        var vSpeed = (m_vMoveDirection * fSpeed);
        var vPos   = SHPhysics.CalculationEuler(Vector3.zero, GetLocalPosition(), ref vSpeed);
        
        SetLocalPosition(GetLimitInGround(vPos));
    }
    #endregion


    #region Utility : Dash
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
    #endregion


    #region Utility : Body Damage
    void AddBodyDamage()
    {
        DelBodyDamage();
        m_pBodyDamage = Single.Damage.AddDamage("Dmg_Char_Body", 
            new SHDamageParam(this, null, null, (pDamage, pTarget) => { OnCrashDamage(pDamage); }));
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


    #region Utility : Check Functions
    bool IsRemainDashGauge()
    {
        return ((0.0f < m_fDashPoint) && (SHHard.m_fCharDecDashPoint < m_fDashPoint));
    }
    bool IsPossibleDash()
    {
        return (true == m_bIsDash) && (true == IsRemainDashGauge());
    }
    bool IsPossibleAttack()
    {
        return m_bIsShoot;
    }
    bool IsPossibleMove()
    {
        return (Vector3.zero != m_vMoveDirection);
    }
    #endregion


    #region Utility : Helper
    bool SetLookNearMonster()
    {
        var pNearMon = Single.Monster.GetNearMonster(GetLocalPosition());
        if (null == pNearMon)
            return false;
        
        SetLocalLookZ((pNearMon.GetLocalPosition() - GetLocalPosition()).normalized);
        return true;
    }
    void SetLookRotation()
    {
        if (Vector3.zero == m_vLookDirection)
            return;
        
        SetLocalLookZ(m_vLookDirection);
    }
    Vector3 GetLimitInGround(Vector3 vPosition)
    {
        return SHPhysics.IncludePointInRect(new Vector4(
            -SHHard.m_fMoveLimitX, -SHHard.m_fMoveLimitY,
             SHHard.m_fMoveLimitX, SHHard.m_fMoveLimitY), vPosition);
    }
    #endregion
}