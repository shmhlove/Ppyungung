using UnityEngine;
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
    
    // 기타(디버그) : FPS 출력용 델타타임
    [ReadOnlyField]
    [SerializeField] private float              m_fDeltaTime        = 0.0f;

    // 기타(디버그) : 로드 시도된 리소스 리스트
    [HideInInspector] private DicRealLoadInfo   m_dicRealLoadInfo   = new DicRealLoadInfo();

    // 기타 : 앱 종료 여부
    [HideInInspector] public static bool        m_bIsAppQuit        = false;
    #endregion


    #region System Functions
    void OnApplicationQuit()
    {
        m_bIsAppQuit = true;
    }
    void OnApplicationPause(bool bIsPause)
    {
    }
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
    void OnDisconnectedFromServer(NetworkDisconnection pInfo)
    {

    }
    public override void Start()
    {
        base.Start();

        SetDontDestroy();
        
        // 언어설정
        SetLocalization();

        // 어플리케이션 정보설정
        SetApplicationInfo();

        // 디버그 기능
        CheckReleaseTime();

		// 디바이스 정보출력
		PrintDeviceInfo();
    }
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
				m_pEventToOK  = SetApplicationQuit,
			});
		}
    }
    void OnGUI()
    {
        DrawAppInformation();
    }
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    #endregion


    #region Interface Functions
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
    public void SetApplicationInfo()
    {
        var pClientInfo = Single.Table.GetTable<JsonClientConfiguration>();
        SetVSync(pClientInfo.GetVSyncCount());
        SetFrameRate(SHHard.m_iFrameRate);//pClientInfo.GetFrameRate());
        SetCacheInfo(pClientInfo.GetCacheSize(), 30);
        SetSleepMode();
        SetCrittercism();

		UnityEngine.Debug.LogFormat ("ProcessID : {0}", GetProcessID());
		UnityEngine.Debug.LogFormat ("DebugPort : {0}", GetDebugPort());
    }
    public static void SetApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        var pThreads = Process.GetCurrentProcess().Threads;
        foreach(var pThread in pThreads)
        {
            pThread.Dispose();
        }

        // Process.GetCurrentProcess().Kill();
        Application.Quit();
