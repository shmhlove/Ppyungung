using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class JsonWeaponData
{
    public eCharWeaponType m_eWeaponType   = eCharWeaponType.NormalBullet;
    public string          m_strDamageName = string.Empty;
    public float           m_fShootDelay   = 0.0f;
}

public class JsonWeaponInfo : SHBaseTable
{
    #region Members : Datas
    private Dictionary<eCharWeaponType, JsonWeaponData> m_dicWeaponInfo = new Dictionary<eCharWeaponType, JsonWeaponData>();
    #endregion


    #region System Functions
    public JsonWeaponInfo()
    {
        m_strFileName     = "WeaponInfo";
        m_strByteFileName = "WeaponInfo";
    }
    #endregion


    #region Override Functions
    public override void Initialize() { }
    public override bool IsLoadTable()
    {
        return (0 != m_dicWeaponInfo.Count);
    }
    public override bool? LoadJsonTable(JSONNode pJson, string strFileName)
    {
        if (null == pJson)
            return false;

        JSONNode pDataNode = pJson["WeaponInfo"];
        for(int iLoop = 0; iLoop < pDataNode.Count; ++iLoop)
        {
            var pWeaponNode = pDataNode[iLoop];
            var pData = new JsonWeaponData();
            pData.m_eWeaponType   = SHUtils.GetStringToEnum<eCharWeaponType>(GetStrToJson(pWeaponNode, "m_eWeaponType"));
            pData.m_strDamageName = GetStrToJson(pWeaponNode, "m_strDamageName");
            pData.m_fShootDelay   = GetFloatToJson(pWeaponNode, "m_fShootDelay");

            AddData(pData.m_eWeaponType, pData);
        }
        return true;
    }
    public override bool? LoadBytesTable(byte[] pByte)
    {
        if (null == pByte)
            return false;
        
        var pSerializer  = new SHSerializer(pByte);
        var iMaxLoop = pSerializer.DeserializeInt();
        for (int iLoop = 0; iLoop < iMaxLoop; ++iLoop)
        {
            var pData = new JsonWeaponData();
            pData.m_eWeaponType   = SHUtils.GetStringToEnum<eCharWeaponType>(pSerializer.DeserializeString());
            pData.m_strDamageName = pSerializer.DeserializeString();
            pData.m_fShootDelay   = pSerializer.DeserializeFloat();
            AddData(pData.m_eWeaponType, pData);
        }

        return true;
    }
    public override byte[] GetBytesTable()
    {
        if (false == IsLoadTable())
            LoadJson(m_strFileName);

        var pSerializer = new SHSerializer();
        pSerializer.Serialize(m_dicWeaponInfo.Count);
        foreach(var kvp in m_dicWeaponInfo)
        {
            pSerializer.Serialize(kvp.Value.m_eWeaponType.ToString());
            pSerializer.Serialize(kvp.Value.m_strDamageName);
            pSerializer.Serialize(kvp.Value.m_fShootDelay);
        }
        return pSerializer.ByteArray;
    }
    #endregion


    #region Utility Functions
    void AddData(eCharWeaponType eType, JsonWeaponData pData)
    {
        if (false == m_dicWeaponInfo.ContainsKey(eType))
            m_dicWeaponInfo.Add(eType, pData);

        m_dicWeaponInfo[eType] = pData;
    }
    #endregion


    #region Interface Functions
    public JsonWeaponData GetWeaponData(eCharWeaponType eType)
    {
        if (false == m_dicWeaponInfo.ContainsKey(eType))
            return new JsonWeaponData();

        return m_dicWeaponInfo[eType];
    }
    #endregion
}