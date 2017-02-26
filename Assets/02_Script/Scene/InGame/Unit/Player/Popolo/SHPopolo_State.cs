using UnityEngine;
using System;
using System.Collections;

public partial class SHPopolo : SHState
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
            pState.m_OnEnter       = OnEnterStateToAttack;
            pState.m_OnFixedUpdate = OnFixedUpdateToAttack;
        }
        
        ChangeState(eState.Idle);
    }
    #endregion
    

    #region State : Idle
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {
        SetLookDirection();

        if (true == m_bIsShoot)
        {
            ChangeState(eState.Attack);
            return;
        }
        
        if (Vector3.zero != m_vMoveDirection)
        {
            ChangeState(eState.Move);
            return;
        }
    }
    #endregion


    #region State : Move
    void OnFixedUpdateToMove(int iCurrentState, int iFixedTick)
    {
        bool bIsMoved  = SetMove();
        bool bIsLooked = SetLookDirection();

        if (true == m_bIsShoot)
        {
            ChangeState(eState.Attack);
            return;
        }

        if (false == bIsMoved)
        {
            ChangeState(eState.Idle);
            return;
        }
    }
    #endregion


    #region State : Attack
    void OnEnterStateToAttack(int iBeforeState, int iCurrentState)
    {
        SetAttack();
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
        bool bMoved  = SetMove();
        bool bLooked = SetLookDirection();

        if (true == m_bIsShoot)
        {
            ChangeState(eState.Attack);
            return;
        }

        if (false == IsAnimPlaying(iCurrentState))
        {
            if (true == bMoved)
                ChangeState(eState.Move);
            else
                ChangeState(eState.Idle);
        }
    }
    #endregion
}