﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

using DicRealLoadInfo = System.Collections.Generic.Dictionary<eSceneType, System.Collections.Generic.List<string>>;

public partial class SHApplicationInfo : SHSingleton<SHApplicationInfo>
{
    #region Members
    // 크레시 레포트
    [Header("Crittercism")]
    [SerializeField] private string             m_strAppKeyForAOS   = string.Empty;
    [SerializeField] private string             m_strAppKeyForIOS   = string.Empty;

    // 로컬라이즈
    [Header("Localization")]
    [SerializeField] private List<string>       m_pLocalFiles       = new List<string>();
    [SerializeField] private eLanguage          m_eLanguage         = eLanguage.None;

    // 배포제한 시간정보
    [Header("Release")]
    [SerializeField] private SHReleaseTimer     m_pReleaseTime      = new SHReleaseTimer();

    // 컴포넌트(디버그) : 디버그용 정보출력 
    [Header("Debug")]
    [SerializeField] private GUIText            m_pDebugText        = null;

    // 기타(디버그) : FPS 출력용 델타타임
    [ReadOnlyField]
    [SerializeField] private float              m_fDeltaTime        = 0.0f;

    // 기타(디버그) : 로드 시도된 리소스 리스트
    [HideInInspector] private DicRealLoadInfo   m_dicRealLoadInfo   = new DicRealLoadInfo();

    // 기타 : 앱 종료 여부
    [HideInInspector] public bool               m_bIsAppQuit        = false;
    #endregion


    #region System Functions
    // 시스템 : App Quit
    void OnApplicationQuit()
    {
        m_bIsAppQuit = true;
    }

    // 시스템 : App Pause
    void OnApplicationPause(bool bIsPause)
    {
    }

    // 시스템 : App Focus
    eBOOL m_eIsFocus = eBOOL.None;
    void OnApplicationFocus(bool bIsFocus)
    {
        // 초기 실행으로 인해 Focus가 true일때는 체크 무시
        if (m_eIsFocus == eBOOL.None)
        {
            m_eIsFocus = bIsFocus ? eBOOL.True : eBOOL.False;
            return;
        }

        // Focus가 true일때 아래 기능 동작할 수 있도록
        if (eBOOL.True != (m_eIsFocus = bIsFocus ? eBOOL.True : eBOOL.False))
            return;

        // 서비스상태 체크 후 Run이 아니면 인트로로 보낸다.
        CheckServiceState((eResult) =>
        {
            if (eServiceState.Run != eResult)
                Single.Scene.GoTo(eSceneType.Entry);
        });
    }

    // 시스템 : Net Disconnect
    void OnDisconnectedFromServer(NetworkDisconnection pInfo)
    {

    }

    // 시스템 : 초기화
    public override void Start()
    {
        base.Start();

        SetDontDestroy();
        
        // 언어설정
        SetLocalization();

        // 어플리케이션 정보설정
        SetApplicationInfo();

        // 디버그 기능
        StartCoroutine(PrintGameInfo());
        StartCoroutine(CheckReleaseTime());
    }

    // 시스템 : 업데이트
    public override void Update()
    {
        m_fDeltaTime += (Time.deltaTime - m_fDeltaTime) * 0.1f;

        if (true == Input.GetKeyDown (KeyCode.Escape)) 
		{
			Single.UI.Show("Panel_Notice", new NoticeUI_Param()
			{
				m_eButtonType = eNoticeButton.Two,
				m_eIconType   = eNoticeIcon.Warning,
				m_strTitle    = "게임 종료",
				m_strMessage  = "정말 게임을 종료하시겠습니까?",
				m_pEventToOK  = SHUtils.GameQuit,
			});
		}
    }

    // 시스템 : GUI 업데이트
    void OnGUI()
    {
        DrawAppInformation();
        ControlRenderFrame();
    }
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    #endregion


    #region Interface Functions
    // 인터페이스 : 언어설정
    public void SetLocalization()
    {
        var pByte = LoadLocalization();
        if (0 == pByte.Count)
            return;
        
        Localization.localizationHasBeenSet = false;
        Localization.LoadCSV(pByte.ToArray());
        if (null != Localization.onLocalize)
            Localization.onLocalize();
        
        UIRoot.Broadcast("OnLocalize");
        Localization.language = GetLanguage().ToString();
    }
    // 인터페이스 : 어플리케이션 정보설정
    public void SetApplicationInfo()
    {
        var pClientInfo = Single.Table.GetTable<JsonClientConfiguration>();
        SetVSync(pClientInfo.GetVSyncCount());
        SetFrameRate(pClientInfo.GetFrameRate());
        SetCacheInfo(pClientInfo.GetCacheSize(), 30);
        SetSleepMode();
        SetCrittercism();

		UnityEngine.Debug.LogFormat ("ProcessID : {0}", GetProcessID());
		UnityEngine.Debug.LogFormat ("DebugPort : {0}", GetDebugPort());
    }

