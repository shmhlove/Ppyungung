using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class JsonConstants : SHBaseTable
{
    #region Members : Constants
    // 캐릭터 관련
    public float         m_fCharMoveSpeed   = 50.0f;
    public float         m_fCharDamageSpeed = 5000.0f;
    public float         m_fCharAutoShoot   = 0.2f;
    public float         m_fCharDashSpeed   = 150.0f;
    public float         m_fCharDashTime    = 0.5f;
    public float         m_fCharDashCool    = 0.0f;

    // 몬스터 관련
    public float         m_fMonMoveSpeed    = 30.0f;
    public float         m_fMonDamageSpeed  = 5000.0f;
    public float         m_fMonGenDaly      = 2.0f;
    public int           m_iMonMaxGen       = 4;
    public int           m_iMonMaxCount     = 12;

    // 기타
    public float         m_fUnitScale       = 1.0f;
    #endregion


    #region Members : ETC
    public bool m_bIsLoaded = false;
    #endregion


    #region System Functions
    public JsonConstants()
    {
        m_strFileName     = "Constants";
        m_strByteFileName = "Constants";
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

        JSONNode pDataNode = pJson["Constants"];
        
        // 캐릭터 관련
        m_fCharMoveSpeed   = GetFloatToJson(pDataNode, "m_fCharMoveSpeed");
        m_fCharDamageSpeed = GetFloatToJson(pDataNode, "m_fCharDamageSpeed");
        m_fCharAutoShoot   = GetFloatToJson(pDataNode, "m_fCharAutoShoot");
        m_fCharDashSpeed   = GetFloatToJson(pDataNode, "m_fCharDashSpeed");
        m_fCharDashTime    = GetFloatToJson(pDataNode, "m_fCharDashTime");
        m_fCharDashCool    = GetFloatToJson(pDataNode, "m_fCharDashCool");

        // 몬스터 관련
        m_fMonMoveSpeed    = GetFloatToJson(pDataNode, "m_fMonMoveSpeed");
        m_fMonDamageSpeed  = GetFloatToJson(pDataNode, "m_fMonDamageSpeed");
        m_fMonGenDaly      = GetFloatToJson(pDataNode, "m_fMonGenDaly");
        m_iMonMaxGen       = GetIntToJson(pDataNode,   "m_iMonMaxGen");
        m_iMonMaxCount     = GetIntToJson(pDataNode,   "m_iMonMaxCount");

        // 기타
        m_fUnitScale       = GetFloatToJson(pDataNode, "m_fUnitScale");


        return (m_bIsLoaded = true);
    }
    public override bool? LoadBytesTable(byte[] pByte)
    {
        if (null == pByte)
            return false;

        var pSerializer = new SHSerializer(pByte);
        
        // 캐릭터 관련
        m_fCharMoveSpeed   = pSerializer.DeserializeFloat();
        m_fCharDamageSpeed = pSerializer.DeserializeFloat();
        m_fCharAutoShoot   = pSerializer.DeserializeFloat();
        m_fCharDashSpeed   = pSerializer.DeserializeFloat();
        m_fCharDashTime    = pSerializer.DeserializeFloat();
        m_fCharDashCool    = pSerializer.DeserializeFloat();
        
        // 몬스터 관련
        m_fMonMoveSpeed    = pSerializer.DeserializeFloat();
        m_fMonDamageSpeed  = pSerializer.DeserializeFloat();
        m_fMonGenDaly      = pSerializer.DeserializeFloat();
        m_iMonMaxGen       = pSerializer.DeserializeInt();
        m_iMonMaxCount     = pSerializer.DeserializeInt();

        // 기타
        m_fUnitScale       = pSerializer.DeserializeFloat();

        return (m_bIsLoaded = true);
    }
    public override byte[] GetBytesTable()
    {
        if (false == IsLoadTable())
            LoadJson(m_strFileName);

        var pSerializer = new SHSerializer();

        // 캐릭터 관련
        pSerializer.Serialize(m_fCharMoveSpeed);
        pSerializer.Serialize(m_fCharDamageSpeed);
        pSerializer.Serialize(m_fCharAutoShoot);
        pSerializer.Serialize(m_fCharDashSpeed);
        pSerializer.Serialize(m_fCharDashTime);
        pSerializer.Serialize(m_fCharDashCool);

        // 몬스터 관련
        pSerializer.Serialize(m_fMonMoveSpeed);
        pSerializer.Serialize(m_fMonDamageSpeed);
        pSerializer.Serialize(m_fMonGenDaly);
        pSerializer.Serialize(m_iMonMaxGen);
        pSerializer.Serialize(m_iMonMaxCount);

        // 기타
        pSerializer.Serialize(m_fUnitScale);

        return pSerializer.ByteArray;
    }
    #endregion
}