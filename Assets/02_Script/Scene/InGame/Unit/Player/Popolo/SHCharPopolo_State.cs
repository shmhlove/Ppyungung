﻿using UnityEngine;
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

        if (true == m_bIsDash)
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
        bool bIsMoved  = SetMove();
        SetLookRotation();

        if (true == m_bIsDash)
        {
            ChangeState(eState.Dash);
            return;
        }

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
    private bool m_bAttackStateLockRotation = false;
    void OnEnterToAttack(int iBeforeState, int iCurrentState)
    {
        m_bAttackStateLockRotation = false;
        {
            var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
            if ((null != pCtrlUI) &&
                (eControlType.Type_2 != pCtrlUI.m_eCtrlType))
            {
                SetLookNearMonster();
                m_bAttackStateLockRotation = true;
            }
        }

        SetAttack();
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
        bool bMoved  = SetMove();

        if (false == m_bAttackStateLockRotation)
            SetLookRotation();

        if (true == m_bIsDash)
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
            if (true == bMoved)
                ChangeState(eState.Move);
            else
                ChangeState(eState.Idle);
        }
    }
    #endregion


    #region State : Dash
    void OnEnterToDash(int iBeforeState, int iCurrentState)
    {
        if (Vector3.zero == m_vDashDirection)
            m_vDashDirection = Vector3.forward;

        if (null != m_pCharDamage)
            m_pCharDamage.m_bIsCrashLock = true;

        Single.Sound.PlayEffect("Audio_Effect_Dash");
    }
    void OnExitToDash(int iBeforeState, int iCurrentState)
    {
        if (null != m_pCharDamage)
            m_pCharDamage.m_bIsCrashLock = false;
    }
    void OnFixedUpdateToDash(int iCurrentState, int iFixedTick)
    {
        SetDash();

        if (false == m_bIsDash)
        {
            ChangeState(eState.Idle);
            return;
        }
    }
    #endregion


    #region State : Die
    void OnEnterToDie(int iBeforeState, int iCurrentState)
    {
        Single.Damage.DelDamage(m_pCharDamage);
        m_pCharDamage = null;
        
        PlayParticle("Particle_Crash_Dust_Big");
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