    // 인터페이스 : 화면 회전모드 확인
    public bool IsLandscape()
    {
        return ((true == IsEditorMode()) ||
                (Screen.orientation == ScreenOrientation.Landscape) ||
                (Screen.orientation == ScreenOrientation.LandscapeLeft) ||
                (Screen.orientation == ScreenOrientation.LandscapeRight));
    }

    // 인터페이스 : 에디터 모드 체크
    public bool IsEditorMode()
    {
        return ((Application.platform == RuntimePlatform.WindowsEditor) ||
                (Application.platform == RuntimePlatform.OSXEditor));
    }

    // 인터페이스 : 실행 플랫폼
    public RuntimePlatform GetRuntimePlatform()
    {
        return Application.platform;
    }
    public string GetStrToRuntimePlatform()
    {
        return SHHard.GetStrToPlatform(GetRuntimePlatform());
    }
    
    // 인터페이스 : 현재 서비스 상태 체크
    public void CheckServiceState(Action<eServiceState> pCallback)
    {
        Single.Table.DownloadServerConfiguration(() =>
        {
            if (null == pCallback)
                return;

            pCallback(GetServiceState());
        });
    }

    // 인터페이스 : 서비스 상태
    public eServiceState GetServiceState()
    {
        var eState = Single.Table.GetServiceState();
        if (eServiceState.None == eState)
            return eServiceState.ConnectMarket;
        else
            return eState;
    }

    // 인터페이스 : 앱 이름
    public string GetAppName()
    {
        return Application.bundleIdentifier.Split('.')[2];
    }
    #endregion


    #region Utility Functions
    // 유틸 : VSync 설정
    void SetVSync(int iCount)
    {
        QualitySettings.vSyncCount = iCount;
    }

    // 유틸 : 프레임 레이트 설정
    void SetFrameRate(int iFrame)
    {
        Application.targetFrameRate = iFrame;
    }

    // 유틸 : SleepMode 설정
    void SetSleepMode()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // 유틸 : 캐시크기 및 완료기간 설정
    void SetCacheInfo(long lSizeMB, int iExpirationMonth)
    {
        Caching.maximumAvailableDiskSpace   = lSizeMB * 1024 * 1024;
        Caching.expirationDelay             = 60 * 60 * 24 * iExpirationMonth;
    }

