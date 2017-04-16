using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHUIPanel_PhaseMenu : SHUIBasePanel
{
    #region Members : Inspector
    [SerializeField] private UIToggle   m_pToggleBuff_1 = null;
    [SerializeField] private UIToggle   m_pToggleBuff_2 = null;
    [SerializeField] private UIToggle   m_pToggleBuff_3 = null;
    [SerializeField] private UILabel    m_pLabelBuff_1  = null;
    [SerializeField] private UILabel    m_pLabelBuff_2  = null;
    [SerializeField] private UILabel    m_pLabelBuff_3  = null;
    #endregion


    #region Members : Info
    private List<eBuffType> m_pBuffList       = null;
    private Action          m_pEventToRestart = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 > pArgs.Length))
            return;

        m_pEventToRestart = (Action)pArgs[0];
        ResetBuffSlot();
    }
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    void ResetBuffSlot()
    {
        m_pToggleBuff_1.Set(false);
        m_pToggleBuff_2.Set(false);
        m_pToggleBuff_3.Set(false);

        m_pBuffList = Single.Buff.GetRandomBuffList(3);
        m_pLabelBuff_1.text = Localization.Get(m_pBuffList[0].ToString());
        m_pLabelBuff_2.text = Localization.Get(m_pBuffList[1].ToString());
        m_pLabelBuff_3.text = Localization.Get(m_pBuffList[2].ToString());
    }
    int GetBuffIndex(string strText)
    {
        if (true == strText.Equals(m_pLabelBuff_1.text)) return 0;
        if (true == strText.Equals(m_pLabelBuff_2.text)) return 1;
        if (true == strText.Equals(m_pLabelBuff_3.text)) return 2;

        return -1;
    }
    #endregion
    

    #region Event Handler
    public void OnClickToStartGame()
    {
        if (null == m_pEventToRestart)
            return;

        m_pEventToRestart();
        Close();
    }
    public void OnClickToBuffSlot(string strText)
    {
        if (null == m_pBuffList)
            return;

        var iIndex = GetBuffIndex(strText);
        if ((0 > iIndex) || (m_pBuffList.Count < iIndex))
            return;

        Single.Buff.SetBuff(m_pBuffList[iIndex]);
    }
    #endregion
}