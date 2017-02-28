using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Members
    private SHPopolo m_pObject = null;
    #endregion
    

    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void StartPlayer()
    {
        m_pObject = Single.ObjectPool.Get<SHPopolo>("Popolo");
        m_pObject.SetActive(true);
        m_pObject.SetParent(SH3DRoot.GetRoot());
    }
    public Vector3 GetPosition()
    {
        if (null == m_pObject)
            return Vector3.zero;

        return m_pObject.GetPosition();
    }
    public Vector3 GetLocalPosition()
    {
        if (null == m_pObject)
            return Vector3.zero;

        return m_pObject.GetLocalPosition();
    }
    #endregion
}