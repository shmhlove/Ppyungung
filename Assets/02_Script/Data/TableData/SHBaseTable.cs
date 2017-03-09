﻿using UnityEngine;

using System;
using System.Collections;

using SimpleJSON;
using System.Xml;

public abstract class SHBaseTable
{
    #region Members
    // 파일이름
    public string       m_strFileName;
    public string       m_strByteFileName;

    // 데이터 타입
    public eDataType    m_eDataType = eDataType.LocalTable;

    // SQL 데이터 Reader
    SQLiteQuery         m_pSQLiteReader  = null;
    #endregion


    #region Virtual Functions
    public virtual void Initialize() { }
    public abstract bool IsLoadTable();
    // 다양화(로드) : 서버 웹 데이터
    public virtual WWW LoadWebData(Action<bool> pDone)                          { return null; }
    // 다양화(로드) : 코드로 데이터를 생성(Hard한 데이터)
    public virtual bool? LoadStaticTable()                                      { return null; }
    // 다양화(로드) : SQLite파일에서 로드
    public virtual bool? LoadDBTable(SQLiteQuery pTable, string strTableName)   { return null; }
    // 다양화(로드) : Json파일에서 로드
    public virtual bool? LoadJsonTable(JSONNode pJson, string strTableName)     { return null; }
    // 다양화(로드) : XML파일에서 로드
    public virtual bool? LoadXMLTable(XmlNode pNode)                            { return null; }
    // 다양화(로드) : Byte파일에서 로드
    public virtual bool? LoadBytesTable(byte[] pByte)                           { return null; }
    // 다양화(로드) : 컨테이너를 시리얼 라이즈해서 Byte파일로 내어주는 함수
    public virtual byte[] GetBytesTable()                                       { return null; }
    // 다양화(로드) : 컨테이너를 콜렉션형태로 내어주는 함수
    public virtual ICollection GetData()                                        { return null; }
    #endregion


