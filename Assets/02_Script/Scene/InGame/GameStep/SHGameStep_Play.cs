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
        Single.Damage.SetLockCheckCollision(false);

        // 게임상태 정리
        Single.GameState.ShowCurrentScore();
        Single.GameState.CloseBestScore();
        Single.GameState.SetUpdatePhase();

        // 유닛정리
        Single.Player.StartPlayer();
        Single.Monster.AllKillMonster();
        Single.Monster.StartGen();
        
        // 사운드 출력
        Single.Sound.PlayBGM("Audio_BGM_InGame");
    }
    public override void FinalStep()
    {
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