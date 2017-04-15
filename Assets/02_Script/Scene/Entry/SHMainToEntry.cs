using UnityEngine;
using System;
using System.Collections;

public class SHMainToEntry : SHMonoWrapper
{
    public override void Start() 
    {
        if (true == Single.AppInfo.IsDevelopment())
            Single.UI.Show("Panel_Development");
        else
            Single.UI.Close("Panel_Development");

        Single.Scene.GoTo(eSceneType.InGame);
    }
}
