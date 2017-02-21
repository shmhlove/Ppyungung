using UnityEngine;
using System;
using System.Collections;

public class SHStep_Start : SHStep_Component
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.ScoreBoard.Clear();
        Single.UI.Show("Panel_StartMenu", (Action)(()=> MoveTo(eStep.Play)));
    }
    public override void FinalStep()
    {
    }
    public override void FrameMove(int iCallCnt)
    {
        base.FrameMove(iCallCnt);
    }
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    #endregion
}