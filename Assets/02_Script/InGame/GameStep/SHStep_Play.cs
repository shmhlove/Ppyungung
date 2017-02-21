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
        var pPlayer = Single.ObjectPool.Get<SHPlayer>("Player");
        pPlayer.SetActive(true);
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