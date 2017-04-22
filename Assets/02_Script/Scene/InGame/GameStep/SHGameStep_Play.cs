using UnityEngine;
using System;
using System.Collections;

public class SHGameStep_Play : SHGameStep_Component
{
    #region Members
    private IEnumerator m_pCoroutinAction = null;
    #endregion


    #region Override Functions
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

        if (null != m_pCoroutinAction)
            return;

        if (true == Single.Player.IsDie())
            SHCoroutine.Instance.StartCoroutine(
                m_pCoroutinAction = CoroutineToAction(eGameStep.Result));

        if (true == Single.GameState.IsPossibleNextPhase())
            SHCoroutine.Instance.StartCoroutine(
                m_pCoroutinAction = CoroutineToAction(eGameStep.ChangePhase));
    }
    #endregion


    #region Coroutine Functions
    IEnumerator CoroutineToAction(eGameStep eNextStep)
    {
        bool bIsDoneAction = false;
        switch(eNextStep)
        {
            case eGameStep.Result:      Single.Root3D.GetMainCamera().PlayCameraGameOver(() => bIsDoneAction = true); break;
            case eGameStep.ChangePhase: Single.Root3D.GetMainCamera().PlayCameraPhase(() => bIsDoneAction = true);    break;
            default:                    bIsDoneAction = true; break;
        }

        Single.Damage.SetLockCheckCollision(true);
        // Single.Timer.SetTimeScale(0.5f);

        while (false == bIsDoneAction)
            yield return null;

        yield return null;
        // Single.Timer.SetTimeScale(1.0f);

        MoveTo(eNextStep);
        m_pCoroutinAction = null;
    }
    #endregion
}