using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicDamages = System.Collections.Generic.Dictionary<string, SHDamageObject>;
using DicUnits   = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<SHMonoWrapper>>;

public partial class SHDamage : SHBaseEngine
{
    #region Members : Damages
    private DicDamages m_dicDelDamages = new DicDamages(); // 제거될 데미지들
    private DicDamages m_dicAddDamages = new DicDamages(); // 추가될 데미지들
    private DicDamages m_dicDamages    = new DicDamages(); // 모든 데미지들
    #endregion


    #region Members : Units
    private DicUnits   m_dicDelUnits   = new DicUnits();   // 제거될 유닛들
    private DicUnits   m_dicAddUnits   = new DicUnits();   // 추가될 유닛들
    private DicUnits   m_dicUnits      = new DicUnits();   // 충돌체크에 포함시킬 유닛들 ( Tag로 그룹관리 )
    #endregion


    #region Members : ETC
    private int        m_iAddCount     = 0;                // 데미지 Key를 중복없이 만들어주기 위해...
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        SHUtils.ForToDic(m_dicDamages, (pKey, pValue) =>
        {
            ReturnDamage(pValue);
        });
        m_dicDamages.Clear();
    }
    public override void OnFrameMove()
    {
        if (true == m_bIsPause)
            return;

        OnAddUnits();
        OnDelUnits();
        OnAddDamage();
        OnDelDamage();

        OnUpdateDamage();
    }
    #endregion


    #region System Functions
    void OnAddUnits()
    {
        SHUtils.ForToDic(m_dicAddUnits, (pKey, pValue) =>
        {
            if (false == m_dicUnits.ContainsKey(pKey))
                m_dicUnits.Add(pKey, new List<SHMonoWrapper>());

            SHUtils.ForToList(pValue, (pUnit) =>
            {
                if (true == m_dicUnits[pKey].Contains(pUnit))
                    return;
                
                m_dicUnits[pKey].Add(pUnit);
            });
        });
        m_dicAddUnits.Clear();
    }
    void OnDelUnits()
    {
        SHUtils.ForToDic(m_dicDelUnits, (pKey, pValue) =>
        {
            if (false == m_dicUnits.ContainsKey(pKey))
                return;

            SHUtils.ForToList(pValue, (pUnit) =>
            { m_dicUnits[pKey].Remove(pUnit); });
        });
        m_dicDelUnits.Clear();
    }
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
            if (false == m_dicDamages.ContainsKey(pKey))
                return;

            ReturnDamage(pValue);
            m_dicDamages.Remove(pKey);
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
    public SHDamageObject AddDamage(string strPrefabName, SHAddDamageParam pParam)
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
        pDamage.SetActive(true);
        pDamage.OnInitialize(strID, pParam);

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
    public void AddUnit(SHMonoWrapper pUnit)
    {
        if (null == pUnit)
            return;

        if (false == m_dicAddUnits.ContainsKey(pUnit.tag))
            m_dicAddUnits.Add(pUnit.tag, new List<SHMonoWrapper>());

        if (true == m_dicAddUnits[pUnit.tag].Contains(pUnit))
            return;

        m_dicAddUnits[pUnit.tag].Add(pUnit);
    }
    public void DelUnit(SHMonoWrapper pUnit)
    {
        if (null == pUnit)
            return;

        if (false == m_dicDelUnits.ContainsKey(pUnit.tag))
            m_dicAddUnits.Add(pUnit.tag, new List<SHMonoWrapper>());

        m_dicDelUnits[pUnit.tag].Add(pUnit);
    }
    #endregion


    #region Event Handler
    public void OnEventToDeleteDamage(SHDamageObject pDamage)
    {
        DelDamage(pDamage);
    }
    #endregion
}
