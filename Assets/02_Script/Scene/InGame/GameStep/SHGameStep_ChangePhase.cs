﻿using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_ChangePhase : SHGameStep_Component
{
    #region Virtual Functions
    public override void InitialStep()
    {
        // UI 정리
        Single.UI.Show("Panel_PhaseMenu", (Action)(() => MoveTo(eGameStep.Play)));
        Single.UI.Close("Panel_CtrlPad");

        // 데미지 정리
        Single.Damage.DelAllDamage();
        
        // 몬스터 터트리기
        Single.Monster.AllKillMonster();
        Single.Monster.StopMonster();

        // 스코어 출력
        Single.GameState.ShowCurrentScore();
        Single.GameState.ShowBestScore();

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