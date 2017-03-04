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
    public static float m_fMonDamageSpeed
    {
        get { return GetTable().m_fMonDamageSpeed; }
        set { GetTable().m_fMonDamageSpeed = value; }
    }
    public static float m_fMonAttackDelay
    {
        get { return GetTable().m_fMonAttackDelay; }
        set { GetTable().m_fMonAttackDelay = value; }
    }
    public static float m_fMonShootDelay
    {
        get { return GetTable().m_fMonShootDelay; }
        set { GetTable().m_fMonShootDelay = value; }
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

    // 기타
    public static float m_fUnitScale
    {
        get { return GetTable().m_fUnitScale; }
        set { GetTable().m_fUnitScale = value; }
    }
    public static float m_fCameraMoveSpeed
    {
        get { return GetTable().m_fCameraMoveSpeed; }
        set { GetTable().m_fCameraMoveSpeed = value; }
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
