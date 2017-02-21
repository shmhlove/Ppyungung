using UnityEngine;
using System;
using System.Collections;

public class SHMainToInGame : MonoBehaviour 
{
    #region Members
    #endregion


    #region System Functions
    void Start()
    {
        if (true == Single.AppInfo.IsDevelopment())
            Single.UI.Show("Panel_Development");
        else
            Single.UI.Close("Panel_Development");

        Single.InGame.StartGame();
    }
    void FixedUpdate()
    {
        Single.InGame.FrameMove();
    }
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    #endregion
}
