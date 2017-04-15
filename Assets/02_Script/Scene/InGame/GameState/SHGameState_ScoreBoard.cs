using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SHGameState : SHInGame_Component
{
    #region Members
    private int   m_iCurrentKillCount = 0;
    private int   m_iTotalKillCount   = 0;
    #endregion
    

    #region Interface Functions
    public void ClearScoreBoard()
    {
        m_iCurrentKillCount = 0;
        m_iTotalKillCount   = 0;

        CloseUI();
    }
    public void ClearCurrentKillCount()
    {
        m_iCurrentKillCount = 0;
    }
    public int GetCurrentKillCount()
    {
        return m_iCurrentKillCount;
    }
    public int GetTotalKillCount()
    {
        return m_iTotalKillCount;
    }
    public int GetBestKillCount()
    {
        return SHPlayerPrefs.GetInt("BestScore", 0);
    }
    public void SetKillCount(int iScore)
    {
        if (GetBestKillCount() < (m_iCurrentKillCount = iScore))
            SaveBestScore(m_iCurrentKillCount);

        ShowCurrentKillCount();
    }
    public void AddKillCount(int iScore)
    {
        if (0 == iScore)
            return;

        m_iCurrentKillCount += iScore;

        if (GetBestKillCount() < (m_iTotalKillCount += iScore))
            SaveBestScore(m_iTotalKillCount);

        ShowCurrentKillCount();
    }
    public void ShowCurrentKillCount()
    {
        Single.UI.Show("Panel_ScoreBoard", "Open_Current", m_iTotalKillCount);
    }
    public void ShowBestKillCount()
    {
        Single.UI.Show("Panel_ScoreBoard", "Open_Best", GetBestKillCount());
    }
    public void CloseBestKillCount()
    {
        Single.UI.Show("Panel_ScoreBoard", "Close_Best");
    }
    #endregion


    #region Utility Functions
    void CloseUI()
    {
        Single.UI.Close("Panel_ScoreBoard");
    }
    void SaveBestScore(int iScore)
    {
        SHPlayerPrefs.SetInt("BestScore", iScore);
    }
    #endregion
}
