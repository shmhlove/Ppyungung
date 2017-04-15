using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class JsonBuffInfo : SHBaseTable
{
    #region Members : Datas
    public float m_fRatioUpgradeMaxHeath = 0.0f;
    public float m_fRatioUpgradeAddDP    = 0.0f;
    public float m_fRatioUpgradeDecDP    = 0.0f;
    public float m_fRatioUpgradeDropCoin = 0.0f;
    public float m_fRatioUpgradeBulletSP = 0.0f;
    public float m_fRatioUpgradeMoveSP   = 0.0f;
    public float m_fRatioDecreaseMonSP   = 0.0f;
    #endregion


    #region Members : ETC
    public bool m_bIsLoaded = false;
    #endregion


    #region System Functions
    public JsonBuffInfo()
    {
        m_strFileName     = "BuffInfo";
        m_strByteFileName = "BuffInfo";
    }
    #endregion


    #region Virtual Functions
    public override void Initialize() { }
    public override bool IsLoadTable()
    {
        return m_bIsLoaded;
    }
    public override bool? LoadJsonTable(JSONNode pJson, string strFileName)
    {
        if (null == pJson)
            return false;

        JSONNode pDataNode = pJson["BuffInfo"];

        m_fRatioUpgradeMaxHeath = GetFloatToJson(pDataNode, "m_fRatioUpgradeMaxHeath");
        m_fRatioUpgradeAddDP    = GetFloatToJson(pDataNode, "m_fRatioUpgradeAddDP");
        m_fRatioUpgradeDecDP    = GetFloatToJson(pDataNode, "m_fRatioUpgradeDecDP");
        m_fRatioUpgradeDropCoin = GetFloatToJson(pDataNode, "m_fRatioUpgradeDropCoin");
        m_fRatioUpgradeBulletSP = GetFloatToJson(pDataNode, "m_fRatioUpgradeBulletSP");
        m_fRatioUpgradeMoveSP   = GetFloatToJson(pDataNode, "m_fRatioUpgradeMoveSP");
        m_fRatioDecreaseMonSP   = GetFloatToJson(pDataNode, "m_fRatioDecreaseMonSP");

        return (m_bIsLoaded = true);
    }
    public override bool? LoadBytesTable(byte[] pByte)
    {
        if (null == pByte)
            return false;
        
        var pSerializer = new SHSerializer(pByte);
        
        m_fRatioUpgradeMaxHeath = pSerializer.DeserializeFloat();
        m_fRatioUpgradeAddDP    = pSerializer.DeserializeFloat();
        m_fRatioUpgradeDecDP    = pSerializer.DeserializeFloat();
        m_fRatioUpgradeDropCoin = pSerializer.DeserializeFloat();
        m_fRatioUpgradeBulletSP = pSerializer.DeserializeFloat();
        m_fRatioUpgradeMoveSP   = pSerializer.DeserializeFloat();
        m_fRatioDecreaseMonSP   = pSerializer.DeserializeFloat();
        
        return true;
    }
    public override byte[] GetBytesTable()
    {
        if (false == IsLoadTable())
            LoadJson(m_strFileName);

        var pSerializer = new SHSerializer();
        
        pSerializer.Serialize(m_fRatioUpgradeMaxHeath);
        pSerializer.Serialize(m_fRatioUpgradeAddDP);
        pSerializer.Serialize(m_fRatioUpgradeDecDP);
        pSerializer.Serialize(m_fRatioUpgradeDropCoin);
        pSerializer.Serialize(m_fRatioUpgradeBulletSP);
        pSerializer.Serialize(m_fRatioUpgradeMoveSP);
        pSerializer.Serialize(m_fRatioDecreaseMonSP);

        return pSerializer.ByteArray;
    }
    #endregion
}