    #region Interface Functions
    // 인터페이스 : Hard한 데이터 로드
    public bool? LoadStatic()
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadStaticTable())
            return Return(null);
        
        Initialize();

        return LoadStaticTable();
    }

    // 인터페이스 : SQLite파일 로드
    public bool? LoadDB(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadDBTable(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        SHSQLite pSQLite = new SHSQLite(strFileName);
        if (false == pSQLite.CheckDBFile())
            return Return(false);

        // 예외처리 : 테이블체크
        SQLiteQuery pTableList = pSQLite.GetTable("TableList");
        if (null == pTableList)
            return Return(false);

        Initialize();

        while (true == pTableList.Step())
        {
            string strTableName = pTableList.GetString("s_Table");
            m_pSQLiteReader     = pSQLite.GetTable(strTableName);
            bool? bResult       = LoadDBTable(m_pSQLiteReader, strTableName);
            if (null != m_pSQLiteReader)
            {
                m_pSQLiteReader.Release();
                m_pSQLiteReader = null;
            }
            if (null == bResult)        return Return(null);
            if (false == bResult.Value) return Return(false);
        }

        pTableList.Release();
        pSQLite.Clear();

        return Return(true);
    }

    // 인터페이스 : Json파일 로드
    public bool? LoadJson(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadJsonTable(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        SHJson pJson = new SHJson(strFileName);
        if (false == pJson.CheckJson())
            return Return(false);

        Initialize();

        return Return(LoadJsonTable(pJson.Node, m_strFileName));
    }
    public bool? LoadJsonToLocal(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadJsonTable(null, null))
            return Return(null);

        // 예외처리 : 로드체크
        SHJson pJson = new SHJson();
        if (null == pJson.LoadToPersistent(strFileName))
            pJson.LoadToStreamingForLocal(strFileName);

        if (false == pJson.CheckJson())
            return Return(false);

        Initialize();

        return Return(LoadJsonTable(pJson.Node, m_strFileName));
    }

    // 인터페이스 : XML파일 로드
    public bool? LoadXML(string strFileName) 
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadXMLTable(null))
            return Return(null);

        return LoadXML(new SHXML(strFileName));
    }
    
    // 인터페이스 : XML파일 로드
    public bool? LoadXML(SHXML pXML)
    {
        if (null == pXML)
            return Return(false);

        // 예외처리 : 오버라이드 체크
        if (null == LoadXMLTable(null))
            return Return(null);

        if (false == pXML.CheckXML())
            return Return(false);

        // 예외처리 : 노드 리스트 체크
        XmlNodeList pNodeList = pXML.GetNodeList(m_strFileName);
        if (null == pNodeList)
            return Return(false);

        Initialize();

        int iMaxNodeCount = pNodeList.Count;
        for (int iLoop = 0; iLoop < iMaxNodeCount; ++iLoop)
        {
            bool? bResult = LoadXMLTable(pNodeList[iLoop]);
            if (null == bResult)        return Return(null);
            if (false == bResult.Value) return Return(false);
        }

        return Return(true);
    }

    // 인터페이스 : Byte파일 로드
    public bool? LoadBytes(string strFileName)
    {
        // 예외처리 : 오버라이드 체크
        if (null == LoadBytesTable(null))
            return Return(null);

        // 예외처리 : 로드체크(실패시 다른 로드방식으로 로드가능하도록 null 리턴)
        SHBytes pBytes = new SHBytes(strFileName);
        if (false == pBytes.CheckBytes())
            return Return(null);

        Initialize();

        return Return(LoadBytesTable(pBytes.GetBytes()));
    }

    // 인터페이스 : 파일 저장된 경로얻기
    public string GetFilePath()
    {
        string strPath = string.Empty;
        switch (GetTableType())
        {
            case eTableType.SQLite:
                strPath = SHSQLite.GetStreamingPath(m_strFileName);
                break;
            case eTableType.Json:
                strPath = SHJson.GetStreamingPath(m_strFileName);
                break;
            case eTableType.XML:
                strPath = string.Format("{0}/{1}", SHPath.GetPathToXML(), m_strFileName);
                break;
            case eTableType.Byte:
                strPath = string.Format("{0}/{1}", SHPath.GetPathToBytes(), m_strFileName);
                break;
        }

        strPath = strPath.Replace("file://", "");
        strPath = strPath.Replace("jar: file://", "");

        return strPath;
    }
    #endregion


    #region Utility Functions
    // 유틸 : 로드함수가 종료될때 Reader객체를 초기화하고, 리턴될 수 있도록
    bool? Return(bool? bReturnValue)
    {
        if (null != m_pSQLiteReader)
            m_pSQLiteReader.Release();

        m_pSQLiteReader = null;

        return bReturnValue;
    }
    
    // 유틸 : 테이블 타입
    eTableType GetTableType()
    {
        if (null != LoadXMLTable(null))        return eTableType.XML;
        if (null != LoadJsonTable(null, null)) return eTableType.Json;
        if (null != LoadDBTable(null, null))   return eTableType.SQLite;
        if (null != LoadBytesTable(null))      return eTableType.Byte;

        return eTableType.None;
    }
    #endregion


    #region Interface : SQLite
    // 유틸함수 : DB에서 String데이터 얻기
    public string GetStrToSQL(string strKey)
    {
        if (null == m_pSQLiteReader)
            return string.Empty;

        return m_pSQLiteReader.GetString(strKey);
    }
    // 유틸함수 : DB에서 int데이터 얻기
    public int GetIntToSQL(string strKey)
    {
        if (null == m_pSQLiteReader)
            return 0;

        return m_pSQLiteReader.GetInteger(strKey);
    }
    // 유틸함수 : DB에서 float데이터 얻기
    public float GetFloatToSQL(string strKey)
    {
        if (null == m_pSQLiteReader)
            return 0.0f;

        return (float)m_pSQLiteReader.GetDouble(strKey);
    }
    // 유틸함수 : DB에서 vector3데이터 얻기
    public Vector3 GetVectorToSQL(string strKey)
    {
        return new Vector3(GetFloatToSQL(strKey + "X"),
                           GetFloatToSQL(strKey + "Y"), 
                           GetFloatToSQL(strKey + "Z"));
    }
    #endregion


    #region Interface : Json
    // 유틸함수 : Json에서 String데이터 얻기
    public string GetStrToJson(JSONNode pNode, string strKey)
    {
        if (null == pNode)
            return string.Empty;

        return pNode[strKey].Value;
    }
    // 유틸함수 : Json에서 int데이터 얻기
    public int GetIntToJson(JSONNode pNode, string strKey)
    {
        if (null == pNode)
            return 0;

        return pNode[strKey].AsInt;
    }
    // 유틸함수 : Json에서 float데이터 얻기
    public float GetFloatToJson(JSONNode pNode, string strKey)
    {
        if (null == pNode)
            return 0.0f;

        return pNode[strKey].AsFloat;
    }
    // 유틸함수 : Json에서 bool데이터 얻기
    public bool GetBoolToJson(JSONNode pNode, string strKey)
    {
        if (null == pNode)
            return false;

        return pNode[strKey].AsBool;
    }
    // 유틸함수 : Json에서 vector3데이터 얻기
    public Vector3 GetVector3ToJson(JSONArray pArray)
    {
        if ((null == pArray) || (3 > pArray.Count))
            return Vector3.zero;

        return new Vector3(pArray[0].AsFloat,
                           pArray[1].AsFloat,
                           pArray[2].AsFloat);
    }
    // 유틸함수 : Json에서 vector2데이터 얻기
    public Vector3 GetVector2ToJson(JSONArray pArray)
    {
        if ((null == pArray) || (2 > pArray.Count))
            return Vector2.zero;

        return new Vector2(pArray[0].AsFloat,
                           pArray[1].AsFloat);
    }
    #endregion


    #region Interface : XML
    // 유틸함수 : XML에서 Value데이터 얻기(형변환이 안된 데이터)
    public string GetValue(XmlNode pNode, string strKey)
    {
        if (null == pNode)
            return string.Empty;

        return pNode.Attributes.GetNamedItem(strKey).Value;
    }
    // 유틸함수 : XML에서 Int데이터 얻기
    public int GetInt(XmlNode pNode, string strKey)
    {
        int iValue = 0;
        int.TryParse(GetValue(pNode, strKey), out iValue);
        return iValue;
    }
    // 유틸함수 : XML에서 uInt데이터 얻기
    public uint GetuInt(XmlNode pNode, string strKey)
    {
        uint uiValue = 0;
        uint.TryParse(GetValue(pNode, strKey), out uiValue);
        return uiValue;
    }
    // 유틸함수 : XML에서 Long데이터 얻기
    public long GetLong(XmlNode pNode, string strKey)
    {
        long lValue = 0;
        long.TryParse(GetValue(pNode, strKey), out lValue);
        return lValue;
    }
    // 유틸함수 : XML에서 uLong데이터 얻기
    public ulong GetuLong(XmlNode pNode, string strKey)
    {
        ulong ulValue = 0;
        ulong.TryParse(GetValue(pNode, strKey), out ulValue);
        return ulValue;
    }
    // 유틸함수 : XML에서 Float데이터 얻기
    public float GetFloat(XmlNode pNode, string strKey)
    {
        float fValue = 0.0f;
        float.TryParse(GetValue(pNode, strKey), out fValue);
        return fValue;
    }
    // 유틸함수 : XML에서 Bool데이터 얻기
    public bool GetBool(XmlNode pNode, string strKey)
    {
        return 0 != GetInt(pNode, strKey);
    }
    // 유틸함수 : XML에서 String데이터 얻기
    public string GetStr(XmlNode pNode, string strKey)
    {
        return GetValue(pNode, strKey);
    }
    // 유틸함수 : XML에서 Time데이터 얻기
    public DateTime GetTime(XmlNode pNode, string strKey, string strFormat)
    {
        string strDate = GetStr(pNode, strKey);
        if (true == string.IsNullOrEmpty(strDate))
            return DateTime.Now;

        return SHUtils.GetDateTimeToString(strDate, strFormat);
    }
    #endregion
}
