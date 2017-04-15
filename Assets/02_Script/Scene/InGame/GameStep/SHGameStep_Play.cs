using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Play : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        // UI 정리
        Single.UI.Show("Panel_CtrlPad");
        Single.UI.Show("Panel_HUD");
        Single.UI.Close("Panel_StartMenu");

        // 데미지 정리
        Single.Damage.Clear();
        Single.Damage.SetLockCheckCollision(false);

        // 게임상태 정리
        Single.GameState.ClearCurrentKillCount();
        Single.GameState.ShowCurrentKillCount();
        Single.GameState.CloseBestKillCount();
        Single.GameState.SetNextPhase();

        // 버프적용
        Single.Buff.ApplyBuff();

        // 유닛정리
        Single.Monster.AllKillMonster();
        Single.Monster.StartGen();
        Single.Player.StartPlayer();

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

        if (true == Single.GameState.IsPossibleNextPhase())
            MoveTo(eGameStep.ChangePhase);
    }
    #endregion
}