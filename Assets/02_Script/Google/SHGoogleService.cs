using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;

public enum eLeaderBoardType
{
    BestScore,
}

public class SHGoogleService : SHSingleton<SHGoogleService>
{
    #region Virtual Functions
    public override void OnInitialize()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#else
#endif
        SetDontDestroy();
    }
    public override void OnFinalize()
    {
        Logout();
    }
    #endregion


    #region Interface : Login
    public void Login(Action<bool> pCallback)
    {
        if (null == pCallback)
            pCallback = (bIsSuccess) => { };

#if UNITY_EDITOR
        pCallback(true);
#elif UNITY_ANDROID
        if (true == IsLogin())
        {
            pCallback(true);
            return;
        }
        
        Social.localUser.Authenticate((bIsSuccess, strMessage) => 
        {
            if (false == string.IsNullOrEmpty(strMessage))
                Debug.LogError(strMessage);
        
            pCallback(bIsSuccess);
        });
#else
        pCallback(false);
#endif
    }
    public void Logout()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (false == IsLogin())
            return;

        ((PlayGamesPlatform)Social.Active).SignOut();
#else
#endif
    }
    public bool IsLogin()
    {
        return Social.localUser.authenticated;
    }
    #endregion


    #region Interface : UserInfo
    public string GetUserID()
    {
        if (false == IsLogin())
            return string.Empty;

        return Social.localUser.id;
    }
    public string GetUserName()
    {
        if (false == IsLogin())
            return string.Empty;

        return Social.localUser.userName;
    }
    public Texture2D GetProfileImage()
    {
        if (false == IsLogin())
            return null;

        return Social.localUser.image;
    }
    #endregion


    #region Interface : LeaderBoard
    public void SetLeaderboard(long lScore, eLeaderBoardType eType, Action<bool> pCallback)
    {
        if (null == pCallback)
            pCallback = (bIsSuccess) => { };

#if UNITY_EDITOR
        pCallback(true);
#elif UNITY_ANDROID
        Action pFunction = () =>
        {
            Social.Active.ReportScore(
                lScore, GetLeaderBoardType(eType), pCallback);
        };
        
        Login((bIsSuccess) =>
        {
            if (false == bIsSuccess)
                pCallback(false);
            else
                pFunction();
        });
#else
        pCallback(false);
#endif
    }
    public void ShowLeaderboard()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        Login((bIsSuccess) =>
        {
            if (true == bIsSuccess)
                Social.Active.ShowLeaderboardUI();
        });
#else
#endif
    }
    #endregion


    #region Utility Functions
    string GetLeaderBoardType(eLeaderBoardType eType)
    {
        switch (eType)
        {
            case eLeaderBoardType.BestScore:
            default:
                return GPGSIds.achievement_1;
        }
    }
    #endregion
}
