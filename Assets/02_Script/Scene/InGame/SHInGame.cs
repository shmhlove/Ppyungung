using UnityEngine;
using System;
using System.Collections;

public class SHInGame : SHSingleton<SHInGame>
{
    #region Members
    private SHStep       m_pStep       = new SHStep();
    private SHScoreBoard m_pScoreBoard = new SHScoreBoard();
    private SHBalance    m_pBalance    = new SHBalance();
    private SHPlayer     m_pPlayer     = new SHPlayer();
    private SHDamage     m_pDamage     = new SHDamage();
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        FinalizeEngine();
    }
    #endregion


    #region System Functions
    #endregion


    #region Interface : System
    public void StartGame()
    {
        if (null != m_pStep)
            m_pStep.OnInitialize();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnInitialize();
        
        if (null != m_pBalance)
            m_pBalance.OnInitialize();

        if (null != m_pPlayer)
            m_pPlayer.OnInitialize();

        if (null != m_pDamage)
            m_pDamage.OnInitialize();
    }
    public void FinalizeEngine()
    {
        if (null != m_pStep)
            m_pStep.OnFinalize();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnFinalize();
        
        if (null != m_pBalance)
            m_pBalance.OnFinalize();

        if (null != m_pPlayer)
            m_pPlayer.OnFinalize();

        if (null != m_pDamage)
            m_pDamage.OnFinalize();
    }
    public void FrameMove()
    {
        if (null != m_pStep)
            m_pStep.OnFrameMove();

        if (null != m_pScoreBoard)
            m_pScoreBoard.OnFrameMove();
        
        if (null != m_pBalance)
            m_pBalance.OnFrameMove();

        if (null != m_pPlayer)
            m_pPlayer.OnFrameMove();

        if (null != m_pDamage)
            m_pDamage.OnFrameMove();
    }
    #endregion


    #region Interface : Helpper
    public SHStep GetStep()
    {
        return m_pStep;
    }
    public SHScoreBoard GetScoreBoard()
    {
        return m_pScoreBoard;
    }
    public SHBalance GetBalance()
    {
        return m_pBalance;
    }
    public SHPlayer GetPlayer()
    {
        return m_pPlayer;
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
