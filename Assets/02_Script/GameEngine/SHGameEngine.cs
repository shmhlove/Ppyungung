using UnityEngine;
using System;
using System.Collections;

public class SHGameEngine : SHSingleton<SHGameEngine>
{
    #region Members
    private SHGameStep   m_pGameStep   = new SHGameStep();
    private SHScoreBoard m_pScoreBoard = new SHScoreBoard();
    private SHInventory  m_pInventory  = new SHInventory();
    private SHBalance    m_pBalance    = new SHBalance();
    private SHPlayer     m_pPlayer     = new SHPlayer();
    private SHMonster    m_pMonster    = new SHMonster();
    private SHDamage     m_pDamage     = new SHDamage();
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        if (null != m_pGameStep)
            m_pGameStep.OnFinalize();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnFinalize();

        if (null != m_pInventory)
            m_pInventory.OnFinalize();

        if (null != m_pBalance)
            m_pBalance.OnFinalize();
        
        if (null != m_pPlayer)
            m_pPlayer.OnFinalize();

        if (null != m_pMonster)
            m_pMonster.OnFinalize();

        if (null != m_pDamage)
            m_pDamage.OnFinalize();
    }
    #endregion


    #region System Functions
    #endregion


    #region Interface : System
    public void StartEngine()
    {
        if (null != m_pGameStep)
            m_pGameStep.OnInitialize();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnInitialize();

        if (null != m_pInventory)
            m_pInventory.OnInitialize();

        if (null != m_pBalance)
            m_pBalance.OnInitialize();

        if (null != m_pPlayer)
            m_pPlayer.OnInitialize();

        if (null != m_pMonster)
            m_pMonster.OnInitialize();

        if (null != m_pDamage)
            m_pDamage.OnInitialize();
    }
    public void FrameMove()
    {
        if (null != m_pGameStep)
            m_pGameStep.OnFrameMove();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnFrameMove();

        if (null != m_pInventory)
            m_pInventory.OnFrameMove();

        if (null != m_pBalance)
            m_pBalance.OnFrameMove();

        if (null != m_pPlayer)
            m_pPlayer.OnFrameMove();

        if (null != m_pMonster)
            m_pMonster.OnFrameMove();

        if (null != m_pDamage)
            m_pDamage.OnFrameMove();
    }
    #endregion


    #region Interface : Helpper
    public SHGameStep GetGameStep()
    {
        return m_pGameStep;
    }
    public SHScoreBoard GetScoreBoard()
    {
        return m_pScoreBoard;
    }
    public SHInventory GetInventory()
    {
        return m_pInventory;
    }
    public SHBalance GetBalance()
    {
        return m_pBalance;
    }
    public SHPlayer GetPlayer()
    {
        return m_pPlayer;
    }
    public SHMonster GetMonster()
    {
        return m_pMonster;
    }
    public SHDamage GetDamage()
    {
        return m_pDamage;
    }
    #endregion


    #region Test Functions
    public GameObject   m_pTestDamageTarget = null;
    public string       m_strTestDamageName = string.Empty;
    [FuncButton] public void TestAddDamage()
    {
        Single.Damage.AddDamage(m_strTestDamageName,
            new SHAddDamageParam(this, m_pTestDamageTarget, null, null));
    }
    #endregion
}
