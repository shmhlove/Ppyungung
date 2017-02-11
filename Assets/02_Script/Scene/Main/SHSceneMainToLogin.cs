using UnityEngine;
using System;
using System.Collections;

public class SHSceneMainToLogin : MonoBehaviour 
{
    #region Members
    #endregion


    #region System Functions
    void Start()
    {
        Single.AppInfo.CreateSingleton();
        Single.UI.Show("Panel_Login", (Action<bool>)OnEventToLogin);
    }
    #endregion


    #region Virtual Functions
    #endregion


    #region Interface Functions
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnEventToLogin(bool bIsSuccess)
    {
        if (false == bIsSuccess)
        {
            Single.UI.ShowNotice_NoMake();
        }
        else
        {
            Single.Scene.GoTo(eSceneType.InGame);
        }
    }
    #endregion
}
