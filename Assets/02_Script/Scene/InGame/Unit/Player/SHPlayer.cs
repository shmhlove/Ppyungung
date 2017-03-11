using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    public SHCharPopolo    m_pCharacter       = null;
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        if (null == m_pCharacter)
            return;

        m_pCharacter.FrameMove();
    }
    public override void SetPause(bool bIsPause)
    {
        base.SetPause(bIsPause);

        if (null == m_pCharacter)
            return;

        m_pCharacter.SetPauseAnimation(bIsPause);
    }
    #endregion


    #region Interface Functions
    public void StartPlayer()
    {
        DestoryCharacter();
        m_pCharacter = Single.ObjectPool.Get<SHCharPopolo>("CharPopolo");
        m_pCharacter.SetActive(true);
        m_pCharacter.SetParent(SH3DRoot.GetRootToPlayer());
        m_pCharacter.SetLocalScale(m_pCharacter.m_vStartScale * SHHard.m_fUnitScale);
        m_pCharacter.OnInitialize();
        m_pCharacter.StartCharacter();
    }
    public void StopPlayer()
    {
        if (null == m_pCharacter)
            return;

        m_pCharacter.StopCharacter();
    }
    public Vector3 GetLocalPosition()
    {
        if (null == m_pCharacter)
            return Vector3.zero;

        return m_pCharacter.GetLocalPosition();
    }
    public float GetHPPercent()
    {
        return 100.0f;
    }
    public float GetDashPercent()
    {
        if (null == m_pCharacter)
            return 0.0f;

        return SHMath.Divide(m_pCharacter.m_fDashGauge, SHHard.m_fCharMaxDashGauge) * 100.0f;
    }
    public bool IsDie()
    {
        if (null == m_pCharacter)
            return true;

        return m_pCharacter.IsDie();
    }
    #endregion


    #region Utility Functions
    void DestoryCharacter()
    {
        if (null == m_pCharacter)
            return;

        m_pCharacter.Destory();
    }
    #endregion
}