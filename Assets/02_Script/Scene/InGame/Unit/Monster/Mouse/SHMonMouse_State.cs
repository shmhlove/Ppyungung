using UnityEngine;
using System;
using System.Collections;

public partial class SHMonMouse : SHState
{
    public enum eState
    {
        None,
        Idle,
        Move,
        Attack,
        Die,
    }


    #region State : Register
    public override void RegisterState()
    {
        var pState = CreateState(eState.Idle);
        {
            pState.m_strAnimClip   = "Anim_Char_Idle";
            pState.m_OnEnter       = OnEnterToIdle;
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
            pState.m_OnEndAnim     = OnEndAnimToAttack;
            pState.m_OnFixedUpdate = OnFixedUpdateToAttack;
        }

        pState = CreateState(eState.Die);
        {
            pState.m_strAnimClip   = "Anim_Char_Attack";
            pState.m_OnEnter       = OnEnterToDie;
            pState.m_OnEndAnim     = OnEndAnimToDie;
            pState.m_OnFixedUpdate = OnFixedUpdateToDie;
        }

        ChangeState(eState.Idle);
    }
    #endregion


    #region State : Idle
    void OnEnterToIdle(int iBeforeState, int iCurrentState)
    {
        m_iAttackCount = 0;
    }
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {
        if (false == IsAttackDelay())
            ChangeState(eState.Attack);
        else
            ChangeState(eState.Move);
    }
    #endregion


    #region State : Move
    void OnFixedUpdateToMove(int iCurrentState, int iFixedTick)
    {
        SetLookPC();
        SetMove();

        if (false == IsAttackDelay())
            ChangeState(eState.Attack);
    }
    #endregion


    #region State : Attack
    void OnEnterToAttack(int iBeforeState, int iCurrentState)
    {
        SetAttack();
    }
    void OnEndAnimToAttack(int iCurrentState)
    {
        if (MAX_ATTACK_COUNT > ++m_iAttackCount)
        {
            ChangeState(eState.Attack);
        }
        else
        {
            Single.Timer.StartDeltaTime(GetAttackKey());
            ChangeState(eState.Idle);
        }
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
        SetLookPC();
    }
    #endregion


    #region State : Die
    void OnEnterToDie(int iBeforeState, int iCurrentState)
    {
    }
    void OnEndAnimToDie(int iCurrentState)
    {
        Single.Monster.DeleteMonster(this);
        SetActive(false);
    }
    void OnFixedUpdateToDie(int iCurrentState, int iFixedTick)
    {
    }
    #endregion
}