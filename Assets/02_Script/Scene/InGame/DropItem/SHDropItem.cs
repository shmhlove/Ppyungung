using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eDropItemType
{
    Coin,
    Weapon,
}

public class SHDropItem : SHInGame_Component
{
    #region Interface Functions
    public void AddDropItem(eDropItemType eType, Vector3 vPosition)
    {
        switch(eType)
        {
            case eDropItemType.Coin:    OnDropToCoin(vPosition);   break;
            case eDropItemType.Weapon:  OnDropToWeapon(vPosition); break;
        }
    }
    #endregion


    #region Utility Functions
    void OnDropToCoin(Vector3 vPosition)
    {
        var pItem = Single.Damage.AddDamage("Dmg_Item_Coin", 
            new SHDamageParam(null, vPosition, pEventCollision: (pDamage, pTarget) => 
            {
                switch (pTarget.transform.tag)
                {
                    case "Character":       break;
                    case "CharacterDamage": break;
                }
            }));
    }
    void OnDropToWeapon(Vector3 vPosition)
    {
        var pItem = Single.Damage.AddDamage("Dmg_Item_Weapon", 
            new SHDamageParam(null, vPosition, pEventCollision: (pDamage, pTarget) =>
            {
                var pStyle = pDamage.GetComponent<SHDropItem_WeaponStyle>();
                
                switch (pTarget.transform.tag)
                {
                    case "Character":
                        Single.Player.SetChangeWeapon(pStyle.m_eActiveType);
                        Single.Damage.DelDamage(pDamage);
                        break;
                    case "CharacterDamage":
                        pStyle.SetActiveType(SHMath.RandomEnum<eCharWeaponType>());
                        break;
                }
            }));

        pItem.SetLocalScale(pItem.m_vStartScale);

        var pWeaponStyle = pItem.GetComponent<SHDropItem_WeaponStyle>();
        pWeaponStyle.SetActiveType(SHMath.RandomEnum<eCharWeaponType>());
    }
    #endregion
}