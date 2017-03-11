﻿using UnityEngine;
using System;
using System.Collections;

public class SHUIBasePanel : SHMonoWrapper
{
    #region Members
    [Header("BaseInfo")]
    [SerializeField] public  eObjectDestoryType  m_eDestroyType = eObjectDestoryType.ChangeScene;
    [SerializeField] public  bool                m_bStartActive = true;
    [SerializeField] private GameObject          m_pAnimRoot    = null;
    [SerializeField] private AnimationClip       m_pAnimToOpen  = null;
    [SerializeField] private AnimationClip       m_pAnimToClose = null;
    #endregion


    #region System Functions
    #endregion


    #region Virtual Functions
    public virtual void OnBeforeShow(params object[] pArgs) { }
    public virtual void OnAfterShow(params object[] pArgs) { }
    public virtual void OnBeforeClose() { }
    public virtual void OnAfterClose() { }
    #endregion


    #region Interface Functions
    public void Initialize(bool bIsActive)
    {
        if (eObjectDestoryType.Never == m_eDestroyType)
            SHGameObject.SetParent(transform, Single.UI.GetRootToGlobal());
        else
            SHGameObject.SetParent(transform, Single.UI.GetRootToScene());

        SetLocalScale(Vector3.one);
        SetActive(bIsActive);
    }
    public void Show(params object[] pArgs)
    {
        Initialize(true);
        OnBeforeShow(pArgs);
        PlayAnimation(m_pAnimToOpen, ()=> 
        {
            OnAfterShow(pArgs);
        });
    }
    public void Close()
    {
        OnBeforeClose();
        PlayAnimation(m_pAnimToClose, ()=> 
        {
            OnAfterClose();
            SetActive(false);
        });
    }
    #endregion


    #region Utility Functions
    protected void PlayAnimation(AnimationClip pClip, Action pEndCallback)
    {
        PlayAnim(eDirection.Front, m_pAnimRoot, pClip, pEndCallback);
    }
    #endregion


    #region Event Handler
    public virtual void OnClickToClose()
    {
        Close();
    }
    #endregion
}
