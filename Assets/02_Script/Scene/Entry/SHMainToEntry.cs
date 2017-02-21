using UnityEngine;
using System;
using System.Collections;

public class SHMainToEntry : SHMonoWrapper
{
    #region Members
    #endregion


    #region System Functions
    public override void Start() 
    {
        //if (true == Single.AppInfo.IsDevelopment())
        //    Single.UI.Show("Panel_Development");
        //else
        //    Single.UI.Close("Panel_Development");

        Single.Scene.GoTo(eSceneType.InGame);
    }
    #endregion
    

    #region Event Handler
    #endregion
}
