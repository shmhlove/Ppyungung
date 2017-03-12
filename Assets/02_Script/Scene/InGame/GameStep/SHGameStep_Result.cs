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
        Single.UI.Close("Panel_CtrlPad");

        // 스코어 출력
        Single.GameState.ShowCurrentScore();
        Single.GameState.ShowBestScore();

        Single.Sound.PlayBGM("Audio_BGM_GameOver");
    }
    public override void FinalStep()
    {
        Single.GameState.ClearScoreBoard();
    }
    #endregion
}
