using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHInGame_Component
{
    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void StartPlayer()
    {
        var pPopolo = Single.ObjectPool.Get<SHPopolo>("Popolo");
        pPopolo.SetActive(true);
        pPopolo.SetParent(SH3DRoot.GetRoot());
    }
    #endregion
}