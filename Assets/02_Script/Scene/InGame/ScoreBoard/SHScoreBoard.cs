using UnityEngine;
using System.Collections;

public class SHScoreBoard : SHInGame_Component
{
    #region Members
    public int   m_iScore     = 0;
    public int   m_iCombo     = 0;
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void Clear()
    {
        m_iScore    = 0;
        m_iCombo    = 0;
        CloseScoreBoard();
    }
    public void ShowScore()
    {
        ShowCurrentScore();
        ShowBestScore();
        ShowComboScore();
    }
    public void AddScore(int iScore)
    {
        if (0 == iScore)
            return;

        var fComboSecond = Single.Timer.GetDeltaTimeToSecond("ScoreBoard_ComboTime");
        if (fComboSecond < SHHard.m_fComboTime)
            AddCombo(1);
        
        if (GetBestScore() < (m_iScore += iScore))
            SetBestScore(m_iScore);

        ShowCurrentScore();
        ShowBestScore();

        Single.Timer.StartDeltaTime("ScoreBoard_ComboTime");
    }
    public void AddCombo(int iCombo)
    {
        if (0 == iCombo)
            return;

        m_iCombo += iCombo;
        ShowComboScore();
    }
    public int GetBestScore()
    {
        return SHPlayerPrefs.GetInt("BestScore", 0);
    }
    #endregion


    #region Utility Functions
    void ShowCurrentScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Current", m_iScore);
    }
    void ShowBestScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Best", GetBestScore());
    }
    void ShowComboScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Combo", m_iCombo);
    }
    void SetBestScore(int iScore)
    {
        SHPlayerPrefs.SetInt("BestScore", iScore);
    }
    void CloseScoreBoard()
    {
        Single.UI.Close("Panel_ScoreBoard");
    }
    #endregion
}
