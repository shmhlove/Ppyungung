﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicRoots  = System.Collections.Generic.Dictionary<int, UnityEngine.Transform>;
using DicObject = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SHObjectInfo>>;

public enum ePoolReturnType
{
    None,           // 반환 : 반환안함
    Disable,        // 반환 : 오브젝트의 Active가 꺼질때 반환
    ChangeScene,    // 반환 : 씬이 변경될때 반환
}

public enum ePoolDestroyType
{
    None,           // 제거 : 제거안함
    Return,         // 제거 : 풀에 반환될때 제거
    ChangeScene,    // 제거 : 씬이 변경될때 제거
}

public class SHObjectInfo
{
    public ePoolReturnType  m_eReturnType    = ePoolReturnType.None;
    public ePoolDestroyType m_eDestroyType   = ePoolDestroyType.None;
    public GameObject       m_pObject        = null;

    public string           m_strPrefabName  = string.Empty;
    public Vector3          m_vStartPosition = Vector3.zero;
    public Quaternion       m_qStartRotate   = Quaternion.identity;
    public Vector3          m_vStartScale    = Vector3.zero;
    
    public SHObjectInfo() { }
    public SHObjectInfo(ePoolReturnType eReturnType, ePoolDestroyType eDestoryType, GameObject pObject, string strPrefabName)
    {
        m_eReturnType   = eReturnType;
        m_eDestroyType  = eDestoryType;
        m_pObject       = pObject;
        m_strPrefabName = strPrefabName;

        if (null != m_pObject)
        {
            m_vStartPosition = m_pObject.transform.localPosition;
            m_qStartRotate   = m_pObject.transform.localRotation;
            m_vStartScale    = m_pObject.transform.localScale;
        }
    }

    public void SetParent(Transform pParent)
    {
        if (null == m_pObject)
            return;

        var pLayer = m_pObject.layer;
        m_pObject.transform.SetParent(pParent);
        m_pObject.layer = pLayer;
    }
    public void SetActive(bool bIsActive)
    {
        if (null == m_pObject)
            return;

        if (bIsActive == m_pObject.activeInHierarchy)
            return;

        m_pObject.SetActive(bIsActive);
    }
    public void SetStartTransform()
    {
        m_pObject.transform.localPosition = m_vStartPosition;
        m_pObject.transform.localRotation = m_qStartRotate;
        m_pObject.transform.localScale    = m_vStartScale;
    }
    public bool IsActive()
    {
        if (null == m_pObject)
            return false;

        return m_pObject.activeInHierarchy;
    }
    public string GetName()
    {
        return m_strPrefabName;
    }
    public bool IsSameObject(GameObject pObject)
    {
        if (null == pObject)
            return false;
        if (null == m_pObject)
            return false;

        return (pObject == m_pObject);
    }
    public void DestroyObject()
    {
        m_eReturnType  = ePoolReturnType.None;
        m_eDestroyType = ePoolDestroyType.None;
        if (null == m_pObject)
            return;

        SHGameObject.DestoryObject(m_pObject);
        m_pObject        = null;
        m_strPrefabName  = string.Empty;
    }
}

public class SHObjectPool : SHSingleton<SHObjectPool>
{
    #region Members : ObjectPool
    DicRoots m_dicRoots      = new DicRoots();
    #endregion


    #region Members : ObjectPool
    DicObject m_dicActives   = new DicObject();
    DicObject m_dicInactives = new DicObject();
    #endregion


    #region Members : Constants
    private readonly int CHECK_DELAY_FOR_ACTIVE = 5;
    #endregion


    #region virtual Functions
    public override void OnInitialize()
    {
        SetDontDestroy();

        ClearAll();
        Single.Scene.AddEventToChangeScene(OnEventToChangeScene);

        StartCoroutine(CoroutineCheckAutoProcess());
    }
    public override void OnFinalize()
    {
        StopAllCoroutines();
    }
    #endregion


