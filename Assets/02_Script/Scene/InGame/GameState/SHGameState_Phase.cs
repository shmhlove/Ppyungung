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
    public void SetUpdatePhaseID()
    {
        var pPhaseInfo = Single.Table.GetPhaseInfo(Single.GameState.m_iScore);
        m_iPhaseID     = pPhaseInfo.m_iPhaseID;
    }
    public bool IsNextPhase()
    {
        var pPhaseInfo = Single.Table.GetPhaseInfo(Single.GameState.m_iScore);
        return (m_iPhaseID != pPhaseInfo.m_iPhaseID);
    }
    #endregion
}
