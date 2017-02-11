using UnityEngine;
using System;
using System.Collections;

public class SHStep_Play : SHStepBase
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.Sound.PlayBGM("Audio_BGM_Pineapple");
        Single.UI.Show("Panel_CtrlPad", (Action)OnEventToTouch);
    }
    public override void FinalStep()
    {
        Single.Player.Stop();
        Single.Monster.Stop();
        Single.Sound.StopBGM("Audio_BGM_Pineapple");
        Single.Sound.PlayBGM("Audio_BGM_GameOver");
        Single.UI.Close("Panel_CtrlPad");
    }
    public override void FrameMove(int iCallCnt)
    {
        base.FrameMove(iCallCnt);

        if (true == Input.GetKeyDown(KeyCode.Space))
        {
            OnEventToTouch();
        }
    }
    #endregion


    #region Event Handler
    public void OnEventToTouch()
    {
        Single.Player.OnEventToTouch(OnEventToPassStick);
    }
    public void OnEventToPassStick()
    {
        MoveTo(eGameStep.Result);
    }
    #endregion
}