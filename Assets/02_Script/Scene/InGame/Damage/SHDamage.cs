using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicDamages = System.Collections.Generic.Dictionary<string, SHDamageObject>;
using DicUnits   = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SHMonoWrapper>>;

public partial class SHDamage : SHInGame_Component
{
    #region Members : Damages
    public DicDamages m_dicDelDamages = new DicDamages(); // 제거될 데미지들
    public DicDamages m_dicAddDamages = new DicDamages(); // 추가될 데미지들
    public DicDamages m_dicDamages    = new DicDamages(); // 모든 데미지들
    #endregion
    

    #region Members : ETC
    private bool       m_bIsLockCheckCollision = false;    // 데미지 충돌체크를 하지 않는다.
    private int        m_iAddCount     = 0;                // 데미지 Key를 중복없이 만들어주기 위해...
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        Clear();
    }
    public override void OnFinalize()
    {
        DelAllDamage();
    }
    public override void OnFrameMove()
    {
        OnAddDamage();
        OnDelDamage();
        
        OnUpdateDamage();
    }
    #endregion


    #region System Functions
    void OnAddDamage()
    {
        SHUtils.ForToDic(m_dicAddDamages, (pKey, pValue) =>
        {
            if (true == m_dicDamages.ContainsKey(pKey))
                pValue.SetActive(false);
            else
                m_dicDamages.Add(pKey, pValue);
        });
        m_dicAddDamages.Clear();
    }
    void OnDelDamage()
    {
        SHUtils.ForToDic(m_dicDelDamages, (pKey, pValue) =>
        {
            if (true == m_dicDamages.ContainsKey(pKey))
            {
                ReturnDamage(pValue);
                m_dicDamages.Remove(pKey);
            }

            if (true == m_dicAddDamages.ContainsKey(pKey))
            {
                ReturnDamage(pValue);
                m_dicAddDamages.Remove(pKey);
            }
        });
        m_dicDelDamages.Clear();
    }
    void OnUpdateDamage()
    {
        SHUtils.ForToDic(m_dicDamages, (pKey, pValue) =>
        {
            pValue.OnFrameMove();
            CheckCollision(pValue);
        });
    }
    #endregion


    #region Interface Functions
    public SHDamageObject AddDamage(string strPrefabName, SHDamageParam pParam)
    {
        if (null == pParam)
        {
            Debug.LogErrorFormat("SHDamage::AddDamage - Param Is Null!!");
            return null;
        }

        var pDamage = CreateDamage(strPrefabName);
        if (null == pDamage)
            return null;
        
        var strID = GetNewDamageID(strPrefabName);
        pParam.AddEventToDelete(OnEventToDeleteDamage);
        pDamage.OnInitialize(strID, pParam);

        if (false == pDamage.m_pInfo.m_bIsTraceToCreator)
            pDamage.SetLocalScale(pDamage.m_vStartScale * SHHard.m_fUnitScale);

        if (false == m_dicAddDamages.ContainsKey(strID))
            m_dicAddDamages.Add(strID, pDamage);
        else
            m_dicAddDamages[strID] = pDamage;

        return pDamage;
    }
    public void DelDamage(SHDamageObject pDamage)
    {
        if (null == pDamage)
            return;

        if (true == m_dicDelDamages.ContainsKey(pDamage.m_pInfo.m_strID))
            return;

        m_dicDelDamages.Add(pDamage.m_pInfo.m_strID, pDamage);
    }
    public void DelAllDamage()
    {
        if (true == SHApplicationInfo.m_bIsAppQuit)
            return;

        SHUtils.ForToDic(m_dicDamages, (pKey, pValue) =>
        {
            ReturnDamage(pValue);
        });
        m_dicDamages.Clear();

        SHUtils.ForToDic(m_dicAddDamages, (pKey, pValue) =>
        {
            ReturnDamage(pValue);
        });
        m_dicAddDamages.Clear();

        m_dicDelDamages.Clear();
    }
    public int GetDamageCount()
    {
        return m_dicDamages.Count;
    }
    public Vector3 GetDamagePosition(string strID)
    {
        if (false == m_dicDamages.ContainsKey(strID))
            return Vector3.zero;

        return m_dicDamages[strID].GetLocalPosition();
    }
    public void SetLockCheckCollision(bool bIsLock)
    {
        m_bIsLockCheckCollision = bIsLock;
    }
    public void Clear()
    {
        OnAddDamage();
        m_dicDelDamages = new DicDamages(m_dicDamages);
        OnDelDamage();
    }
    #endregion


    #region Event Handler
    public void OnEventToDeleteDamage(SHDamageObject pDamage)
    {
        DelDamage(pDamage);
    }
    #endregion
}
