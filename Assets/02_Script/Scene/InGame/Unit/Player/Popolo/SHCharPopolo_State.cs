using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
{
    public enum eState
    {
        Idle,
        Move,
        Attack,
        Dash,
        Die,
    }


    #region Override Functions
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
            pState.m_OnEnter       = OnEnterAttack;
            pState.m_OnFixedUpdate = OnFixedUpdateToAttack;
        }
        
        pState = CreateState(eState.Dash);
        {
            pState.m_strAnimClip   = "Anim_Char_Move";
            pState.m_OnEnter       = OnEnterToDash;
            pState.m_OnExit        = OnExitToDash;
            pState.m_OnFixedUpdate = OnFixedUpdateToDash;
        }

        pState = CreateState(eState.Die);
        {
            pState.m_strAnimClip   = "Anim_Char_Die";
            pState.m_OnEnter       = OnEnterToDie;
        }

        ChangeState(eState.Idle);
    }
    #endregion
    

    #region State : Idle
    void OnFixedUpdateToIdle(int iFixedTick)
    {
        SetLookRotation();

        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

        if (true == IsPossibleAttack())
        {
            ChangeState(eState.Attack);
            return;
        }

        if (true == IsPossibleMove())
        {
            ChangeState(eState.Move);
            return;
        }
    }
    #endregion


    #region State : Move
    void OnFixedUpdateToMove(int iFixedTick)
    {
        SetMove(SHHard.m_fCharMoveSpeed);
        SetLookRotation();
        
        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

        if (true == IsPossibleAttack())
        {
            ChangeState(eState.Attack);
            return;
        }

        if (false == IsPossibleMove())
        {
            ChangeState(eState.Idle);
            return;
        }
    }
    #endregion


    #region State : Attack
    void OnEnterAttack(int iBeforeState)
    {
        StartCoroutine(CoroutineToAttack());
    }
    void OnFixedUpdateToAttack(int iFixedTick)
    {
        SetMove(SHHard.m_fCharMoveSpeed);
        LookAttackDirection();

        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

        //if (false == IsAnimPlaying(m_iCurrentStateID))
        if (false == IsPossibleAttack())
        {
            if (true == IsPossibleMove())
                ChangeState(eState.Move);
            else
                ChangeState(eState.Idle);
            return;
        }
    }
    #endregion


    #region State : Dash
    void OnEnterToDash(int iBeforeState)
    {
        SetBodyDamageLock(true);
        Single.Sound.PlayEffect("Audio_Effect_Dash");
    }
    void OnExitToDash(int iBeforeState)
    {
        SetBodyDamageLock(false);
    }
    void OnFixedUpdateToDash(int iFixedTick)
    {
        SetMove(SHHard.m_fCharDashSpeed);
        SetLookRotation();
        DecreaseDashGauge();

        if (false == IsPossibleDash())
        {
            ChangeState(eState.Move);
            return;
        }
    }
    #endregion


    #region State : Die
    void OnEnterToDie(int iBeforeState)
    {
        DelBodyDamage();
        InitControlValue();
    }
    #endregion
}