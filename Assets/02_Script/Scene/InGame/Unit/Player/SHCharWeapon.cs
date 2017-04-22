using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharWeaponType
{
    NormalBullet,
    ThreeBullet,
    GuidedBullet,
    SplashBullet,
    Laser,
}

public class SHCharWeapon
{
    public eCharWeaponType m_eType = eCharWeaponType.NormalBullet;

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
}