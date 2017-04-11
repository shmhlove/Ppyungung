using UnityEngine;
using System;
using System.Collections;

public partial class SHMonUsainBolt : SHBaseMonster
{
    public enum eState
    {
        Move,
        Die,
        Kill,
    }


    #region Override Functions
    public override string GetState()
    {
        return ((eState)m_iCurrentStateID).ToString();
    }
    public override int GetDieState()
    {
        return (int)eState.Die;
    }
    public override int GetKillState()
    {
        return (int)eState.Kill;
    }
    public override void RegisterState()
    {
        var pState = CreateState(eState.Move);
        {
            pState.m_strAnimClip   = "Anim_Mon_Mouse_Move";
            pState.m_OnFixedUpdate = OnFixedUpdateToMove;
        }

        pState = CreateState(eState.Kill);
        {
            pState.m_strAnimClip   = "Anim_Mon_Mouse_Die";
            pState.m_OnFixedUpdate = OnFixedUpdateToKill;
        }

        pState = CreateState(eState.Die);
        {
            pState.m_strAnimClip   = "Anim_Mon_Mouse_Die";
            pState.m_OnFixedUpdate = OnFixedUpdateToDie;
        }
    }
    #endregion


    #region Utility : Move State
    void OnFixedUpdateToMove(int iFixedTick)
    {
        SetMove();
    }
    #endregion


    #region Utility : Die State
    void OnFixedUpdateToDie(int iFixedTick)
    {
        if (25 > iFixedTick)
            return;

        SetExplosionDie();
    }
    #endregion


    #region Utility : Kill State
    void OnFixedUpdateToKill(int iFixedTick)
    {
        SetActive(false);
        Single.Monster.DeleteMonster(this);
    }
    #endregion
}
