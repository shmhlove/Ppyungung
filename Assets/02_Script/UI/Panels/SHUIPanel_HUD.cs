using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_HUD : SHUIBasePanel
{
    #region Members : Inspector
    public UILabel       m_pLabelCoin            = null;
    public UILabel       m_pLabelCoinAction      = null;
    public TweenScale    m_pTweenCoinActionScale = null;
    public TweenAlpha    m_pTweenCoinActionAlpha = null;
    public SHMonoWrapper m_pCoinTarget           = null;
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
            case "coin":     SetCurrentCoin((int)pArgs[1]); break;
        }

        Single.Damage.AddUnit(m_pCoinTarget);
    }
    public override void OnBeforeClose()
    {
        Single.Damage.DelUnit(m_pCoinTarget);
    }
    #endregion


    #region Interface Functions
    public GameObject GetCoinTarget()
    {
        if (null == m_pCoinTarget)
            return null;

        return m_pCoinTarget.GetGameObject();
    }
    #endregion


    #region Utility Functions
    void SetCurrentCoin(int iCoin)
    {
        m_pLabelCoin.text       = iCoin.ToString();
        m_pLabelCoinAction.text = iCoin.ToString();

        m_pTweenCoinActionScale.ResetToBeginning();
        m_pTweenCoinActionScale.PlayForward();
        m_pTweenCoinActionAlpha.ResetToBeginning();
        m_pTweenCoinActionAlpha.PlayForward();
    }
    #endregion


    #region Event Handler
    #endregion
}