    // 유틸 : 크래시 래포트 설정
    void SetCrittercism()
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogFormat("AOS Crittercism : {0}", m_strAppKeyForAOS);
        UnityEngine.Debug.LogFormat("IOS Crittercism : {0}", m_strAppKeyForIOS);
#elif UNITY_ANDROID
        UnityEngine.Debug.LogFormat("Crittercism.DidCrashOnLastLoad = {0}", CrittercismAndroid.DidCrashOnLastLoad());
		CrittercismAndroid.Init(m_strAppKeyForAOS);
        CrittercismAndroid.SetLogUnhandledExceptionAsCrash(true);
#elif UNITY_IPHONE || UNITY_IOS
        UnityEngine.Debug.LogFormat("Crittercism.DidCrashOnLastLoad = {0}", CrittercismIOS.DidCrashOnLastLoad());
		CrittercismIOS.Init(m_strAppKeyForIOS);
        CrittercismIOS.SetLogUnhandledExceptionAsCrash(true);
#endif
    }

    // 유틸 : 해상도 비율값
    int GetRatioW(int iValue)
    {
        return (int)(iValue * (Screen.width / 1280.0f));
    }
    int GetRatioH(int iValue)
    {
        return (int)(iValue * (Screen.height / 720.0f));
    }

    // 유틸 : 언어설정
    eLanguage GetLanguage()
    {
        m_eLanguage = (eLanguage)SHPlayerPrefs.GetInt("ApplicationInfo_Language", 0);
        if (eLanguage.None == m_eLanguage)
            SetLanguage(SHHard.GetSystemLanguage());

        return m_eLanguage;
    }
    void SetLanguage(eLanguage eLang)
    {
        m_eLanguage = eLang;
        SHPlayerPrefs.SetInt("ApplicationInfo_Language", (int)m_eLanguage);
    }

    // 유틸 : 언어파일 로드
    List<byte> LoadLocalization()
    {
        var pByte    = new List<byte>();
        var pNewLine = System.Text.Encoding.UTF8.GetBytes("\n");
        SHUtils.ForToList(m_pLocalFiles, (strFile) =>
        {
            var pData = Single.Resource.GetTextAsset(strFile);
            if (null == pData)
                return;

            pByte.AddRange(pNewLine);
            pByte.AddRange(pData.bytes);
        });

        return pByte;
    }
    #endregion


    #region 에디터 테스트
    // 디버그 : 실시간 로드 리소스 리스트 파일로 출력
    [FuncButton]
    public void SaveLoadResourceList()
    {
        string strBuff = string.Empty;
        SHUtils.ForToDic(m_dicRealLoadInfo, (pKey, pValue) =>
        {
            strBuff += string.Format("Scene : {0}\n", pKey);
            SHUtils.ForToList(pValue, (pInfo) =>
            {
                strBuff += string.Format("\t{0}\n", pInfo);
            });
        });

        string strSavePath = string.Format("{0}/{1}", SHPath.GetPathToAssets(), "RealTimeLoadResource.txt");
        SHUtils.SaveFile(strBuff, strSavePath);
        System.Diagnostics.Process.Start(strSavePath);
    }
    [FuncButton]
    public void ClearLoadResourceList()
    {
        m_dicRealLoadInfo.Clear();
    }
    [FuncButton]
    public int GetProcessID()
    {
        var pProcess = System.Diagnostics.Process.GetCurrentProcess ();
        return pProcess.Id;
    }
    [FuncButton]
    public int GetDebugPort()
	{
		return 56000 + (GetProcessID() % 1000);
	}
    #endregion


    #region 디버그 로그
    // 디버그 : 앱정보 출력
    void DrawAppInformation()
    {
        var pServerInfo = Single.Table.GetTable<JsonServerConfiguration>();
        if (false == pServerInfo.IsLoadTable())
            return;

        GUIStyle pStyle = new GUIStyle(GUI.skin.box);
        pStyle.fontSize = GetRatioW(20);

        GUI.Box(new Rect(0, (Screen.height - GetRatioH(30)), GetRatioW(350), GetRatioH(30)),
            string.Format("{0} : {1} : {2} Scene", Single.Table.GetServiceMode(), GetRuntimePlatform(), Single.Scene.GetCurrentScene()), pStyle);

        GUI.Box(new Rect((Screen.width * 0.5f) - (GetRatioW(120) * 0.5f), (Screen.height - GetRatioH(30)), GetRatioW(120), GetRatioH(30)),
            string.Format("Ver {0}", Single.Table.GetClientVersion()), pStyle);
    }

    // 디버그 : 렌더 프레임 제어
    void ControlRenderFrame()
    {
        //if (true == GUI.Button(new Rect(GetRatioW(150), 0, GetRatioW(150), GetRatioH(50)), string.Format("Up RenderFrame : {0}", GetFrameRate())))
        //    SetFrameRate(GetFrameRate() + 1);
        //if (true == GUI.Button(new Rect(GetRatioW(150), GetRatioH(50), GetRatioW(150), GetRatioH(50)), string.Format("Down RenderFrame : {0}", GetFrameRate())))
        //    SetFrameRate(GetFrameRate() - 1);
    }

    // 디버그 : 게임정보 출력
    IEnumerator PrintGameInfo()
    {
        if (null == m_pDebugText)
            yield break;

        yield return new WaitForSeconds(1.0f);

        Profiler.BeginSample("CheckMemory");

        float fMemory            = Profiler.GetTotalAllocatedMemory() / 1024.0f / 1024.0f;
        m_pDebugText.text        = string.Format("UsedMemory : {0:F2}MB\nFPS : {1:F2}", fMemory, (1.0f / m_fDeltaTime));
        m_pDebugText.fontSize    = GetRatioW(20);
        m_pDebugText.pixelOffset = new Vector2(0.0f, Screen.height * 0.7f);

        Profiler.EndSample();
        StartCoroutine(PrintGameInfo());
    }

    // 디버그 : 배포제한시간 체크
    IEnumerator CheckReleaseTime()
    {
        if (true == IsEditorMode())
            yield break;

        yield return new WaitForSeconds(1.0f);

        if (true == Single.Timer.IsPastTimeToLocal(m_pReleaseTime))
            SHUtils.GameQuit();
        else
            StartCoroutine(CheckReleaseTime());
    }

    // 디버그 : 실시간 로드 리소스 리스트
    public void SetLoadResource(string strInfo)
    {
        if (false == m_dicRealLoadInfo.ContainsKey(Single.Scene.GetActiveScene()))
            m_dicRealLoadInfo.Add(Single.Scene.GetActiveScene(), new List<string>());

        //// 콜스택 남기기
        //strInfo += string.Format("\n< CallStack >\n{0}", SHUtils.GetCallStack());

        m_dicRealLoadInfo[Single.Scene.GetActiveScene()].Add(strInfo);
    }
    #endregion
}
