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
    public int m_iPhaseID = 0;
    #endregion
    
    #region Interface Functions
    public void SetUpdatePhase()
    {
        var pPhaseInfo        = Single.Table.GetPhaseInfo(Single.GameState.m_iScore);
        m_iPhaseID            = pPhaseInfo.m_iPhaseID;
        SHHard.m_fMonGenDaly  = pPhaseInfo.m_fMonGenDaly;
        SHHard.m_iMonMaxGen   = pPhaseInfo.m_iMonMaxGen;
        SHHard.m_iMonMaxCount = pPhaseInfo.m_iMonMaxCount;
    }
    public bool IsNextPhase()
    {
        var pPhaseInfo = Single.Table.GetPhaseInfo(Single.GameState.m_iScore);
        return (m_iPhaseID != pPhaseInfo.m_iPhaseID);
    }
    #endregion
}
