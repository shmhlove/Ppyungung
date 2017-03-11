using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicPanels = System.Collections.Generic.Dictionary<string, SHUIBasePanel>;

public partial class SHUIManager : SHSingleton<SHUIManager>
{
    #region Members
    [ReadOnlyField][SerializeField] private DicPanels m_dicPanels = new DicPanels();
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        SetDontDestroy();
        Single.Scene.AddEventToChangeScene(OnEventToChangeScene);
        SHGameObject.SetParent(Single.Resource.GetGameObject("UIRoot_Global"), gameObject);
    }
    public override void OnFinalize()
    {
        
    }
    #endregion


    #region System Functions
    #endregion


    #region Interface Functions
    public void AddPanel(SHUIBasePanel pPanel, bool bIsActive)
    {
        if (null == pPanel)
        {
            Debug.LogError("AddPanel() - Panel Is null!!");
            return;
        }

        if (true == m_dicPanels.ContainsKey(pPanel.name))
            m_dicPanels[pPanel.name] = pPanel;
        else
            m_dicPanels.Add(pPanel.name, pPanel);

        pPanel.Initialize(bIsActive);
    }
    public T GetPanel<T>(string strPanelName) where T : SHUIBasePanel
    {
        return GetPanel(strPanelName) as T;
    }
    public SHUIBasePanel Show(string strName, params object[] pArgs)
    {
        var pPanel = GetPanel(strName);
        if (null == pPanel)
        {
            Debug.LogErrorFormat("Show() - No Exist Panel(Name : {0})", strName);
            return null;
        }
        
        pPanel.Show(pArgs);
        return pPanel;
    }
    public SHUIBasePanel Close(string strName)
    {
        var pPanel = GetPanel(strName);
        if (null == pPanel)
        {
            Debug.LogErrorFormat("Close() - No Exist Panel(Name : {0})", strName);
            return null;
        }

        pPanel.Close();
        return pPanel;
    }
    public bool IsExistPanel(string strName)
    {
        return (null != GetPanel(strName));
    }
    public Transform GetRootToGlobal()
    {
        return SHUIRoot_Global.GetRoot();
    }
    public Transform GetRootToScene()
    {
        return SHUIRoot_Scene.GetRoot();
    }
    #endregion


    #region Utility Functions
    SHUIBasePanel GetPanel(string strName)
    {
        if (false == m_dicPanels.ContainsKey(strName))
        {
            AddPanel(Single.ObjectPool.Get<SHUIBasePanel>(strName,
                false, ePoolReturnType.None, ePoolDestroyType.None), false);
        }

        if (false == m_dicPanels.ContainsKey(strName))
        {
            return null;
        }
        
        return m_dicPanels[strName];
    }
    void DestoryPanel(Dictionary<string, SHUIBasePanel> dicPanels)
    {
        if (null == dicPanels)
            return;

        SHUtils.ForToDic(new DicPanels(dicPanels), (pKey, pValue) =>
        {
            DestroyPanel(pValue);
            m_dicPanels.Remove(pKey);
        });
    }
    void DestroyPanel(SHUIBasePanel pPanel)
    {
        if (null == pPanel)
            return;

        pPanel.Destory();
    }
    #endregion


    #region Event Handler
    public void OnEventToChangeScene(eSceneType eCurrentScene, eSceneType eNextScene)
    {
        var pDestroyPanels = new DicPanels();
        SHUtils.ForToDic(m_dicPanels, (pKey, pValue) =>
        {
            if (eObjectDestoryType.ChangeScene != pValue.m_eDestroyType)
                return;

            pDestroyPanels.Add(pKey, pValue);
        });
        
        DestoryPanel(pDestroyPanels);
    }
    #endregion


    #region Temp Functions
    public void ShowNotice(string strTitle, string strMessage)
    {
        Show("Panel_Notice", new NoticeUI_Param()
        {
            m_eButtonType   = eNoticeButton.One,
            m_eIconType     = eNoticeIcon.Information,
            m_strTitle      = strTitle,
            m_strMessage    = strMessage,
        });
    }
    public void ShowNotice_TwoBtn(string strTitle, string strMessage, Action pOk, Action pCancel)
    {
        Show("Panel_Notice", new NoticeUI_Param()
        {
            m_eButtonType    = eNoticeButton.Two,
            m_eIconType      = eNoticeIcon.Information,
            m_strTitle       = strTitle,
            m_strMessage     = strMessage,
            m_pEventToOK     = pOk,
            m_pEventToCancel = pCancel,
        });
    }
    public void ShowNotice_NoMake()
    {
        ShowNotice("알림", "업데이트 예정입니다!");
    }
    #endregion
}
