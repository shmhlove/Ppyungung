using UnityEngine;
using System;
using System.Collections;

public partial class SHMonPopolo : SHState
{
    public enum eState
    {
        None,
        Idle,
        Move,
        Attack,
    }


    #region State : Register
    public override void RegisterState()
    {
        var pState = CreateState(eState.Idle);
        {
            pState.m_strAnimClip   = "Anim_Char_Idle";
            pState.m_OnFixedUpdate = OnFixedUpdateToIdle;
        }

        pState = CreateState(eState.Move);
        {
            pState.m_strAnimClip   = "Anim_Char_Move";
            pState.m_OnFixedUpdate = OnFixedUpdateToMove;
        }

        pState = CreateState(eState.Attack);
        {
            pState.m_strAnimClip   = "Anim_Char_Attack";
            pState.m_OnEnter       = OnEnterToAttack;
            pState.m_OnFixedUpdate = OnFixedUpdateToAttack;
        }

        ChangeState(eState.Idle);
    }
    #endregion
    

    #region State : Idle
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {
    }
    #endregion


    #region State : Move
    void OnFixedUpdateToMove(int iCurrentState, int iFixedTick)
    {
    }
    #endregion


    #region State : Attack
    void OnEnterToAttack(int iBeforeState, int iCurrentState)
    {
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
    }
    #endregion
}