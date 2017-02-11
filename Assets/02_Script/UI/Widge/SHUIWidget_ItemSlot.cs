using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_ItemSlot : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField]  private UILabel    m_pLabelPrice  = null;
    [SerializeField]  private GameObject m_pSelector    = null;
    [SerializeField]  private GameObject m_pLock        = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    public void Initialize()
    {
        SetActiceToLock(false);
        SetPrice(0);
        SetActiceToSelector(false);
    }
    public void SetGoodsState(eGoodsState eState, int iPrice)
    {
        switch(eState)
        {
            case eGoodsState.NotHas:
                SetActiceToLock(true);
                SetPrice(iPrice);
                SetActiceToSelector(false);
                break;
            case eGoodsState.Disable:
                SetActiceToLock(false);
                SetPrice(iPrice);
                SetActiceToSelector(false);
                break;
            case eGoodsState.Enable:
                SetActiceToLock(false);
                SetPrice(iPrice);
                SetActiceToSelector(true);
                break;
        }
    }
    public void SetActiceToSelector(bool bIsActive)
    {
        if (null == m_pSelector)
            return;

        m_pSelector.SetActive(bIsActive);
    }
    public void SetActiceToLock(bool bIsActive)
    {
        if (null == m_pLock)
            return;

        m_pLock.SetActive(bIsActive);
    }
    public void SetPrice(int iPrice)
    {
        if (null == m_pLabelPrice)
            return;

        m_pLabelPrice.text = iPrice.ToString();
    }
    #endregion


    #region Utility Functions
    #endregion


    #region Event Functions
    #endregion
}
