using UnityEngine;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public partial class SHTableData : SHBaseData
{
    #region Members : TableData
    private Dictionary<Type, SHBaseTable> m_dicTables = new Dictionary<Type, SHBaseTable>();
    public Dictionary<Type, SHBaseTable> Tables { get { return m_dicTables; } }
    #endregion


    #region Members : FileMonitor
    private FileSystemWatcher            m_pFMToDB          = null;
    private FileSystemWatcher            m_pFMToJson        = null;
    private FileSystemWatcher            m_pFMToXML         = null;
    private FileSystemWatcher            m_pFMToByte        = null;
    private Dictionary<eTableType, Type> m_dicFMChangedList = new Dictionary<eTableType, Type>();
    #endregion


    #region Override Functions
    public override void OnInitialize()
    {
        m_dicTables.Clear();

        m_dicTables.Add(typeof(JsonClientConfiguration),          new JsonClientConfiguration());
        m_dicTables.Add(typeof(JsonServerConfiguration),          new JsonServerConfiguration());
        m_dicTables.Add(typeof(JsonPreLoadResourcesTable),        new JsonPreLoadResourcesTable());
        m_dicTables.Add(typeof(JsonResourcesTable),               new JsonResourcesTable());
        m_dicTables.Add(typeof(JsonAssetBundleInfo),              new JsonAssetBundleInfo());
        m_dicTables.Add(typeof(JsonConstants),                    new JsonConstants());
        m_dicTables.Add(typeof(JsonPhaseInfo),                    new JsonPhaseInfo());
        m_dicTables.Add(typeof(JsonBuffInfo),                     new JsonBuffInfo());
        m_dicTables.Add(typeof(JsonWeaponInfo),                   new JsonWeaponInfo());

#if UNITY_EDITOR
        RegisterFileMonitor();
#endif
    }
    public override void OnFinalize()
    {
        m_dicTables.Clear();

#if UNITY_EDITOR
        DesposeFileMonitor();
#endif
    }
    public override void FrameMove()
    {
#if UNITY_EDITOR
        CheckFileMonitor();
#endif
    }
    public override Dictionary<string, SHLoadData> GetLoadList(eSceneType eType)
    {
        var dicLoadList = new Dictionary<string, SHLoadData>();

        // 로컬 테이블 데이터
        SHUtils.ForToDic<Type, SHBaseTable>(m_dicTables,
        (pKey, pValue) =>
        {
            // 이미 로드된 데이터인지 체크
            if (true == pValue.IsLoadTable())
                return;

            dicLoadList.Add(pValue.m_strFileName, CreateLoadInfo(pValue.m_strFileName));
        });

        return dicLoadList; 
    }
    public override Dictionary<string, SHLoadData> GetPatchList()
    {
        return new Dictionary<string, SHLoadData>();
    }
    public override void Load(SHLoadData pInfo, Action<string, SHLoadStartInfo> pStart,
                                                Action<string, SHLoadEndInfo> pDone)
    {
        if (0 == m_dicTables.Count)
            OnInitialize();

        var pType = GetTypeToFileName(pInfo.m_strName);
        if (false == m_dicTables.ContainsKey(pType))
        {
            Debug.LogError(string.Format("[TableData] 등록된 테이블이 아닙니다.!!({0})", pInfo.m_strName));
            pDone(pInfo.m_strName, new SHLoadEndInfo(false, eLoadErrorCode.Load_Table));
            return;
        }
        
        var eErrorCode = Load(m_dicTables[pType]);
        pDone(pInfo.m_strName, new SHLoadEndInfo((eLoadErrorCode.None == eErrorCode), eErrorCode));
    }
    public override void Patch(SHLoadData pInfo, Action<string, SHLoadStartInfo> pStart,
                                                 Action<string, SHLoadEndInfo> pDone)
    {
    }
    #endregion


    #region Interface Functions
    public SHLoadData CreateLoadInfo(string strName)
    {
        return new SHLoadData()
        {
            m_eDataType = eDataType.LocalTable,
            m_strName   = strName,
            m_pLoadFunc = Load
        };
    }
    public eLoadErrorCode Load(SHBaseTable pTable)
    {
        if (null == pTable)
            return eLoadErrorCode.Load_Table;

        foreach(var pLambda in GetLoadOrder(pTable))
        {
            bool? bIsSuccess = pLambda();
            if (null != bIsSuccess)
            {
                if (true == bIsSuccess.Value)
                    return eLoadErrorCode.None;
                else
                    return eLoadErrorCode.Load_Table;
            }
        }

        return eLoadErrorCode.Load_Table;
    }
    public T GetTable<T>() where T : SHBaseTable
    {
        return GetTable(typeof(T)) as T;
    }
    public SHBaseTable GetTable(Type pType)
    {
        if (0 == m_dicTables.Count)
            OnInitialize();

        if (false == m_dicTables.ContainsKey(pType))
            return null;

        if (false == m_dicTables[pType].IsLoadTable())
            Load(m_dicTables[pType]);
        
        return m_dicTables[pType];
    }
    public SHBaseTable GetTable(string strFileName)
    {
        if (true == string.IsNullOrEmpty(strFileName))
            return null;

        return GetTable(GetTypeToFileName(strFileName));
    }
    public ICollection GetData<T>()
    {
        return GetData(typeof(T));
    }
    public ICollection GetData(string strClassType)
    {
        return GetData(Type.GetType(strClassType));
    }
    public ICollection GetData(Type pType)
    {
        SHBaseTable pTable = GetTable(pType);
        if (null == pTable)
            return null;

        return pTable.GetData();
    }
    public Type GetTypeToFileName(string strFileName)
    {
        strFileName = Path.GetFileNameWithoutExtension(strFileName);
        foreach (var kvp in m_dicTables)
        {
            if (true == kvp.Value.m_strFileName.Equals(strFileName))
                return kvp.Key;
        }

        return null;
    }
    #endregion


    #region Utility Functions
    // 유틸 : 테이블 타입별 로드 순서 ( 앞선 타입의 로드에 성공하면 뒤 타입들은 로드명령 하지 않는다 )
    List<Func<bool?>> GetLoadOrder(SHBaseTable pTable)
    {
        var pLoadOrder = new List<Func<bool?>>();
        pLoadOrder.Add(() => { return pTable.LoadStatic();                        });
#if UNITY_EDITOR
        pLoadOrder.Add(() => { return pTable.LoadJson(pTable.m_strFileName);      });
        pLoadOrder.Add(() => { return pTable.LoadXML(pTable.m_strFileName);       });
        pLoadOrder.Add(() => { return pTable.LoadDB(pTable.m_strFileName);        });
        pLoadOrder.Add(() => { return pTable.LoadBytes(pTable.m_strByteFileName); });
#else
        pLoadOrder.Add(() => { return pTable.LoadBytes(pTable.m_strByteFileName); });
        pLoadOrder.Add(() => { return pTable.LoadJson(pTable.m_strFileName);      });
        pLoadOrder.Add(() => { return pTable.LoadXML(pTable.m_strFileName);       });
        pLoadOrder.Add(() => { return pTable.LoadDB(pTable.m_strFileName);        });
#endif
        return pLoadOrder;
    }
#endregion


#region FileMonitor Functions
    void CheckFileMonitor()
    {
        foreach (var kvp in m_dicFMChangedList)
        {
            var pTable = GetTable(kvp.Value);
            if (null == pTable)
                continue;

            switch (kvp.Key)
            {
                case eTableType.XML:    pTable.LoadXML(pTable.m_strFileName);   break;
                case eTableType.SQLite: pTable.LoadDB(pTable.m_strFileName);    break;
                case eTableType.Json:   pTable.LoadJson(pTable.m_strFileName);  break;
                case eTableType.Byte:   pTable.LoadBytes(pTable.m_strFileName); break;
            }
        }

        m_dicFMChangedList.Clear();
    }
    void RegisterFileMonitor()
    {
        if (false == Application.isPlaying)
            return;

        DesposeFileMonitor();
        
        // .DB파일 모니터 등록
        if ((null == m_pFMToDB) &&
            (true == SHUtils.IsExistsDirectory(SHPath.GetPathToSQLite())))
        {
            m_pFMToDB                       = new FileSystemWatcher();
            m_pFMToDB.Path                  = SHPath.GetPathToSQLite();
            m_pFMToDB.NotifyFilter          = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFMToDB.Filter                = "*.db";
            m_pFMToDB.Changed               += new FileSystemEventHandler(OnEventToChangedDB);
            m_pFMToDB.EnableRaisingEvents   = true;
            m_pFMToDB.IncludeSubdirectories = true;
        }
        
        // .Json파일 모니터 등록
        if ((null == m_pFMToJson) &&
            (true == SHUtils.IsExistsDirectory(SHPath.GetPathToJson())))
        {
            m_pFMToJson                     = new FileSystemWatcher();
            m_pFMToJson.Path                = SHPath.GetPathToJson();
            m_pFMToJson.NotifyFilter        = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFMToJson.Filter              = "*.json";
            m_pFMToJson.Changed             += new FileSystemEventHandler(OnEventToChangedJson);
            m_pFMToJson.EnableRaisingEvents = true;
            m_pFMToJson.IncludeSubdirectories = true;
        }

        // XML파일 모니터 등록
        if ((null == m_pFMToJson) &&
            (true == SHUtils.IsExistsDirectory(SHPath.GetPathToXML())))
        {
            m_pFMToXML                      = new FileSystemWatcher();
            m_pFMToXML.Path                 = SHPath.GetPathToXML();
            m_pFMToXML.NotifyFilter         = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFMToXML.Filter               = "*.xml";
            m_pFMToXML.Changed              += new FileSystemEventHandler(OnEventToChangedXML);
            m_pFMToXML.EnableRaisingEvents  = true;
            m_pFMToXML.IncludeSubdirectories = true;
        }

        // Byte파일 모니터 등록
        if ((null == m_pFMToByte) &&
            (true == SHUtils.IsExistsDirectory(SHPath.GetPathToBytes())))
        {
            m_pFMToByte                     = new FileSystemWatcher();
            m_pFMToByte.Path                = SHPath.GetPathToBytes();
            m_pFMToByte.NotifyFilter        = (NotifyFilters.LastWrite | NotifyFilters.Size);
            m_pFMToByte.Filter              = "*.bytes";
            m_pFMToByte.Changed             += new FileSystemEventHandler(OnEventToChangedByte);
            m_pFMToByte.EnableRaisingEvents = true;
            m_pFMToByte.IncludeSubdirectories = true;
        }
    }
    void DesposeFileMonitor()
    {
        if (null != m_pFMToDB)    m_pFMToDB.Dispose();
        if (null != m_pFMToJson)  m_pFMToJson.Dispose();
        if (null != m_pFMToXML)   m_pFMToXML.Dispose();
        if (null != m_pFMToByte)  m_pFMToByte.Dispose();

        m_pFMToDB     = null;
        m_pFMToJson   = null;
        m_pFMToXML    = null;
        m_pFMToByte   = null;
        m_dicFMChangedList.Clear();
    }
    void OnEventToChangedDB(object pSender, FileSystemEventArgs pArgs)
    {
        var pType = GetTypeToFileName(pArgs.FullPath);
        if (null == pType)
            return;

        m_dicFMChangedList.Add(eTableType.SQLite, pType);
    }
    void OnEventToChangedJson(object pSender, FileSystemEventArgs pArgs)
    {
        var pType = GetTypeToFileName(pArgs.FullPath);
        if (null == pType)
            return;

        m_dicFMChangedList.Add(eTableType.Json, pType);
    }
    void OnEventToChangedXML(object pSender, FileSystemEventArgs pArgs)
    {
        var pType = GetTypeToFileName(pArgs.FullPath);
        if (null == pType)
            return;

        m_dicFMChangedList.Add(eTableType.XML, pType);
    }
    void OnEventToChangedByte(object pSender, FileSystemEventArgs pArgs)
    {
        var pType = GetTypeToFileName(pArgs.FullPath);
        if (null == pType)
            return;

        m_dicFMChangedList.Add(eTableType.Byte, pType);
    }
#endregion
}