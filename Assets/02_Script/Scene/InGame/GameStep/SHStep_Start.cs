using UnityEngine;
using System;
using System.Collections;

public class SHStep_Start : SHStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        Single.Sound.PlayBGM("Audio_BGM_OutGame");
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
}