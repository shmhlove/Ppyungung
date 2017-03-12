using UnityEngine;
using System.Collections;

public static partial class SHHard
{
    // 캐릭터 관련
    public static float m_iCharMaxHealthPoint
    {
        get { return GetTable().m_iCharMaxHP; }
        set { GetTable().m_iCharMaxHP = value; }
    }
    public static float m_fCharMoveSpeed
    {
        get { return GetTable().m_fCharMoveSpeed; }
        set { GetTable().m_fCharMoveSpeed = value; }
    }
    public static float m_fCharDamageSpeed
    {
        get { return GetTable().m_fCharDamageSpeed; }
        set { GetTable().m_fCharDamageSpeed = value; }
    }
    public static float m_fCharShootDelay
    {
        get { return GetTable().m_fCharShootDelay; }
        set { GetTable().m_fCharShootDelay = value; }
    }
    public static float m_fCharDashSpeed
    {
        get { return GetTable().m_fCharDashSpeed; }
        set { GetTable().m_fCharDashSpeed = value; }
    }
    public static float m_fCharAddDashPoint
    {
        get { return GetTable().m_fCharAddDashPoint; }
        set { GetTable().m_fCharAddDashPoint = value; }
    }
    public static float m_fCharDecDashPoint
    {
        get { return GetTable().m_fCharDecDashPoint; }
        set { GetTable().m_fCharDecDashPoint = value; }
    }
    public static float m_fCharMaxDashPoint
    {
        get { return GetTable().m_fCharMaxDashPoint; }
        set { GetTable().m_fCharMaxDashPoint = value; }
    }
    

    // 몬스터 관련
    public static float m_fMonMoveSpeed
    {
        get { return GetTable().m_fMonMoveSpeed; }
        set { GetTable().m_fMonMoveSpeed = value; }
    }
    public static float m_fMonDamageSpeed
    {
        get { return GetTable().m_fMonDamageSpeed; }
        set { GetTable().m_fMonDamageSpeed = value; }
    }
    public static float m_fMonGenDaly
    {
        get { return GetTable().m_fMonGenDaly; }
        set { GetTable().m_fMonGenDaly = value; }
    }
    public static int m_iMonMaxGen
    {
        get { return GetTable().m_iMonMaxGen; }
        set { GetTable().m_iMonMaxGen = value; }
    }
    public static int m_iMonMaxCount
    {
        get { return GetTable().m_iMonMaxCount; }
        set { GetTable().m_iMonMaxCount = value; }
    }


    // 이동 관련
    public static float m_fBasicMoveSpeed
    {
        get { return GetTable().m_fBasicMoveSpeed; }
        set { GetTable().m_fBasicMoveSpeed = value; }
    }
    public static float m_fMoveLimitX
    {
        get { return GetTable().m_fMoveLimitX; }
        set { GetTable().m_fMoveLimitX = value; }
    }
    public static float m_fMoveLimitY
    {
        get { return GetTable().m_fMoveLimitY; }
        set { GetTable().m_fMoveLimitY = value; }
    }


    // 기타
    public static float m_fUnitScale
    {
        get { return GetTable().m_fUnitScale; }
        set { GetTable().m_fUnitScale = value; }
    }
    public static int m_iFrameRate
    {
        get { return GetTable().m_iFrameRate; }
        set { GetTable().m_iFrameRate = value; }
    }
    
    // 유틸
    static JsonConstants GetTable()
    {
        var pTable = Single.Table.GetTable<JsonConstants>();

        if (false == pTable.IsLoadTable())
        {
#if UNITY_EDITOR
            pTable.LoadJson(pTable.m_strFileName);
#else
            pTable.LoadBytes(pTable.m_strByteFileName);
#endif
        }

        return Single.Table.GetTable<JsonConstants>();
    }
}
