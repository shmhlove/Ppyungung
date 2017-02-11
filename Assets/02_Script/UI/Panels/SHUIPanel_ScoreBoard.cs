using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_ScoreBoard : SHUIBasePanel
{
    #region Members : Inspector
    public UILabel    m_pLabelCurrent = null;
    public UILabel    m_pLabelAction  = null;
    public UILabel    m_pLabelBest    = null;
    public TweenScale m_pTweenScale   = null;
    public TweenAlpha m_pTweenAlpha   = null;
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
            case "current":     SetCurrentScore((int)pArgs[1]); break;
            case "best":        SetBestScore((int)pArgs[1]);    break;
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
    #endregion


    #region Event Handler
    #endregion
}
