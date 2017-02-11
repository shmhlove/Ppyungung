using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicMonsters = System.Collections.Generic.Dictionary<eMonsterType, eGoodsState>;

public enum eMonsterType
{
    None,
    Monster_1,
    Monster_2,
    Monster_3,
    Monster_4,
    Monster_5,
    Monster_6,
    Monster_7,
    Monster_8,
    Monster_9,
    Monster_10,
    Monster_11,
    Monster_12,
    Max_NormalMonster,
    Monster_Bonus,
    Max
}

public partial class SHInventory : SHBaseEngine
{
    #region Members : Info
    private DicMonsters m_dicMonsterInfo = new DicMonsters();
    #endregion


    #region Members : Constants
    private int MIN_ENABLE_COUNT = 4;
    #endregion


    #region Interface : System
    public void InitMonster()
    {
        RegisterBasicMonster();
        ResetMonsterInfo();
    }
    public void OnUpdateMonster()
    {

    }
    #endregion


    #region Interface : Dic Helpper
    public List<eMonsterType> GetEnableMonstersForDic()
    {
        var pResult = new List<eMonsterType>();
        SHUtils.ForToDic(m_dicMonsterInfo, (pKey, pValue) =>
        {
            if (eGoodsState.Enable != pValue)
                return;

            pResult.Add(pKey);
        });

        return pResult;
    }
    #endregion


    #region Interface : PlayerPrefs Helpper
    public void SetMonsterTypeToPlayerPrefs(eMonsterType eMonType, eGoodsState eGoods)
    {
        if (eGoodsState.Disable == eGoods)
        {
            if (MIN_ENABLE_COUNT >= GetEnableMonstersToPlayerPrefs().Count)
            {
                Single.UI.ShowNotice("알림", "몬스터는 최소 5마리 이상입니다.");
                return;
            }
        }

        SHPlayerPrefs.SetInt(string.Format("Inventory_Monste_{0}", (int)eMonType), (int)eGoods);
    }
    public eGoodsState GetMonsterGoodsStateToPlayerPrefs(eMonsterType eType)
    {
        return (eGoodsState)SHPlayerPrefs.GetInt(
            string.Format("Inventory_Monste_{0}", (int)eType), (int)eGoodsState.NotHas);
    }
    public List<eMonsterType> GetEnableMonstersToPlayerPrefs()
    {
        var pResult = new List<eMonsterType>();
        SHUtils.ForToEnum<eMonsterType>((eType) =>
        {
            if (false == IsEnableMonsterToPlayerPrefs(eType))
                return;

            pResult.Add(eType);
        });

        return pResult;
    }
    public bool IsHasMonsterToPlayerPrefs(eMonsterType eType)
    {
        return (eGoodsState.NotHas != GetMonsterGoodsStateToPlayerPrefs(eType));
    }
    public bool IsEnableMonsterToPlayerPrefs(eMonsterType eType)
    {
        return (eGoodsState.Enable == GetMonsterGoodsStateToPlayerPrefs(eType));
    }
    #endregion


    #region Utility Functions
    private void RegisterBasicMonster()
    {
        RegisterMonster(eMonsterType.Monster_1);
        RegisterMonster(eMonsterType.Monster_2);
        RegisterMonster(eMonsterType.Monster_3);
        RegisterMonster(eMonsterType.Monster_4);
    }
    private void RegisterMonster(eMonsterType eType)
    {
        if (true == IsHasMonsterToPlayerPrefs(eType))
            return;

        SetMonsterTypeToPlayerPrefs(eType, eGoodsState.Enable);
    }
    void ResetMonsterInfo()
    {
        m_dicMonsterInfo.Clear();
        SHUtils.ForToEnum<eMonsterType>((eType) =>
        {
            if ((eMonsterType.None == eType)              ||
                (eMonsterType.Max_NormalMonster == eType) ||
                (eMonsterType.Max == eType))
                return;

            m_dicMonsterInfo.Add(eType, GetMonsterGoodsStateToPlayerPrefs(eType));
        });
    }
    #endregion
}
