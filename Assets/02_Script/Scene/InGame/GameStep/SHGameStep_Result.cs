using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Result : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        // UI 정리
        Single.UI.Show("Panel_ResultMenu", (Action)(() => MoveTo(eGameStep.Play)));
        Single.UI.Close("Panel_StartMenu");
        Single.UI.Close("Panel_CtrlPad");
        Single.UI.Close("Panel_HUD");

        // 데미지 정리
        Single.Damage.SetLockCheckCollision(true);

        // 유닛정리
        Single.Monster.StopGen();
        Single.Player.StopPlayer();
        Single.Player.ClearPlayer();

        // 스코어 출력
        Single.GameState.ShowCurrentKillCount();
        Single.GameState.ShowBestKillCount();

        // 사운드 출력
        Single.Sound.PlayBGM("Audio_BGM_GameOver");
    }
    public override void FinalStep()
    {
        // 스코어 보드 정리
        Single.GameState.ClearScoreBoard();
    }
    #endregion
}
