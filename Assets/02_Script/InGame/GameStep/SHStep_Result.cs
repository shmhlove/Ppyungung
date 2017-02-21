using UnityEngine;
using System;
using System.Collections;

public class SHStep_Result : SHStepBase
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.ScoreBoard.ShowBestScore();
        Single.UI.Show("Panel_ResultMenu", (Action)OnEventToRetry);
    }
    #endregion
    

    #region Event Handler
    public void OnEventToRetry()
    {
        MoveTo(eGameStep.Start);
    }
    #endregion
}
