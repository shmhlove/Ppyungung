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
    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void Clear()
    {
        ClearScoreBoard();
    }
    #endregion
}
