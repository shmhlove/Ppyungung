using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHMonster : SHInGame_Component
{
    #region Members
    private List<SHMonPopolo> m_pObject = new List<SHMonPopolo>();
    #endregion
    

    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void StartMonster()
    {

    }
    #endregion


    #region Utility Functions
    void GenMonster()
    {
        // var pObject = Single.ObjectPool.Get<SHPopolo>("Popolo");
        // pObject.SetActive(true);
        // pObject.SetParent(SH3DRoot.GetRoot());
    }
    #endregion
}