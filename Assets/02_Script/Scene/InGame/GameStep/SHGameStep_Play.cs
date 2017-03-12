using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Play : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        Single.UI.Show("Panel_CtrlPad");
        Single.UI.Show("Panel_HUD");

        // 데미지 정리
        Single.Damage.Clear();

        // 게임상태 정리
        Single.GameState.ShowCurrentScore();
        Single.GameState.CloseBestScore();
        Single.GameState.SetUpdatePhaseID();

        // 유닛정리
        Single.Player.StartPlayer();
        Single.Monster.AllKillMonster();
        Single.Monster.StartMonster();
        
        Single.Sound.PlayBGM("Audio_BGM_InGame");
    }
    public override void FinalStep()
    {
        Single.UI.Close("Panel_CtrlPad");
        Single.UI.Close("Panel_HUD");

        Single.Player.StopPlayer();
    }
    public override void FrameMove(int iCallCnt)
    {
        base.FrameMove(iCallCnt);

        if (false == Single.Player.IsActive())
            MoveTo(eGameStep.Result);

        if (true == Single.GameState.IsNextPhase())
            MoveTo(eGameStep.ChangePhase);
    }
    #endregion
}