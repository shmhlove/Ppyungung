using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* partial Information
 * 
 * SHGameState
 * SHGameState_ScoreBoard.cs
 * SHGameState_Phase.cs
 * 
*/

public partial class SHGameState : SHInGame_Component
{
    #region Members
    public int m_iCurrentPhaseID    = -1;
    #endregion
    

    #region Interface Functions
    public void InitPhase()
    {
        m_iCurrentPhaseID = -1;
    }
    public void SetNextPhase()
    {
        var pPhaseInfo        = Single.Table.GetPhaseInfo(m_iCurrentPhaseID += 1);
        m_iCurrentPhaseID     = pPhaseInfo.m_iPhaseID;
        SHHard.m_fMonGenDaly  = pPhaseInfo.m_fMonGenDaly;
        SHHard.m_iMonMaxGen   = pPhaseInfo.m_iMonMaxGen;
        SHHard.m_iMonMaxCount = pPhaseInfo.m_iMonMaxCount;
    }
    public bool IsPossibleNextPhase()
    {
        var pPhaseInfo = Single.Table.GetPhaseInfo(m_iCurrentPhaseID + 1);
        return (pPhaseInfo.m_iPhaseCount <= Single.GameState.GetCurrentKillCount());
    }
    public SHPhaseInfo GetCurrentPhaseInfo()
    {
        return Single.Table.GetPhaseInfo(m_iCurrentPhaseID);
    }
    public SHPhaseInfo GetNextPhaseInfo()
    {
        return Single.Table.GetPhaseInfo(m_iCurrentPhaseID + 1);
    }
    #endregion
}