#endif
    }
    public bool IsLandscape()
    {
        return ((true == IsEditorMode()) ||
                (Screen.orientation == ScreenOrientation.Landscape) ||
                (Screen.orientation == ScreenOrientation.LandscapeLeft) ||
                (Screen.orientation == ScreenOrientation.LandscapeRight));
    }
    public bool IsEditorMode()
    {
        return ((Application.platform == RuntimePlatform.WindowsEditor) ||
                (Application.platform == RuntimePlatform.OSXEditor));
    }
    public bool IsDevelopment()
    {
        return UnityEngine.Debug.isDebugBuild;
    }
    public RuntimePlatform GetRuntimePlatform()
    {
        return Application.platform;
    }
    public void CheckServiceState(Action<eServiceState> pCallback)
    {
        Single.Table.DownloadServerConfiguration(() =>
        {
            if (null == pCallback)
                return;

            pCallback(GetServiceState());
        });
    }
    public eServiceState GetServiceState()
    {
        var eState = Single.Table.GetServiceState();
        if (eServiceState.None == eState)
            return eServiceState.ConnectMarket;
        else
            return eState;
    }
    public string GetAppName()
    {
        return Application.bundleIdentifier.Split('.')[2];
    }
    public int GetScreenWidth()
    {
        return Screen.width;
    }
    public int GetScreenHeight()
    {
        return Screen.height;
    }
    #endregion


    #region Utility Functions
    void SetVSync(int iCount)
    {
        QualitySettings.vSyncCount = iCount;
    }
    public void SetFrameRate(int iFrame)
    {
        Application.targetFrameRate = iFrame;
    }
    void SetSleepMode()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void SetCacheInfo(long lSizeMB, int iExpirationMonth)
    {
        Caching.maximumAvailableDiskSpace   = lSizeMB * 1024 * 1024;
        Caching.expirationDelay             = 60 * 60 * 24 * iExpirationMonth;
    }
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
    int GetRatioW(int iValue)
    {
        return (int)(iValue * (GetScreenWidth() / 1280.0f));
    }
    int GetRatioH(int iValue)
    {
        return (int)(iValue * (GetScreenHeight() / 720.0f));
    }
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


    #region Coroutine Functions
    IEnumerator CheckReleaseTime()
    {
        if (true == IsEditorMode())
            yield break;

        yield return new WaitForSeconds(1.0f);

        if (true == Single.Timer.IsPastTimeToLocal(m_pReleaseTime))
            SHApplicationInfo.SetApplicationQuit();
        else
            StartCoroutine(CheckReleaseTime());
    }
    #endregion

    
    #region 에디터 테스트
    [FuncButton] public void SaveLoadResourceList()
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
    [FuncButton] public void ClearLoadResourceList()
    {
        m_dicRealLoadInfo.Clear();
    }
    [FuncButton] public int GetProcessID()
    {
        var pProcess = System.Diagnostics.Process.GetCurrentProcess ();
        return pProcess.Id;
    }
    [FuncButton] public int GetDebugPort()
	{
		return 56000 + (GetProcessID() % 1000);
	}
    #endregion


    #region 디버그 기능
    void DrawAppInformation()
    {
        var pServerInfo = Single.Table.GetTable<JsonServerConfiguration>();
        if (false == pServerInfo.IsLoadTable())
            return;

        GUIStyle pStyle = new GUIStyle(GUI.skin.box);
        pStyle.fontSize = GetRatioW(20);

        GUI.Box(new Rect(0, (Screen.height - GetRatioH(30)), GetRatioW(350), GetRatioH(30)),
            string.Format("{0} - {1} : {2} Scene", Single.Table.GetServiceMode(), GetRuntimePlatform(), Single.Scene.GetCurrentScene()), pStyle);

        GUI.Box(new Rect((Screen.width * 0.5f) - (GetRatioW(120) * 0.5f), (Screen.height - GetRatioH(30)), GetRatioW(120), GetRatioH(30)),
            string.Format("Ver {0}", Single.Table.GetClientVersion()), pStyle);
    }
    public void SetLoadResource(string strInfo)
    {
        if (false == m_dicRealLoadInfo.ContainsKey(Single.Scene.GetActiveScene()))
            m_dicRealLoadInfo.Add(Single.Scene.GetActiveScene(), new List<string>());

        //// 콜스택 남기기
        //strInfo += string.Format("\n< CallStack >\n{0}", SHUtils.GetCallStack());

        m_dicRealLoadInfo[Single.Scene.GetActiveScene()].Add(strInfo);
    }
	[FuncButton] public void PrintDeviceInfo()
    {
        string strBuff = "<color=yellow>==================Device Information==================</color>\n";
        {
            strBuff += string.Format("Screen.currentResolution.height : {0}\n", Screen.currentResolution.height);
            strBuff += string.Format("Screen.currentResolution.width : {0}\n", Screen.currentResolution.width);
            strBuff += string.Format("Screen.dpi : {0}\n", Screen.dpi);
            strBuff += string.Format("Screen.fullScreen : {0}\n", Screen.fullScreen);
            strBuff += string.Format("Screen.sleepTimeout : {0}\n", Screen.sleepTimeout);
            strBuff += string.Format("Screen.width : {0}\n", Screen.width);
            strBuff += string.Format("Screen.height : {0}\n", Screen.height);
            strBuff += string.Format("deviceModel : {0}\n", SystemInfo.deviceModel);
            strBuff += string.Format("deviceName : {0}\n", SystemInfo.deviceName);
            strBuff += string.Format("deviceType : {0}\n", SystemInfo.deviceType);
#if UNITY_EDITOR
            strBuff += string.Format("deviceUniqueIdentifier : {0}\n", SystemInfo.deviceUniqueIdentifier);
#endif
            strBuff += string.Format("graphicsDeviceID : {0}\n", SystemInfo.graphicsDeviceID);
            strBuff += string.Format("graphicsDeviceName : {0}\n", SystemInfo.graphicsDeviceName);
            strBuff += string.Format("graphicsDeviceType : {0}\n", SystemInfo.graphicsDeviceType);
            strBuff += string.Format("graphicsDeviceVendor : {0}\n", SystemInfo.graphicsDeviceVendor);
            strBuff += string.Format("graphicsDeviceVendorID : {0}\n", SystemInfo.graphicsDeviceVendorID);
            strBuff += string.Format("graphicsDeviceVersion : {0}\n", SystemInfo.graphicsDeviceVersion);
            strBuff += string.Format("graphicsMemorySize : {0}\n", SystemInfo.graphicsMemorySize);
            strBuff += string.Format("graphicsMultiThreaded : {0}\n", SystemInfo.graphicsMultiThreaded);
            strBuff += string.Format("graphicsShaderLevel : {0}\n", SystemInfo.graphicsShaderLevel);
            strBuff += string.Format("maxTextureSize : {0}\n", SystemInfo.maxTextureSize);
            strBuff += string.Format("npotSupport : {0}\n", SystemInfo.npotSupport);
            strBuff += string.Format("operatingSystem : {0}\n", SystemInfo.operatingSystem);
            strBuff += string.Format("processorCount : {0}\n", SystemInfo.processorCount);
            strBuff += string.Format("processorFrequency : {0}\n", SystemInfo.processorFrequency);
            strBuff += string.Format("processorType : {0}\n", SystemInfo.processorType);
            strBuff += string.Format("supportedRenderTargetCount : {0}\n", SystemInfo.supportedRenderTargetCount);
            strBuff += string.Format("supports3DTextures : {0}\n", SystemInfo.supports3DTextures);
            strBuff += string.Format("supportsAccelerometer : {0}\n", SystemInfo.supportsAccelerometer);
            strBuff += string.Format("supportsComputeShaders : {0}\n", SystemInfo.supportsComputeShaders);
            strBuff += string.Format("supportsGyroscope : {0}\n", SystemInfo.supportsGyroscope);
            strBuff += string.Format("supportsImageEffects : {0}\n", SystemInfo.supportsImageEffects);
            strBuff += string.Format("supportsInstancing : {0}\n", SystemInfo.supportsInstancing);
            strBuff += string.Format("supportsLocationService : {0}\n", SystemInfo.supportsLocationService);
            strBuff += string.Format("supportsRawShadowDepthSampling : {0}\n", SystemInfo.supportsRawShadowDepthSampling);
            strBuff += string.Format("supportsRenderTextures : {0}\n", SystemInfo.supportsRenderTextures);
            strBuff += string.Format("supportsRenderToCubemap : {0}\n", SystemInfo.supportsRenderToCubemap);
            strBuff += string.Format("supportsShadows : {0}\n", SystemInfo.supportsShadows);
            strBuff += string.Format("supportsSparseTextures : {0}\n", SystemInfo.supportsSparseTextures);
            strBuff += string.Format("supportsStencil : {0}\n", SystemInfo.supportsStencil);
            strBuff += string.Format("supportsVibration : {0}\n", SystemInfo.supportsVibration);
            strBuff += string.Format("systemMemorySize : {0}\n", SystemInfo.systemMemorySize);
        }

        UnityEngine.Debug.Log(strBuff);
    }
    #endregion
}
