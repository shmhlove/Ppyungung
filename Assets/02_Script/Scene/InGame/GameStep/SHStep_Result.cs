using UnityEngine;
using System;
using System.Collections;

public class SHStep_Result : SHStep_Component
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.Damage.Clear();
        Single.ScoreBoard.ShowBestScore();
        Single.UI.Show("Panel_ResultMenu", (Action)OnEventToRetry);
        Single.UI.Close("Panel_CtrlPad");
    }
    #endregion
    

    #region Event Handler
    public void OnEventToRetry()
    {
        MoveTo(eStep.Play);
    }
    #endregion
}
