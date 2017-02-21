using UnityEngine;
using System;
using System.Collections;

public class SHStep_Play : SHStepBase
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.UI.Show("Panel_CtrlPad");
    }
    public override void FinalStep()
    {
    }
    public override void FrameMove(int iCallCnt)
    {
        base.FrameMove(iCallCnt);
    }
    #endregion


    #region Event Handler
    #endregion
}