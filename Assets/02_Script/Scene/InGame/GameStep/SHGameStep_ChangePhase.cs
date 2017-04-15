using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_ChangePhase : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        // UI 정리
        Single.UI.Show("Panel_PhaseMenu", (Action)(() => MoveTo(eGameStep.Play)));
        Single.UI.Close("Panel_StartMenu");
        Single.UI.Close("Panel_CtrlPad");
        
        // 데미지 정리
        Single.Damage.SetLockCheckCollision(true);

        // 유닛정리
        Single.Monster.StopGen();
        Single.Monster.AllDieMonster();
        Single.Player.StopPlayer();

        // 스코어 출력
        Single.GameState.ShowCurrentKillCount();
        Single.GameState.ShowBestKillCount();

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