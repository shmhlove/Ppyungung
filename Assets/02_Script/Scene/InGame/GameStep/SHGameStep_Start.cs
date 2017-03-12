using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Start : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        Single.GameState.ClearScoreBoard();
        Single.UI.Show("Panel_StartMenu", (Action)(()=> MoveTo(eGameStep.Play)));

        Single.Sound.PlayBGM("Audio_BGM_OutGame");
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