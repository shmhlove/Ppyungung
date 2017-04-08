using UnityEngine;
using System;
using System.Collections;

public class SHInGame : SHSingleton<SHInGame>
{
    #region Members
    private SHGameStep   m_pGameStep   = new SHGameStep();
    private SHGameState  m_pGameState  = new SHGameState();
    private SHBalance    m_pBalance    = new SHBalance();
    private SHPlayer     m_pPlayer     = new SHPlayer();
    private SHMonster    m_pMonster    = new SHMonster();
    private SHDamage     m_pDamage     = new SHDamage();
    private SHBackGround m_pBackground = new SHBackGround();
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        if (null != m_pGameStep)
            m_pGameStep.OnFinalize();

        if (null != m_pGameState)
            m_pGameState.OnFinalize();
        
        if (null != m_pBalance)
            m_pBalance.OnFinalize();

        if (null != m_pPlayer)
            m_pPlayer.OnFinalize();

        if (null != m_pMonster)
            m_pMonster.OnFinalize();

        if (null != m_pDamage)
            m_pDamage.OnFinalize();

        if (null != m_pBackground)
            m_pBackground.OnFinalize();
    }
    #endregion


    #region System Functions
    #endregion


    #region Interface : System
    public void StartInGame()
    {
        if (null != m_pGameStep)
            m_pGameStep.OnInitialize();
        
        if (null != m_pGameState)
            m_pGameState.OnInitialize();
        
        if (null != m_pBalance)
            m_pBalance.OnInitialize();

        if (null != m_pPlayer)
            m_pPlayer.OnInitialize();

        if (null != m_pMonster)
            m_pMonster.OnInitialize();
        
        if (null != m_pDamage)
            m_pDamage.OnInitialize();

        if (null != m_pBackground)
            m_pBackground.OnInitialize();
    }
    public void PauseInGame(bool bIsPause)
    {
        SetPause(bIsPause);

        if (null != m_pGameStep)
            m_pGameStep.SetPause(bIsPause);
        
        if (null != m_pGameState)
            m_pGameState.SetPause(bIsPause);
        
        if (null != m_pBalance)
            m_pBalance.SetPause(bIsPause);

        if (null != m_pPlayer)
            m_pPlayer.SetPause(bIsPause);

        if (null != m_pMonster)
            m_pMonster.SetPause(bIsPause);

        if (null != m_pDamage)
            m_pDamage.SetPause(bIsPause);

        if (null != m_pBackground)
            m_pBackground.SetPause(bIsPause);
    }
    public void FrameMove()
    {
        if (true == m_bIsPause)
            return;

        if (null != m_pGameStep)
            m_pGameStep.OnFrameMove();
        
        if (null != m_pGameState)
            m_pGameState.OnFrameMove();
        
        if (null != m_pBalance)
            m_pBalance.OnFrameMove();

        if (null != m_pPlayer)
            m_pPlayer.OnFrameMove();

        if (null != m_pMonster)
            m_pMonster.OnFrameMove();

        if (null != m_pDamage)
            m_pDamage.OnFrameMove();

        if (null != m_pBackground)
            m_pBackground.OnFrameMove();
    }
    #endregion


    #region Interface : Helpper
    public SHGameStep GetGameStep()
    {
        return m_pGameStep;
    }
    public SHGameState GetGameState()
    {
        return m_pGameState;
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
    public SHBackGround GetBackground()
    {
        return m_pBackground;
    }
    #endregion


    #region Test Functions
    public GameObject   m_pTestDamageTarget = null;
    public string       m_strTestDamageName = string.Empty;
    [FuncButton] public void TestAddDamage()
    {
        Single.Damage.AddDamage(m_strTestDamageName,
            new SHDamageParam(this, m_pTestDamageTarget));
    }
    #endregion
}
