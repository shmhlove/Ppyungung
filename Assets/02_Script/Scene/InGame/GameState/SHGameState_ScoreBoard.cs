using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SHGameState : SHInGame_Component
{
    #region Members
    public int   m_iScore = 0;
    #endregion
    

    #region Interface Functions
    public void ClearScoreBoard()
    {
        m_iScore = 0;
        CloseUI();
    }
    public void AddScore(int iScore)
    {
        if (0 == iScore)
            return;
        
        if (GetBestScore() < (m_iScore += iScore))
            SaveBestScore(m_iScore);

        ShowCurrentScore();
    }
    public int GetBestScore()
    {
        return SHPlayerPrefs.GetInt("BestScore", 0);
    }
    public void ShowCurrentScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Open_Current", m_iScore);
    }
    public void ShowBestScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Open_Best", GetBestScore());
    }
    public void CloseCurrentScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Close_Current");
    }
    public void CloseBestScore()
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
