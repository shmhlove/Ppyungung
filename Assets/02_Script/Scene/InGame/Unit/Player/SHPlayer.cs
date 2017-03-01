﻿using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    private GameObject      m_pPlayerRoot = null;
    private SHCharPopolo    m_pCharacter  = null;
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