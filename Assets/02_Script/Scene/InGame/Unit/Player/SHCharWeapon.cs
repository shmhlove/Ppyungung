using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCharWeaponType
{
    NormalBullet,
    ThreeBullet,
    GuidedBullet,
    SplashBullet,
    Radiate,
}

public class SHCharWeapon
{
    public eCharWeaponType m_eType = eCharWeaponType.NormalBullet;

    public string GetDamageName()
    {
        switch(m_eType)
        {
            case eCharWeaponType.NormalBullet:  return "Dmg_Char_Bullet";
            case eCharWeaponType.ThreeBullet:   return "Dmg_Char_Three_Bullet";
            case eCharWeaponType.GuidedBullet:  return "Dmg_Char_Guided_Bullet";
            case eCharWeaponType.SplashBullet:  return "Dmg_Char_Splash_Bullet";
            case eCharWeaponType.Radiate:       return "Dmg_Char_Bullet";
        }

        return "Dmg_Char_Bullet";
    }
}