    #region Interface Functions
    public T Get<T>(
        string           strName, 
        bool             bIsActive    = false,
        ePoolReturnType  eReturnType  = ePoolReturnType.Disable, 
        ePoolDestroyType eDestroyType = ePoolDestroyType.ChangeScene) where T : Component
    {
        return SHGameObject.GetComponent<T>(Get(strName, bIsActive, eReturnType, eDestroyType));
    }
    public GameObject Get(
        string           strName, 
        bool             bIsActive    = false,
        ePoolReturnType  eReturnType  = ePoolReturnType.Disable, 
        ePoolDestroyType eDestroyType = ePoolDestroyType.ChangeScene)
    {
        var pObject = GetInactiveObject(eReturnType, eDestroyType, strName);
        if (null == pObject)
            return null;

        SetActiveObject(strName, pObject, bIsActive);
        return pObject.m_pObject;
    }
    public void Return(GameObject pObject)
    {
        var pObjectInfo = GetObjectInfo(pObject);
        if (null == pObjectInfo)
            return;

        SetReturnObject(pObjectInfo.GetName(), pObjectInfo);
    }
    public void SetStartTransform(GameObject pObject)
    {
        var pObjectInfo = GetObjectInfo(pObject);
        if (null == pObjectInfo)
            return;

        pObjectInfo.SetStartTransform();
    }
    public int GetAllObjectCount()
    {
        int iCount = 0;
        SHUtils.ForToDic(m_dicActives,   (pKey, pValue) => iCount += pValue.Count);
        SHUtils.ForToDic(m_dicInactives, (pKey, pValue) => iCount += pValue.Count);
        return iCount;
    }
    #endregion


    #region Utility : Get And Return
    private void SetActiveObject(string strName, SHObjectInfo pObjectInfo, bool bIsActive)
    {
        CheckDictionary(m_dicActives,   strName);
        CheckDictionary(m_dicInactives, strName);

        m_dicActives[strName].Add(pObjectInfo);
        m_dicInactives[strName].Remove(pObjectInfo);
        
        pObjectInfo.SetParent(GetRoot(pObjectInfo.m_pObject.layer));
        pObjectInfo.SetStartTransform();
        pObjectInfo.SetActive(bIsActive);
    }
    private void SetReturnObject(string strName, SHObjectInfo pObjectInfo)
    {
        CheckDictionary(m_dicActives,   strName);
        CheckDictionary(m_dicInactives, strName);

        if ((null == pObjectInfo.m_pObject) || 
            (ePoolDestroyType.Return == pObjectInfo.m_eDestroyType))
        {
            SetDestroyObject(strName, pObjectInfo);
        }
        else
        {
            m_dicActives[strName].Remove(pObjectInfo);
            m_dicInactives[strName].Add(pObjectInfo);

            pObjectInfo.SetParent(GetRoot(pObjectInfo.m_pObject.layer));
            pObjectInfo.SetActive(false);
        }
    }
    private void SetDestroyObject(string strName, SHObjectInfo pObjectInfo)
    {
        CheckDictionary(m_dicActives,   strName);
        CheckDictionary(m_dicInactives, strName);

        m_dicActives[strName].Remove(pObjectInfo);
        m_dicInactives[strName].Remove(pObjectInfo);

        pObjectInfo.DestroyObject();
    }
    #endregion


