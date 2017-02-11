using UnityEngine;
using System.Collections;

public partial class SHInventory : SHBaseEngine
{
    #region Members
    public int m_iCoin      { get; private set; }
    public int m_iAddCoin   { get; private set; }
    #endregion


    #region Interface Functions
    public void InitCoin()
    {
        m_iCoin     = GetCoin();
        m_iAddCoin  = 0;
    }
    public void OnUpdateCoin()
    {
        if (0 == m_iAddCoin)
            return;

        SetCoin(m_iCoin + m_iAddCoin);
        ShowCoinUI();
        m_iAddCoin = 0;
    }
    public void AddCoin(int iCoin)
    {
        m_iAddCoin += iCoin;
    }
    public void ConsumeCoin(int iCoin)
    {
        m_iAddCoin -= iCoin;
        m_iAddCoin = Mathf.Clamp(m_iAddCoin, 0, m_iAddCoin);
    }
    #endregion


    #region Utility Functions
    private void SetCoin(int iCoin)
    {
        SHPlayerPrefs.SetInt("Inventory_Coin", (m_iCoin = iCoin));
    }
    private int GetCoin()
    {
        return SHPlayerPrefs.GetInt("Inventory_Coin", 0);
    }
    public void ShowCoinUI()
    {
        Single.UI.Show("Panel_HUD", "Coin", m_iCoin);
    }
    #endregion
}
