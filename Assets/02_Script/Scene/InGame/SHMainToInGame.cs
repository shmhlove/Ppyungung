using UnityEngine;
using System;
using System.Collections;

public class SHMainToInGame : MonoBehaviour 
{
    void Start()
    {
        if (true == Single.AppInfo.IsDevelopment())
            Single.UI.Show("Panel_Development");
        else
            Single.UI.Close("Panel_Development");

        Single.InGame.StartInGame();
    }
    void FixedUpdate()
    {
        Single.InGame.FrameMove();
    }

    [FuncButton] void OnNextPhase()
    {
        Single.GameState.SetNextPhase();
        Single.GameStep.MoveTo(eGameStep.ChangePhase);
    }
}
