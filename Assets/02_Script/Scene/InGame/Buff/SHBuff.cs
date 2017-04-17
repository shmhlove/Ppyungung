using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum eBuffType
{
    None,
    Buff_ResetHeath,         // 체력 충전
    Buff_UpgradeMaxHeath,    // 체력 최대량 증가
    Buff_UpgradeAddDP,       // 몬스터 처치 시 대시 게이지 회복량 증가
    Buff_UpgradeDecDP,       // 대시 게이지 소모량 감소
    Buff_UpgradeDropCoin,    // 드랍 코인 증가
    Buff_Shield,             // 보호막 생성
    Buff_UpgradeBulletSP,    // 총알 속도 증가
    Buff_UpgradeMoveSP,      // 이동 속도 증가
    Buff_DecreaseMonSP,      // 몬스터 이동 속도 감소
}

public class SHBuff : SHInGame_Component
{
    #region Members : 
    private eBuffType m_eSelectBuff = eBuffType.None;
    #endregion


    #region Members BuffValue
    public float m_fMaxHeath      = 0.0f;
    public float m_fAddDP         = 0.0f;
    public float m_fDecDP         = 0.0f;
    public float m_fDropCoin      = 0.0f;
    public float m_fBulletSP      = 0.0f;
    public float m_fMoveSP        = 0.0f;
    public float m_fDecreaseMonSP = 0.0f;
    #endregion


    #region Interface Functions
    public void ClearBuffValue()
    {
        m_fMaxHeath      = 0.0f;
        m_fAddDP         = 0.0f;
        m_fDecDP         = 0.0f;
        m_fDropCoin      = 0.0f;
        m_fBulletSP      = 0.0f;
        m_fMoveSP        = 0.0f;
        m_fDecreaseMonSP = 0.0f;
    }
    public void SetBuff(eBuffType eType)
    {
        m_eSelectBuff = eType;
    }
    public void ApplyBuff()
    {
        if (eBuffType.None == m_eSelectBuff)
            return;

        OnApplyBuff(m_eSelectBuff);
        m_eSelectBuff = eBuffType.None;
    }
    public List<eBuffType> GetRandomBuffList(int iCount)
    {
        var pBuffAll = GetAllBuffList();
        {
            SHUtils.For(0, pBuffAll.Count, (iIndex) =>
            {
                var iRandValue = SHMath.Random(0, pBuffAll.Count);
                var pTempValue = pBuffAll[iIndex];
                pBuffAll[iIndex] = pBuffAll[iRandValue];
                pBuffAll[iRandValue] = pTempValue;
            });
        }
        return pBuffAll.GetRange(0, iCount);
    }
    public List<eBuffType> GetAllBuffList()
    {
        List<eBuffType> pResult = new List<eBuffType>();
        SHUtils.ForToEnum<eBuffType>((eType) => 
        {
            if (eBuffType.None == eType)
                return;

            pResult.Add(eType);
        });
        return pResult;
    }
    #endregion


    #region Utility Functions
    void OnApplyBuff(eBuffType eType)
    {
        switch (eType)
        {
            case eBuffType.Buff_ResetHeath:
                Single.Player.ResetHP();
                break;
            case eBuffType.Buff_UpgradeMaxHeath:
                m_fMaxHeath += SHHard.m_iCharMaxHealthPoint *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioUpgradeMaxHeath;
                break;
            case eBuffType.Buff_UpgradeAddDP:
                m_fAddDP += SHHard.m_fCharAddDashPoint *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioUpgradeAddDP;
                break;
            case eBuffType.Buff_UpgradeDecDP:
                m_fDecDP += SHHard.m_fCharMaxDashPoint *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioUpgradeDecDP;
                break;
            case eBuffType.Buff_UpgradeDropCoin:
                break;
            case eBuffType.Buff_Shield:
                var pDamage = Single.Player.AddShieldDamage();
                pDamage.m_pInfo.m_iDamageHP = Single.Table.GetTable<JsonBuffInfo>().m_iShieldCount;
                break;
            case eBuffType.Buff_UpgradeBulletSP:
                m_fBulletSP += SHHard.m_fCharDamageSpeed *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioUpgradeBulletSP;
                break;
            case eBuffType.Buff_UpgradeMoveSP:
                m_fMoveSP += SHHard.m_fCharMoveSpeed *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioUpgradeMoveSP;
                break;
            case eBuffType.Buff_DecreaseMonSP:
                m_fDecreaseMonSP += SHHard.m_fMonMoveSpeed *
                    Single.Table.GetTable<JsonBuffInfo>().m_fRatioDecreaseMonSP;
                break;
        }
    }
    #endregion
}