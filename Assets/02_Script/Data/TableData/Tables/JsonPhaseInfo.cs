using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class SHPhaseMonsterGenInfo
{
    public string m_strName = string.Empty;
    public float m_fWeight  = 0.0f;
}

public class SHPhaseInfo
{
    public int                         m_iPhaseID       = 0;
    public int                         m_iPhaseCount    = 0;
    public float                       m_fMonGenDaly    = 0.0f;
    public int                         m_iMonMaxGen     = 0;
    public int                         m_iMonMaxCount   = 0;
    public List<SHPhaseMonsterGenInfo> m_pMonsterGenInfo = new List<SHPhaseMonsterGenInfo>();

    public List<string> GetMonsterList()
    {
        var pMonsters = new List<string>();
        SHUtils.ForToList(m_pMonsterGenInfo, (pInfo) => pMonsters.Add(pInfo.m_strName));
        return pMonsters;
    }
    public List<float> GetWeightList()
    {
        var pWeights = new List<float>();
        SHUtils.ForToList(m_pMonsterGenInfo, (pInfo) => pWeights.Add(pInfo.m_fWeight));
        return pWeights;
    }
}

public class JsonPhaseInfo : SHBaseTable
{
    #region Members : Datas
    public Dictionary<int, SHPhaseInfo> m_dicPhaseInfo = new Dictionary<int, SHPhaseInfo>();
    #endregion


    #region System Functions
    public JsonPhaseInfo()
    {
        m_strFileName     = "PhaseInfo";
        m_strByteFileName = "PhaseInfo";
    }
    #endregion


    #region Override Functions
    public override void Initialize()
    {
        m_dicPhaseInfo.Clear();
    }
    public override bool IsLoadTable()
    {
        return (0 != m_dicPhaseInfo.Count);
    }
    public override bool? LoadJsonTable(JSONNode pJson, string strFileName)
    {
        if (null == pJson)
            return false;

        JSONNode pDataNode = pJson["PhaseInfo"];
        for(int iPhaseLoop = 0; iPhaseLoop < pDataNode.Count; ++iPhaseLoop)
        {
            var pPhaseNode = pDataNode[iPhaseLoop];
            var pPhaseInfo = new SHPhaseInfo();

            pPhaseInfo.m_iPhaseID       = iPhaseLoop;
            pPhaseInfo.m_iPhaseCount    = GetIntToJson(pPhaseNode, "m_iPhaseCount");
            pPhaseInfo.m_fMonGenDaly    = GetFloatToJson(pPhaseNode, "m_fMonGenDaly");
            pPhaseInfo.m_iMonMaxGen     = GetIntToJson(pPhaseNode, "m_iMonMaxGen");
            pPhaseInfo.m_iMonMaxCount   = GetIntToJson(pPhaseNode, "m_iMonMaxCount");
            
            var pMonsterNode = pPhaseNode["m_pMonsterGenInfo"];
            for (int iMonLoop = 0; iMonLoop< pMonsterNode.Count; ++iMonLoop)
            {
                var pMonInfo        = new SHPhaseMonsterGenInfo();
                pMonInfo.m_strName  = GetStrToJson(pMonsterNode[iMonLoop],   "m_strName");
                pMonInfo.m_fWeight  = GetFloatToJson(pMonsterNode[iMonLoop], "m_fWeight");
                pPhaseInfo.m_pMonsterGenInfo.Add(pMonInfo);
            }

            AddData(pPhaseInfo.m_iPhaseID, pPhaseInfo);
        }

        return true;
    }
    public override bool? LoadBytesTable(byte[] pByte)
    {
        if (null == pByte)
            return false;
        
        var pSerializer = new SHSerializer(pByte);
        var iPhaseCount = pSerializer.DeserializeInt();
        for(int iPhaseLoop = 0; iPhaseLoop < iPhaseCount; ++iPhaseLoop)
        {
            var pPhaseInfo              = new SHPhaseInfo();
            pPhaseInfo.m_iPhaseID       = pSerializer.DeserializeInt();
            pPhaseInfo.m_iPhaseCount    = pSerializer.DeserializeInt();
            pPhaseInfo.m_fMonGenDaly    = pSerializer.DeserializeFloat();
            pPhaseInfo.m_iMonMaxGen     = pSerializer.DeserializeInt();
            pPhaseInfo.m_iMonMaxCount   = pSerializer.DeserializeInt();

            var iMonCount               = pSerializer.DeserializeInt();
            for (int iMonLoop = 0; iMonLoop < iMonCount; ++iMonLoop)
            {
                var pMonInfo = new SHPhaseMonsterGenInfo();
                pMonInfo.m_strName = pSerializer.DeserializeString();
                pMonInfo.m_fWeight = pSerializer.DeserializeFloat();
                pPhaseInfo.m_pMonsterGenInfo.Add(pMonInfo);
            }

            AddData(pPhaseInfo.m_iPhaseID, pPhaseInfo);
        }
        
        return true;
    }
    public override byte[] GetBytesTable()
    {
        if (false == IsLoadTable())
            LoadJson(m_strFileName);

        var pSerializer = new SHSerializer();

        pSerializer.Serialize(m_dicPhaseInfo.Count);
        foreach(var kvp in m_dicPhaseInfo)
        {
            pSerializer.Serialize(kvp.Value.m_iPhaseID);
            pSerializer.Serialize(kvp.Value.m_iPhaseCount);
            pSerializer.Serialize(kvp.Value.m_fMonGenDaly);
            pSerializer.Serialize(kvp.Value.m_iMonMaxGen);
            pSerializer.Serialize(kvp.Value.m_iMonMaxCount);

            pSerializer.Serialize(kvp.Value.m_pMonsterGenInfo.Count);
            foreach(var pMonInfo in kvp.Value.m_pMonsterGenInfo)
            {
                pSerializer.Serialize(pMonInfo.m_strName);
                pSerializer.Serialize(pMonInfo.m_fWeight);
            }
        }
        
        return pSerializer.ByteArray;
    }
    #endregion


    #region Utility Functions
    void AddData(int iPhaseID, SHPhaseInfo pInfo)
    {
        if (false == m_dicPhaseInfo.ContainsKey(iPhaseID))
            m_dicPhaseInfo.Add(iPhaseID, pInfo);
        else
            m_dicPhaseInfo[iPhaseID] = pInfo;
    }
    void LoadTable()
    {
#if UNITY_EDITOR
        LoadJson(m_strFileName);
#else
        LoadBytes(m_strByteFileName);
#endif
    }
    #endregion


    #region Interface Functions
    public SHPhaseInfo GetPhaseInfo(int iKey)
    {
        if (false == IsLoadTable())
            LoadTable();

        if (false == IsLoadTable())
            return new SHPhaseInfo();

        if (false == m_dicPhaseInfo.ContainsKey(iKey))
            return m_dicPhaseInfo[m_dicPhaseInfo.Count - 1];
        
        return m_dicPhaseInfo[iKey];
    }
    #endregion
}