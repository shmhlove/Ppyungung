using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;

public class JsonConstants : SHBaseTable
{
    #region Members : Constants
    // 캐릭터 관련
    public float         m_iCharMaxHP        = 3.0f;
    public float         m_fCharMoveSpeed    = 50.0f;
    public float         m_fCharDashSpeed    = 150.0f;
    public float         m_fCharAddDashPoint = 5.0f;
    public float         m_fCharDecDashPoint = 1.0f;
    public float         m_fCharMaxDashPoint = 100.0f;

    // 몬스터 관련
    public float         m_fMonMoveSpeed    = 30.0f;
    public float         m_fMonDamageSpeed  = 5000.0f;
    public float         m_fMonGenDaly      = 2.0f;
    public int           m_iMonMaxGen       = 4;
    public int           m_iMonMaxCount     = 12;

    // 이동 관련
    public float         m_fBasicMoveSpeed  = 0.5f;
    public float         m_fMoveLimitX      = 100.0f;
    public float         m_fMoveLimitY      = 100.0f;

    // 기타
    public float         m_fUnitScale       = 1.0f;
    public int           m_iFrameRate       = 45;

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
        m_iCharMaxHP            = GetFloatToJson(pDataNode, "m_iCharMaxHP");
        m_fCharMoveSpeed        = GetFloatToJson(pDataNode, "m_fCharMoveSpeed");
        m_fCharDashSpeed        = GetFloatToJson(pDataNode, "m_fCharDashSpeed");
        m_fCharAddDashPoint     = GetFloatToJson(pDataNode, "m_fCharAddDashPoint");
        m_fCharDecDashPoint     = GetFloatToJson(pDataNode, "m_fCharDecDashPoint");
        m_fCharMaxDashPoint     = GetFloatToJson(pDataNode, "m_fCharMaxDashPoint");

        // 몬스터 관련
        m_fMonMoveSpeed         = GetFloatToJson(pDataNode, "m_fMonMoveSpeed");
        m_fMonDamageSpeed       = GetFloatToJson(pDataNode, "m_fMonDamageSpeed");
        m_fMonGenDaly           = GetFloatToJson(pDataNode, "m_fMonGenDaly");
        m_iMonMaxGen            = GetIntToJson(pDataNode,   "m_iMonMaxGen");
        m_iMonMaxCount          = GetIntToJson(pDataNode,   "m_iMonMaxCount");

        // 이동 관련
        m_fBasicMoveSpeed       = GetFloatToJson(pDataNode, "m_fBasicMoveSpeed");
        m_fMoveLimitX           = GetFloatToJson(pDataNode, "m_fMoveLimitX");
        m_fMoveLimitY           = GetFloatToJson(pDataNode, "m_fMoveLimitY");

        // 기타
        m_fUnitScale            = GetFloatToJson(pDataNode, "m_fUnitScale");
        m_iFrameRate            = GetIntToJson(pDataNode, "m_iFrameRate");
        
        return (m_bIsLoaded = true);
    }
    public override bool? LoadBytesTable(byte[] pByte)
    {
        if (null == pByte)
            return false;

        var pSerializer = new SHSerializer(pByte);

        // 캐릭터 관련
        m_iCharMaxHP            = pSerializer.DeserializeFloat();
        m_fCharMoveSpeed        = pSerializer.DeserializeFloat();
        m_fCharDashSpeed        = pSerializer.DeserializeFloat();
        m_fCharAddDashPoint     = pSerializer.DeserializeFloat();
        m_fCharDecDashPoint     = pSerializer.DeserializeFloat();
        m_fCharMaxDashPoint     = pSerializer.DeserializeFloat();

        // 몬스터 관련
        m_fMonMoveSpeed         = pSerializer.DeserializeFloat();
        m_fMonDamageSpeed       = pSerializer.DeserializeFloat();
        m_fMonGenDaly           = pSerializer.DeserializeFloat();
        m_iMonMaxGen            = pSerializer.DeserializeInt();
        m_iMonMaxCount          = pSerializer.DeserializeInt();

        // 이동 관련
        m_fBasicMoveSpeed       = pSerializer.DeserializeFloat();
        m_fMoveLimitX           = pSerializer.DeserializeFloat();
        m_fMoveLimitY           = pSerializer.DeserializeFloat();

        // 기타
        m_fUnitScale            = pSerializer.DeserializeFloat();
        m_iFrameRate            = pSerializer.DeserializeInt();

        return (m_bIsLoaded = true);
    }
    public override byte[] GetBytesTable()
    {
        if (false == IsLoadTable())
            LoadJson(m_strFileName);

        var pSerializer = new SHSerializer();

        // 캐릭터 관련
        pSerializer.Serialize(m_iCharMaxHP);
        pSerializer.Serialize(m_fCharMoveSpeed);
        pSerializer.Serialize(m_fCharDashSpeed);
        pSerializer.Serialize(m_fCharAddDashPoint);
        pSerializer.Serialize(m_fCharDecDashPoint);
        pSerializer.Serialize(m_fCharMaxDashPoint);

        // 몬스터 관련
        pSerializer.Serialize(m_fMonMoveSpeed);
        pSerializer.Serialize(m_fMonDamageSpeed);
        pSerializer.Serialize(m_fMonGenDaly);
        pSerializer.Serialize(m_iMonMaxGen);
        pSerializer.Serialize(m_iMonMaxCount);

        // 이동관련
        pSerializer.Serialize(m_fBasicMoveSpeed);
        pSerializer.Serialize(m_fMoveLimitX);
        pSerializer.Serialize(m_fMoveLimitY);

        // 기타
        pSerializer.Serialize(m_fUnitScale);
        pSerializer.Serialize(m_iFrameRate);

        return pSerializer.ByteArray;
    }
    #endregion
}