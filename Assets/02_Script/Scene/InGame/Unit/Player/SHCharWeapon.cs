using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharWeaponType
{
    FlameThrower,       // 화염 방사기
    Laser,              // 레이져건
    Commando,           // 코만도
    RPG,                // RPG
    Shotgun,            // 샷건
    Rifle,              // 돌격소총
    Snipe,              // 저격총    
    Stingermissile,     // 스팅어미사일
    PhlegmaticSplash,   // 점액질 분열탄
    FireBomb,           // 화염병 발사기
}

public class SHCharWeapon
{
    public eCharWeaponType m_eType = eCharWeaponType.FlameThrower;

    #region Insterface : WeaponType
    public void ClearWeapon()
    {
        m_eType = eCharWeaponType.FlameThrower;
    }

    public void SetWeapon(eCharWeaponType eType)
    {
        m_eType = eType;
    }

    public eCharWeaponType GetWeaponType()
    {
        return m_eType;
    }
    #endregion


    #region Insterface : Damage
    public SHDamageObject AddDamage(SHDamageParam pParam)
    {
        var pDamage = Single.Damage.AddDamage(GetDamageName(), pParam);
        pDamage.m_pInfo.m_fDamageValue = GetWeaponData().m_fDamageValue;
        return pDamage;
    }
    public void DelDamage(SHDamageObject pDamage)
    {
        Single.Damage.DelDamage(pDamage);
    }
    #endregion


    #region Insterface : Json Data
    public string GetDamageName()
    {
        return GetWeaponData().m_strDamageName;
    }

    public float GetShootDelay()
    {
        return GetWeaponData().m_fShootDelay;
    }

    public JsonWeaponData GetWeaponData()
    {
        var pTable = Single.Table.GetTable<JsonWeaponInfo>();
        return pTable.GetWeaponData(m_eType);
    }
    #endregion
}