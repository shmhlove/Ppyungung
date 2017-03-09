using UnityEngine;
using System;
using System.Collections;

public class SHStep_Play : SHStep_Component
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.UI.Show("Panel_CtrlPad");
        Single.UI.Show("Panel_HUD");

        Single.ScoreBoard.Clear();
        Single.Player.StartPlayer();
        Single.Monster.StartMonster();
        Single.Sound.PlayBGM("Audio_BGM_InGame");
    }
    public override void FinalStep()
    {
        Single.UI.Close("Panel_CtrlPad");
        Single.UI.Close("Panel_HUD");

        Single.Player.StopPlayer();
        Single.Monster.StopMonster();
    }
    public override void FrameMove(int iCallCnt)
    {
        base.FrameMove(iCallCnt);

        if (true == Single.Player.IsDie())
            MoveTo(eStep.Result);
    }
    #endregion
}