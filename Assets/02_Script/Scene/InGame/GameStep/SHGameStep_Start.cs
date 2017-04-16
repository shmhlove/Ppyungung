using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Start : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        // UI 정리
        Single.UI.Show("Panel_StartMenu", (Action)(()=> MoveTo(eGameStep.Play)));

        // 게임상태 정리
        Single.GameState.InitPhase();
        Single.GameState.ClearScoreBoard();
        Single.Buff.ClearBuffValue();

        // 사운드 재생
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