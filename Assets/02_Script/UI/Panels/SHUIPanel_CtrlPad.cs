using UnityEngine;
using System;
using System.Collections;

public class SHUIPanel_CtrlPad : SHUIBasePanel
{
    #region Members
    private Action m_pEventToPress = null;
    #endregion

    
    #region Virtual Functions
    public override void OnBeforeShow(params object[] pArgs)
    {
        if ((null == pArgs) || (1 != pArgs.Length))
            return;

        m_pEventToPress = (Action)pArgs[0];
    }
    #endregion
    

    #region Event Handler
    public void OnPressToTouchPad()
    {
        if (null == m_pEventToPress)
            return;

        m_pEventToPress();
    }
    #endregion
}
