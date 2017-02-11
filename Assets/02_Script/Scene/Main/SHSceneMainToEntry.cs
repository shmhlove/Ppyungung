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
        base.Start();
        Single.AppInfo.CreateSingleton();
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
