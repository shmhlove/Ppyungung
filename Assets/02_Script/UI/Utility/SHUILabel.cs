using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHUILabel : MonoBehaviour
{
    #region Members : Inspector
    public UILabel  m_pLabel     = null;
    public string   m_strPreFix  = string.Empty;
    public string   m_strPostFix = string.Empty;
    #endregion


    #region Interface Functions
    public void SetLabel(string strText)
    {
        if (null == m_pLabel)
            return;

        m_pLabel.text = string.Format("{0}{1}{2}", m_strPreFix, strText, m_strPostFix);
    }
    #endregion
}