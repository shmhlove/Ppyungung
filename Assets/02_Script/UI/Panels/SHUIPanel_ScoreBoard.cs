using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_ScoreBoard : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Score")]
    public UILabel          m_pLabelCurrent         = null;
    public UILabel          m_pLabelAction          = null;
    public UILabel          m_pLabelBest            = null;
    public TweenScale       m_pTweenScale           = null;
    public TweenAlpha       m_pTweenAlpha           = null;
    #endregion
    

    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        switch(((string)pArgs[0]).ToLower())
        {
            case "open_current":     SetCurrentScore((int)pArgs[1]);    break;
            case "open_best":        SetBestScore((int)pArgs[1]);       break;
            case "close_current":    SetActiveCurrentScore(false);      break;
            case "close_best":       SetActiveBestScore(false);         break;
        }
    }
    #endregion
    

    #region Utility Functions
    void SetCurrentScore(int iScore)
    {
        SetActiveCurrentScore(true);

        m_pLabelCurrent.text = iScore.ToString();
        m_pLabelAction.text  = iScore.ToString();

        m_pTweenScale.ResetToBeginning();
        m_pTweenScale.PlayForward();
        m_pTweenAlpha.ResetToBeginning();
        m_pTweenAlpha.PlayForward();
    }
    void SetBestScore(int iScore)
    {
        SetActiveBestScore(true);
        m_pLabelBest.text = string.Format("★ {0} ★", iScore);
    }
    void SetActiveCurrentScore(bool bIsActive)
    {
        NGUITools.SetActive(m_pLabelCurrent.gameObject, bIsActive);
    }
    void SetActiveBestScore(bool bIsActive)
    {
        NGUITools.SetActive(m_pLabelBest.gameObject, bIsActive);
    }
    #endregion
}