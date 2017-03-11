using UnityEngine;
using System;
using System.Collections;

public partial class SHCharPopolo : SHState
{
    public enum eState
    {
        None,
        Idle,
        Move,
        Attack,
        Dash,
        Die,
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
            pState.m_OnFixedUpdate = OnFixedUpdateToDie;
        }

        ChangeState(eState.Idle);
    }
    #endregion
    

    #region State : Idle
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {
        SetLookRotation();

        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

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
        SetMove();
        SetLookRotation();

        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

        if (true == m_bIsShoot)
        {
            ChangeState(eState.Attack);
            return;
        }
    }
    #endregion


    #region State : Attack
    void OnEnterToAttack(int iBeforeState, int iCurrentState)
    {
        SetLookRotation();
        SetAttack();
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
        var bIsMove = SetMove();
        SetLookRotation();

        if (true == IsPossibleDash())
        {
            ChangeState(eState.Dash);
            return;
        }

        if (true == m_bIsShoot)
        {
            ChangeState(eState.Attack);
            return;
        }

        if (false == IsAnimPlaying(iCurrentState))
        {
            if (true == bIsMove)
                ChangeState(eState.Move);
            else
                ChangeState(eState.Idle);
            return;
        }
    }
    #endregion


    #region State : Dash
    void OnEnterToDash(int iBeforeState, int iCurrentState)
    {
        SetBodyDamageLock(true);
        Single.Sound.PlayEffect("Audio_Effect_Dash");
    }
    void OnExitToDash(int iBeforeState, int iCurrentState)
    {
        SetBodyDamageLock(false);
    }
    void OnFixedUpdateToDash(int iCurrentState, int iFixedTick)
    {
        SetDashMove();
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
    void OnEnterToDie(int iBeforeState, int iCurrentState)
    {
        DelBodyDamage();        
    }
    void OnFixedUpdateToDie(int iCurrentState, int iFixedTick)
    {
        if (100 < iFixedTick)
        {
            SetActive(false);
        }
    }
    #endregion
}