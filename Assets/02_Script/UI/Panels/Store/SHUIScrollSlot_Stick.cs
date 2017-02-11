using UnityEngine;
using System;
using System.Collections;

public class SHUIScrollSlot_Stick : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField]  private SHUIWidget_ItemSlot m_pLeftSlot  = null;
    [SerializeField]  private SHUIWidget_ItemSlot m_pRightSlot = null;
    #endregion


    #region Members : Info
    [SerializeField]  public eStickType       m_eLeftType   = eStickType.None;
    [SerializeField]  public eStickType       m_eRightType  = eStickType.None;
    [HideInInspector] public SHUIWidget_Stick m_pLeftStick  = null;
    [HideInInspector] public SHUIWidget_Stick m_pRightStick = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    public void Initialize(eStickType eType1, eStickType eType2)
    {
        ReturnStickObject(m_pLeftStick);
        ReturnStickObject(m_pRightStick);
        SetStickSlot(
            (m_eLeftType  = eType1), 
            (m_pLeftStick = CreateStickSlot(eType1)), 
            m_pLeftSlot);
        SetStickSlot(
            (m_eRightType = eType2), 
            (m_pRightStick = CreateStickSlot(eType2)), 
            m_pRightSlot);
        
        SetSelector();
    }
    public void SetSelector()
    {
        if (null != m_pLeftSlot)
        {
            if (null == m_pLeftStick)
            {
                m_pLeftSlot.Initialize();
            }
            else
            {
                var eGoodsType = Single.Inventory.GetStickGoodsStateToPlayerPrefs(m_eLeftType);
                m_pLeftSlot.SetGoodsState(eGoodsType, m_pLeftStick.GetPrice());
            }
        }

        if (null != m_pRightSlot)
        {
            if (null == m_pRightStick)
            {
                m_pRightSlot.Initialize();
            }
            else
            {
                var eGoodsType = Single.Inventory.GetStickGoodsStateToPlayerPrefs(m_eRightType);
                m_pRightSlot.SetGoodsState(eGoodsType, m_pRightStick.GetPrice());
            }
        }
    }
    #endregion


    #region Utility Functions
    void ReturnStickObject(SHUIWidget_Stick pStick)
    {
        if (null == pStick)
            return;

        Single.ObjectPool.Return(pStick.GetGameObject());
    }
    SHUIWidget_Stick CreateStickSlot(eStickType eType)
    {
        if ((eStickType.None == eType) || (eStickType.Max == eType))
            return null;
        
        return Single.ObjectPool.Get<SHUIWidget_Stick>(SHHard.GetStickName(eType),
            ePoolReturnType.ChangeScene, ePoolDestroyType.ChangeScene);
    }
    void SetStickSlot(eStickType eType, SHUIWidget_Stick pStick, SHUIWidget_ItemSlot pSlot)
    {
        if ((null == pStick) || (null == pSlot))
            return;

        SHGameObject.SetParent(pStick.GetGameObject(), pSlot.GetGameObject());
        pStick.Initialize(eType);
        pStick.SetLocalPosition(Vector3.zero);
        pStick.SetLocalScale(Vector3.one);
        pStick.SetActive(true);
    }
    #endregion
}
