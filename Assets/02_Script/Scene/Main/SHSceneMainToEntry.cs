using UnityEngine;
using System;
using System.Collections;

public class SHSceneMainToEntry : SHMonoWrapper
{
    #region Members
    #endregion


    #region System Functions
    public override void Start() 
    {
        if (true == Single.AppInfo.IsDevelopment())
            Single.UI.Show("Panel_Development");
        else
            Single.UI.Close("Panel_Development");

        Single.UI.Show("Panel_Entry", (Action)OnEventToNextScene);
    }
    #endregion
    

    #region Event Handler
    void OnEventToNextScene()
    {
        Single.Scene.GoTo(eSceneType.InGame);
    }
    #endregion
}
