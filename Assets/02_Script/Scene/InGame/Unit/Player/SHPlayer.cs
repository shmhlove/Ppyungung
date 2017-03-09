using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    public SHCharPopolo    m_pCharacter  = null;
    #endregion


    #region Members : HardValue
    public bool            m_bIsAutoAttacking = false;
    public bool            m_bIsMoving        = false;
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
    #endregion


    #region Interface Functions
    public void StartPlayer()
    {
        m_pCharacter = Single.ObjectPool.Get<SHCharPopolo>("CharPopolo");
        m_pCharacter.SetActive(true);
        m_pCharacter.SetParent(SH3DRoot.GetRootToPlayer());
        m_pCharacter.SetLocalScale(m_pCharacter.m_vStartScale * SHHard.m_fUnitScale);
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
        return 100.0f;
    }
    public bool IsDie()
    {
        if (null == m_pCharacter)
            return true;

        return m_pCharacter.IsDie();
    }
    #endregion
}