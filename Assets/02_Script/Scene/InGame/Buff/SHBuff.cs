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
    #region Members
    private eBuffType m_eSelectBuff = eBuffType.None;
    #endregion


    #region Interface Functions
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
                SHHard.m_iCharMaxHealthPoint += SHHard.m_iCharMaxHealthPoint * 0.5f;
                break;
            case eBuffType.Buff_UpgradeAddDP:
                SHHard.m_fCharAddDashPoint += SHHard.m_fCharAddDashPoint * 0.5f;
                break;
            case eBuffType.Buff_UpgradeDecDP:
                SHHard.m_fCharMaxDashPoint += SHHard.m_fCharMaxDashPoint * 0.5f;
                break;
            case eBuffType.Buff_UpgradeDropCoin:
                break;
            case eBuffType.Buff_Shield:
                break;
            case eBuffType.Buff_UpgradeBulletSP:
                SHHard.m_fCharDamageSpeed += SHHard.m_fCharDamageSpeed * 0.5f;
                break;
            case eBuffType.Buff_UpgradeMoveSP:
                SHHard.m_fCharMoveSpeed += SHHard.m_fCharMoveSpeed * 0.5f;
                break;
            case eBuffType.Buff_DecreaseMonSP:
                SHHard.m_fMonMoveSpeed -= SHHard.m_fMonMoveSpeed * 0.5f;
                break;
        }
    }
    #endregion
}