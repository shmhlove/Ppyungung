using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    public SHCharPopolo    m_pUnit   = null;
    public SHCharWeapon    m_pWeapon = new SHCharWeapon();
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        if (null == m_pUnit)
            return;

        m_pUnit.FrameMove();
    }
    public override void SetPause(bool bIsPause)
    {
        base.SetPause(bIsPause);

        if (null == m_pUnit)
            return;

        m_pUnit.SetPauseAnimation(bIsPause);
    }
    #endregion


    #region Interface Player Functions
    public void ClearPlayer()
    {
        if (null == m_pUnit)
            return;

        m_pUnit.OnInitialize();
    }
    public void StartPlayer()
    {
        if (null == m_pUnit)
        {
            m_pUnit = Single.ObjectPool.Get<SHCharPopolo>("CharPopolo", true, ePoolReturnType.ChangeScene, ePoolDestroyType.Return);
            m_pUnit.SetParent(Single.Root3D.GetRootPlayer());
            m_pUnit.SetLocalScale(m_pUnit.m_vStartScale * SHHard.m_fUnitScale);
            m_pUnit.OnInitialize();
        }

        m_pUnit.SetActive(true);
        m_pUnit.StartCharacter();
    }
    public void StopPlayer()
    {
        if (null == m_pUnit)
            return;

        m_pUnit.StopCharacter();
    }
    public Vector3 GetLocalPosition()
    {
        if (null == m_pUnit)
            return Vector3.zero;

        return m_pUnit.GetLocalPosition();
    }
    public float GetHPPercent()
    {
        if (null == m_pUnit)
            return 0.0f;

        return SHMath.Divide(m_pUnit.m_fHealthPoint, (float)SHHard.m_iCharMaxHealthPoint) * 100.0f;
    }
    public float GetDPPercent()
    {
        if (null == m_pUnit)
            return 0.0f;

        return SHMath.Divide(m_pUnit.m_fDashPoint, SHHard.m_fCharMaxDashPoint) * 100.0f;
    }
    public void ResetHP()
    {
        m_pUnit.ResetHP();
    }
    public bool IsDie()
    {
        if (null == m_pUnit)
            return false;

        return m_pUnit.IsDie();
    }
    #endregion


    #region Interface Weapon Functions
    public void ClearWeapon()
    {
        m_pWeapon.m_eType = eCharWeaponType.NormalBullet;
    }
    public void SetChangeWeapon(eCharWeaponType eType)
    {
        m_pWeapon.m_eType = eType;
    }
    public eCharWeaponType GetCurrentWeapon()
    {
        return m_pWeapon.m_eType;
    }
    public string GetDamageName()
    {
        return m_pWeapon.GetDamageName();
    }
    #endregion


    #region Interface Buff Functions
    public SHDamageObject AddShieldDamage()
    {
        if (null == m_pUnit)
            return null;

        return m_pUnit.AddShieldDamage();
    }
    #endregion
}