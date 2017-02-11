using UnityEngine;
using System;
using System.Collections;

public class SHUIScrollSlot_Monster : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField]  private SHUIWidget_ItemSlot m_pLeftSlot     = null;
    [SerializeField]  private SHUIWidget_ItemSlot m_pRightSlot    = null;
    #endregion


    #region Members : Info
    [SerializeField]  public eMonsterType       m_eLeftType     = eMonsterType.None;
    [SerializeField]  public eMonsterType       m_eRightType    = eMonsterType.None;
    [HideInInspector] public SHUIWidget_Monster m_pLeftMonster  = null;
    [HideInInspector] public SHUIWidget_Monster m_pRightMonster = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    public void Initialize(eMonsterType eType1, eMonsterType eType2)
    {
        ReturnStickObject(m_pLeftMonster);
        ReturnStickObject(m_pRightMonster);
        SetMonsterSlot(
            (m_eLeftType    = eType1), 
            (m_pLeftMonster = CreateMonsterSlot(eType1)), 
            m_pLeftSlot);
        SetMonsterSlot(
            (m_eRightType    = eType2), 
            (m_pRightMonster = CreateMonsterSlot(eType2)), 
            m_pRightSlot);
        
        SetGoodsState();
    }
    public void SetGoodsState()
    {
        if (null != m_pLeftSlot)
        {
            if (null == m_pLeftMonster)
            {
                m_pLeftSlot.Initialize();
            }
            else
            {
                var eGoodsType = Single.Inventory.GetMonsterGoodsStateToPlayerPrefs(m_eLeftType);
                m_pLeftSlot.SetGoodsState(eGoodsType, m_pLeftMonster.GetPrice());
            }
        }

        if (null != m_pRightSlot)
        {
            if (null == m_pRightMonster)
            {
                m_pRightSlot.Initialize();
            }
            else
            {
                var eGoodsType = Single.Inventory.GetMonsterGoodsStateToPlayerPrefs(m_eRightType);
                m_pRightSlot.SetGoodsState(eGoodsType, m_pRightMonster.GetPrice());
            }
        }
    }
    #endregion


    #region Utility Functions
    void ReturnStickObject(SHUIWidget_Monster pMonster)
    {
        if (null == pMonster)
            return;

        Single.ObjectPool.Return(pMonster.GetGameObject());
    }
    SHUIWidget_Monster CreateMonsterSlot(eMonsterType eType)
    {
        if ((eMonsterType.None == eType) || (eMonsterType.Max_NormalMonster == eType) || (eMonsterType.Max == eType))
            return null;
        
        return Single.ObjectPool.Get<SHUIWidget_Monster>(SHHard.GetMonsterName(eType),
            ePoolReturnType.ChangeScene, ePoolDestroyType.ChangeScene);
    }
    void SetMonsterSlot(eMonsterType eType, SHUIWidget_Monster pMonster, SHUIWidget_ItemSlot pSlot)
    {
        if ((null == pMonster) || (null == pSlot))
            return;

        SHGameObject.SetParent(pMonster.GetGameObject(), pSlot.GetGameObject());
        pMonster.Initialize(eType, 0.5f, 0.0f, 0.0f);
        pMonster.SetLocalPosition(Vector3.zero);
        pMonster.SetLocalScale(Vector3.one);
        pMonster.StopMoveTween();
        pMonster.SetActive(true);
    }
    #endregion
}
