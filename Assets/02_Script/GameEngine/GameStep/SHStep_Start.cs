using UnityEngine;
using System;
using System.Collections;

public class SHStep_Start : SHStepBase
{
    #region Members
    #endregion


    #region Virtual Functions
    public override void InitialStep()
    {
        Single.ScoreBoard.Clear();
        Single.Inventory.Clear();
        Single.Player.StartStick();
        Single.Monster.StartMonster();
        Single.UI.Show("Panel_StartMenu", (Action)OnEventToTouch);
    }
    public override void FinalStep()
    {
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


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnEventToTouch()
    {
        Single.Player.OnEventToTouch(null);
        MoveTo(eGameStep.Play);
    }
    #endregion
}