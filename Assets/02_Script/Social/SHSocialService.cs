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
#if !UNITY_EDITOR && UNITY_ANDROID
    private static PlayGamesClientConfiguration m_pGPGConfig;
#endif

    #region Virtual Functions
    public override void OnInitialize()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        m_pGPGConfig = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(m_pGPGConfig);
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

        if (true == IsLogin())
        {
            pCallback(true);
            return;
        }

#if UNITY_EDITOR
        pCallback(true);
#else
        Social.localUser.Authenticate((bIsSuccess) => 
        {
            pCallback(bIsSuccess);
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
        if (false == IsLogin())
            pCallback(false);
        else
        {
            Social.Active.ReportScore(
                lScore, GetLeaderBoardType(eType), (bIsSuccess) => 
                {
                    if (false == bIsSuccess)
                        Debug.LogError("LeaderBoard Report 실패!!");
                    else
                        ShowLeaderboard();

                    pCallback(bIsSuccess);
                });
        }
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
