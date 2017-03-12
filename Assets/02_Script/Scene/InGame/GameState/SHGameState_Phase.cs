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
    public bool IsNextPhase()
    {
        // 몬스터 잡은 마릿수와 현재 PhaseID로 판단
        return false;
    }
    #endregion
}
