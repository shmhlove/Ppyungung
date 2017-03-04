using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    public GameObject      m_pPlayerRoot = null;
    public SHCharPopolo    m_pCharacter  = null;
    #endregion


    #region Members : HardValue
    public bool            m_bIsAttacking = false;
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void StartPlayer()
    {
        m_pCharacter = Single.ObjectPool.Get<SHCharPopolo>("CharPopolo");
        m_pCharacter.SetActive(true);
        m_pCharacter.SetParent(GetRoot());
        m_pCharacter.SetLocalScale(m_pCharacter.m_vStartScale * SHHard.m_fUnitScale);
        m_pCharacter.StartCharacter();
    }
    public void StopPlayer()
    {
        if (null == m_pCharacter)
            return;

        m_pCharacter.StopCharacter();
    }
    public Vector3 GetPosition()
    {
        if (null == m_pCharacter)
            return Vector3.zero;

        return m_pCharacter.GetPosition();
    }
    public Vector3 GetLocalPosition()
    {
        if (null == m_pCharacter)
            return Vector3.zero;

        return m_pCharacter.GetLocalPosition();
    }
    public void LimiteInCamera()
    {
        if (null == m_pCharacter)
            return;

        m_pCharacter.LimitInCamera();
    }
    public bool IsDie()
    {
        if (null == m_pCharacter)
            return true;

        return m_pCharacter.IsDie();
    }
    #endregion


    #region Utility Functions
    GameObject GetRoot()
    {
        if (null == m_pPlayerRoot)
        {
            m_pPlayerRoot = SHGameObject.CreateEmptyObject("Player");
            m_pPlayerRoot.transform.SetParent(SH3DRoot.GetRoot());
        }

        return m_pPlayerRoot;
    }
    #endregion
}