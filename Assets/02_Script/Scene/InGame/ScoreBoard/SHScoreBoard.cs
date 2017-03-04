using UnityEngine;
using System.Collections;

public class SHScoreBoard : SHInGame_Component
{
    #region Members
    public float m_fMoveMeter = 0;
    public int   m_iScore     = 0;
    public int   m_iAddScore  = 0;
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        if (0 != m_iAddScore)
        {
            SetScore(m_iScore + m_iAddScore);
            ShowCurrentScore();
            m_iAddScore = 0;
        }
    }
    #endregion


    #region Interface Functions
    public void Clear()
    {
        m_iScore    = 0;
        m_iAddScore = 0;
        CloseScoreBoard();
    }
    public void ShowBestScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Best", GetBestScore());
    }
    public void AddScore(int iScore)
    {
        m_iAddScore += iScore;
    }
    public void SetMeter(float fMeter)
    {
        m_fMoveMeter = fMeter;

        if (GetBestMeter() < m_fMoveMeter)
            SetBestMeter(m_fMoveMeter);

        Single.UI.Show("Panel_ScoreBoard", "Meter",     m_fMoveMeter);
        Single.UI.Show("Panel_ScoreBoard", "BestMeter", GetBestMeter());
    }
    #endregion


    #region Utility Functions
    private void SetScore(int iScore)
    {
        m_iScore = iScore;

        if (GetBestScore() < m_iScore)
            SetBestScore(m_iScore);
    }
    private void SetBestScore(int iScore)
    {
        SHPlayerPrefs.SetInt("BestScore", iScore);
    }
    private int GetBestScore()
    {
        return SHPlayerPrefs.GetInt("BestScore", 0);
    }
    private void ShowCurrentScore()
    {
        Single.UI.Show("Panel_ScoreBoard", "Current", m_iScore);
    }
    private void SetBestMeter(float fMeter)
    {
        SHPlayerPrefs.SetFloat("BestMeter", fMeter);
    }
    private float GetBestMeter()
    {
        return SHPlayerPrefs.GetFloat("BestMeter", 0.0f);
    }

    private void CloseScoreBoard()
    {
        Single.UI.Close("Panel_ScoreBoard");
    }
    #endregion


    #region Event Handler
    #endregion
}