    #region Utility : Helpper
    private List<SHObjectInfo> GetInactiveObjects(string strName)
    {
        if (false == m_dicInactives.ContainsKey(strName))
            return new List<SHObjectInfo>();
        else
            return m_dicInactives[strName];
    }
    private SHObjectInfo GetInactiveObject(ePoolReturnType eReturnType, ePoolDestroyType eDestroyType, string strName)
    {
        var pObjects = GetInactiveObjects(strName);
        if (0 == pObjects.Count)
        {
            var pObject = Single.Resource.GetGameObject(strName);
            if (null == pObject)
                return null;

            return new SHObjectInfo(
                eReturnType, eDestroyType, pObject, strName);
        }
        else
        {
            pObjects[0].m_eReturnType  = eReturnType;
            pObjects[0].m_eDestroyType = eDestroyType;
            return pObjects[0];
        }
    }
    private SHObjectInfo GetObjectInfo(GameObject pObject)
    {
        if (null == pObject)
            return null;
        
        var strName     = pObject.name;
        if (true == m_dicActives.ContainsKey(strName))
        {
            var pObjectInfo = m_dicActives[strName].Find((pItem) =>
                              { return pItem.IsSameObject(pObject); });
            if (null != pObjectInfo)
                return pObjectInfo;
        }

        if (true == m_dicInactives.ContainsKey(strName))
        {
            var pObjectInfo = m_dicInactives[strName].Find((pItem) =>
                              { return pItem.IsSameObject(pObject); });
            if (null != pObjectInfo)
                return pObjectInfo;
        }

        return null;
    }
    private void ClearAll()
    {
        SHUtils.ForToDic(m_dicActives, (pKey, pValue) =>
        {
            SHUtils.ForToList(pValue, (pObject) => pObject.DestroyObject());
        });
        SHUtils.ForToDic(m_dicInactives, (pKey, pValue) =>
        {
            SHUtils.ForToList(pValue, (pObject) => pObject.DestroyObject());
        });

        m_dicActives.Clear();
        m_dicInactives.Clear();
    }
    private void CheckDictionary(DicObject dicObjects, string strName)
    {
        if (true == dicObjects.ContainsKey(strName))
            return;

        dicObjects.Add(strName, new List<SHObjectInfo>());
    }
    private void ForItemActives(Action<SHObjectInfo> pCallback)
    {
        if (null == pCallback)
            return;
        
        SHUtils.ForToDic(m_dicActives, (pKey, pValue) =>
        {
            SHUtils.ForToList(pValue, (pItem) => pCallback(pItem));
        });
    }
    private void ForItemInactives(Action<SHObjectInfo> pCallback)
    {
        if (null == pCallback)
            return;
        
        SHUtils.ForToDic(m_dicInactives, (pKey, pValue) =>
        {
            SHUtils.ForToList(pValue, (pItem) => pCallback(pItem));
        });
    }
    #endregion


    #region Utility : Auto Return Or Destroy
    private void CheckAutoReturnObject(bool bIsChangeScene)
    {
        var pReturns = new List<SHObjectInfo>();
        ForItemActives((pItem) =>
        {
            switch (pItem.m_eReturnType)
            {
                case ePoolReturnType.Disable:
                    if (false == pItem.IsActive())
                        pReturns.Add(pItem);
                    break;
                case ePoolReturnType.ChangeScene:
                    if (true == bIsChangeScene)
                        pReturns.Add(pItem);
                    break;
            }
        });

        SHUtils.ForToList(pReturns, (pItem) =>
        {
            SetReturnObject(pItem.GetName(), pItem);
        });
    }
    private void CheckAutoDestroyObject(bool bIsChangeScene)
    {
        var pDestroys = new List<SHObjectInfo>();
        ForItemActives((pItem) =>
        {
            switch (pItem.m_eDestroyType)
            {
                case ePoolDestroyType.ChangeScene:
                    if (true == bIsChangeScene)
                        pDestroys.Add(pItem);
                    break;
            }
        });

        ForItemInactives((pItem) =>
        {
            switch (pItem.m_eDestroyType)
            {
                case ePoolDestroyType.ChangeScene:
                    if (true == bIsChangeScene)
                        pDestroys.Add(pItem);
                    break;
            }
        });

        SHUtils.ForToList(pDestroys, (pItem) =>
        {
            SetDestroyObject(pItem.GetName(), pItem);
        });
    }
    #endregion


    #region Utility Functions
    Transform GetRoot(int iLayer)
    {
        if (false == m_dicRoots.ContainsKey(iLayer))
        {
            var pRoot = SHGameObject.CreateEmptyObject(string.Format("SHObjectPool_{0}", iLayer));
            pRoot.layer = iLayer;
            pRoot.transform.SetParent(transform);
            m_dicRoots.Add(iLayer, pRoot.transform);
        }

        return m_dicRoots[iLayer];
    }
    IEnumerator CoroutineCheckAutoProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(CHECK_DELAY_FOR_ACTIVE);

            CheckAutoReturnObject(false);
            CheckAutoDestroyObject(false);
        }
    }
    #endregion


    #region Event Handler
    public void OnEventToChangeScene(eSceneType eCurrentScene, eSceneType eNextScene)
    {
        CheckAutoReturnObject(true);
        CheckAutoDestroyObject(true);
    }
    #endregion
}
