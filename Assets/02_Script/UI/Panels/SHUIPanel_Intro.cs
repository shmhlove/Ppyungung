using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_Intro : SHUIBasePanel
{
    #region Members : Inspector
    [Header("IntroInfo")]
    [SerializeField] private GameObject     m_pButton        = null;
    [SerializeField] private GameObject     m_pLogo          = null;
    [SerializeField] private AnimationClip  m_pStartLogoClip = null;
    #endregion


    #region Members : Event
    private Action m_pEventToScreen = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 > pArgs.Length))
            return;

        m_pEventToScreen = (Action)pArgs[0];
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnClickToScreen()
    {
        if (null != m_pStartLogoClip)
            PlayAnim(eDirection.Front, m_pLogo, m_pStartLogoClip, null);

        if (null != m_pButton)
            m_pButton.SetActive(false);
        
        SHCoroutine.Instance.WaitTime(() =>
        {
            if (null != m_pEventToScreen)
                m_pEventToScreen();
        }, 0.2f);
    }
    #endregion
}
