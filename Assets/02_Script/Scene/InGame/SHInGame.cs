using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicComponent = System.Collections.Generic.Dictionary<System.Type, SHInGame_Component>;

public class SHInGame : SHSingleton<SHInGame>
{
    #region Members
    private DicComponent m_dicComponent = new DicComponent();
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        m_dicComponent.Clear();
        m_dicComponent.Add(typeof(SHGameStep),   new SHGameStep());
        m_dicComponent.Add(typeof(SHGameState),  new SHGameState());
        m_dicComponent.Add(typeof(SHBalance),    new SHBalance());
        m_dicComponent.Add(typeof(SHPlayer),     new SHPlayer());
        m_dicComponent.Add(typeof(SHMonster),    new SHMonster());
        m_dicComponent.Add(typeof(SHDamage),     new SHDamage());
        m_dicComponent.Add(typeof(SHBackGround), new SHBackGround());
        m_dicComponent.Add(typeof(SHBuff),       new SHBuff());
        m_dicComponent.Add(typeof(SHDropItem),   new SHDropItem());
    }
    public override void OnFinalize()
    {
        SHUtils.ForToDic(m_dicComponent, (pKey, pValue) => pValue.OnFinalize());
    }
    #endregion
    

    #region Interface : System
    public void StartInGame()
    {
        SHUtils.ForToDic(m_dicComponent, (pKey, pValue) => pValue.OnInitialize());
    }
    public void PauseInGame(bool bIsPause)
    {
        SetPause(bIsPause);
        SHUtils.ForToDic(m_dicComponent, (pKey, pValue) => pValue.SetPause(bIsPause));
    }
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        SHUtils.ForToDic(m_dicComponent, (pKey, pValue) => pValue.OnFrameMove());
    }
    #endregion


    #region Interface : Helpper
    public T GetIngameComponent<T>() where T : SHInGame_Component
    {
        var pType = typeof(T);
        if (false == m_dicComponent.ContainsKey(pType))
            return default(T);

        return m_dicComponent[pType] as T;
    }
    #endregion


    #region Test Functions
    public GameObject   m_pTestDamageTarget = null;
    public string       m_strTestDamageName = string.Empty;
    [FuncButton] public void TestAddDamage()
    {
        Single.Damage.AddDamage(m_strTestDamageName,
            new SHDamageParam(this, GetPosition(), m_pTestDamageTarget));
    }
    #endregion
}
