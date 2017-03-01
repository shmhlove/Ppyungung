using UnityEngine;
using System.Collections;

public static partial class SHHard
{
    // 캐릭터 관련
    public static float m_fCharMoveSpeed
    {
        get { return GetTable().m_fCharMoveSpeed; }
        set { GetTable().m_fCharMoveSpeed = value; }
    }
    public static float m_fCharAutoShoot
    {
        get { return GetTable().m_fCharAutoShoot; }
        set { GetTable().m_fCharAutoShoot = value; }
    }
    public static float m_fCharDashSpeed
    {
        get { return GetTable().m_fCharDashSpeed; }
        set { GetTable().m_fCharDashSpeed = value; }
    }
    public static float m_fCharDashTime
    {
        get { return GetTable().m_fCharDashTime; }
        set { GetTable().m_fCharDashTime = value; }
    }
    public static float m_fCharDashCool
    {
        get { return GetTable().m_fCharDashCool; }
        set { GetTable().m_fCharDashCool = value; }
    }

    // 몬스터 관련
    public static float m_fMonMoveSpeed
    {
        get { return GetTable().m_fMonMoveSpeed; }
        set { GetTable().m_fMonMoveSpeed = value; }
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
