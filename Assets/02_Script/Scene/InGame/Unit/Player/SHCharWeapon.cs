using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharWeaponType
{
    NormalBullet,
    ThreeBullet,
    GuidedMissile,
    SplashDamage,
    Radiate,
}

public class SHCharWeapon
{
    public eCharWeaponType m_eType = eCharWeaponType.NormalBullet;

    public string GetDamageName()
    {
        switch(m_eType)
        {
            case eCharWeaponType.NormalBullet:  return "";
            case eCharWeaponType.ThreeBullet:   return "";
            case eCharWeaponType.GuidedMissile: return "";
            case eCharWeaponType.SplashDamage:  return "";
            case eCharWeaponType.Radiate:       return "";
        }

        return "";
    }
}
