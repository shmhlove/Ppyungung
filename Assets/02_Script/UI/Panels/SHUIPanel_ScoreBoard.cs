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
    [Header("Combo")]
    public UILabel          m_pLabelCombo           = null;
    public UILabel          m_pLabelComboAction     = null;
    public TweenPosition    m_pTweenComboPosition   = null;
    public TweenAlpha       m_pTweenComboAlpha      = null;
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
            case "current": SetCurrentScore((int)pArgs[1]); break;
            case "best":    SetBestScore((int)pArgs[1]);    break;
            case "combo":   SetCombo((int)pArgs[1]);        break;
        }
    }
    #endregion
    

    #region Utility Functions
    void SetCurrentScore(int iScore)
    {
        m_pLabelCurrent.text = iScore.ToString();
        m_pLabelAction.text  = iScore.ToString();

        m_pTweenScale.ResetToBeginning();
        m_pTweenScale.PlayForward();
        m_pTweenAlpha.ResetToBeginning();
        m_pTweenAlpha.PlayForward();
    }
    void SetBestScore(int iScore)
    {
        m_pLabelBest.text = string.Format("★ {0} ★", iScore);
    }
    void SetCombo(int iCombo)
    {
        m_pLabelCombo.text       = iCombo.ToString();
        m_pLabelComboAction.text = iCombo.ToString();
        m_pLabelComboAction.gameObject.SetActive(0 != iCombo);

        m_pTweenComboPosition.ResetToBeginning();
        m_pTweenComboPosition.PlayForward();
        m_pTweenComboAlpha.ResetToBeginning();
        m_pTweenComboAlpha.PlayForward();
    }
    #endregion
}
