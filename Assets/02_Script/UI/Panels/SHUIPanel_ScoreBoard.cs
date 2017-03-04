using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_ScoreBoard : SHUIBasePanel
{
    #region Members : Inspector
    [Header("Score")]
    public UILabel    m_pLabelCurrent = null;
    public UILabel    m_pLabelAction  = null;
    public UILabel    m_pLabelBest    = null;
    public TweenScale m_pTweenScale   = null;
    public TweenAlpha m_pTweenAlpha   = null;
    [Header("Meter")]
    public UILabel    m_pLabelMeter   = null;
    public UILabel    m_pLabelBestMeter = null;
    #endregion


    #region Members : Event
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (2 != pArgs.Length))
            return;

        switch(((string)pArgs[0]).ToLower())
        {
            case "current":     SetCurrentScore((int)pArgs[1]);     break;
            case "best":        SetBestScore((int)pArgs[1]);        break;
            case "meter":       SetCurrentMeter((float)pArgs[1]);   break;
            case "bestmeter":   SetBestMeter((float)pArgs[1]);      break;
        }
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    void SetCurrentScore(int iScore)
    {
        m_pLabelCurrent.text = iScore.ToString();
        m_pLabelAction.text = iScore.ToString();

        m_pTweenScale.ResetToBeginning();
        m_pTweenScale.PlayForward();
        m_pTweenAlpha.ResetToBeginning();
        m_pTweenAlpha.PlayForward();

        m_pLabelBest.gameObject.SetActive(false);
    }
    void SetBestScore(int iScore)
    {
        m_pLabelBest.text = string.Format("★ {0} ★", iScore);
        m_pLabelBest.gameObject.SetActive(true);
    }
    void SetCurrentMeter(float fMeter)
    {
        m_pLabelMeter.text = fMeter.ToString("0.00");
    }
    void SetBestMeter(float fBestMeter)
    {
        m_pLabelBestMeter.text = fBestMeter.ToString("0.00");
    }
    #endregion


    #region Event Handler
    #endregion
}
