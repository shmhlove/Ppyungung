using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHBaseEngine
{
    #region Members : Info
    private SHUIWidget_Stick m_pStick = null;
    #endregion


    #region System Functions
    #endregion


    #region Interface Functions
    public void StartStick()
    {
        ClearStick();
        CreateStick();
    }
    public void Stop()
    {
        ClearStick();
    }
    #endregion


    #region Utility Functions
    private SHUIWidget_Stick CreateStick()
    {
        m_pStick = Single.ObjectPool.Get<SHUIWidget_Stick>(
            SHHard.GetStickName(GetStickType()));
        if (null == m_pStick)
            return null;
        
        SHGameObject.SetParent(m_pStick.transform, Single.UI.GetRootToScene());
        Single.ObjectPool.SetStartTransform(m_pStick.gameObject);
        m_pStick.SetActive(true);
        m_pStick.Initialize(GetStickType());

        return m_pStick;
    }
    void ClearStick()
    {
        if (null == m_pStick)
            return;

        Single.ObjectPool.Return(m_pStick.gameObject);
        m_pStick = null;
    }
    eStickType GetStickType()
    {
        return Single.Inventory.GetEnableSticksForDic();
    }
    #endregion


    #region Event Handler
    public void OnEventToTouch(Action pEventToPass)
    {
        if (null == m_pStick)
            return;

        m_pStick.SetActive(false);
        m_pStick.SetActive(true);
        m_pStick.Shoot(pEventToPass);
        SHCoroutine.Instance.WaitTime(() => CreateStick(), m_pStick.m_fReCreateTime);
        m_pStick = null;
    }
    #endregion
}