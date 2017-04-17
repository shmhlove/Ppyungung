using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SHWeaponStyleSlot
{
    public eCharWeaponType m_eType   = eCharWeaponType.NormalBullet;
    public GameObject      m_pObject = null;
}

public class SHDropItem_WeaponStyle : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private List<SHWeaponStyleSlot> m_pStyle = null;
    #endregion


    #region Members : Info
    public eCharWeaponType m_eActiveType = eCharWeaponType.NormalBullet;
    #endregion

    #region Interface Functions
    public void SetActiveType(eCharWeaponType eType)
    {
        m_eActiveType = eType;

        SHUtils.ForToList(m_pStyle, (pItem) =>
        {
            pItem.m_pObject.SetActive(pItem.m_eType == eType);
        });
    }
    #endregion
}
