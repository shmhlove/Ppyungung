using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;
#endif


public enum eLeaderBoardType
{
    BestScore,
}

public class SHSocialService : SHSingleton<SHSocialService>
{
    #region Virtual Functions
    public override void OnInitialize()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        // var pGPGConfig = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        // PlayGamesPlatform.InitializeInstance(pGPGConfig);
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
    public void Login(Action<bool, string> pCallback)
    {
        if (null == pCallback)
            pCallback = (bIsSuccess, strMessage) => { };

        if (true == IsLogin())
        {
            pCallback(true, "");
            return;
        }

#if UNITY_EDITOR
        pCallback(true, "");
#else
        Social.localUser.Authenticate((bIsSuccess, strMessage) => 
        {
            if (false == string.IsNullOrEmpty(strMessage))
                Debug.LogError(strMessage);
        
            pCallback(bIsSuccess, strMessage);
        });
#endif
    }
    public void Logout()
    {
        if (false == IsLogin())
            return;

#if UNITY_EDITOR
#elif UNITY_ANDROID            
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
#else
        Login((bIsSuccess, strMessage) =>
        {
            if (false == bIsSuccess)
                pCallback(false);
            else
            {
                Social.Active.ReportScore(
                    lScore, GetLeaderBoardType(eType), (bIsReport) => 
                    {
                        ShowLeaderboard();
                        pCallback(bIsReport);
                    });
            }
        });
#endif
    }
    public void ShowLeaderboard()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GetLeaderBoardType(eLeaderBoardType.BestScore));
#else
        Social.ShowLeaderboardUI();
#endif
    }
    #endregion


    #region Utility Functions
    string GetLeaderBoardType(eLeaderBoardType eType)
    {
#if UNITY_ANDROID
        switch (eType)
        {
            case eLeaderBoardType.BestScore:
            default:
			return GPGSIds.leaderboard_high_score;
        }
#else
        switch (eType)
        {
            case eLeaderBoardType.BestScore:
            default:
                return "PPYUNGUNG_HIGHT_SCORE";
        }
#endif
    }
    #endregion
}
