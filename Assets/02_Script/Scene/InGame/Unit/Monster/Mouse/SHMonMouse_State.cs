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
        if (false == IsAttackDelay())
            StartCoroutine(COROUTINE_ATTACK);
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
            StartCoroutine(COROUTINE_ATTACK);
    }
    #endregion


    #region State : Attack
    private string COROUTINE_ATTACK = "CoroutineToAttackDelay";
    IEnumerator CoroutineToAttackDelay()
    {
        for(int iLoop = 0; iLoop < MAX_ATTACK_COUNT; ++iLoop)
        {
            ChangeState(eState.Attack);

            yield return new WaitForSeconds(SHHard.m_fMonShootDelay);
        }

        Single.Timer.StartDeltaTime(GetAttackKey());
        ChangeState(eState.Idle);
    }
    void OnEnterToAttack(int iBeforeState, int iCurrentState)
    {
        SetAttack(Vector3.zero);
    }
    void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick)
    {
        SetLookPC();
        SetMove();
    }
    #endregion


    #region State : Die
    void OnEnterToDie(int iBeforeState, int iCurrentState)
    {
        StopCoroutine(COROUTINE_ATTACK);
        Single.Monster.DeleteMonster(this);
        Single.Damage.DelDamage(m_pMonDamage);
        m_pMonDamage = null;

        PlayParticle("Particle_Crash_Dust_Big");
    }
    void OnFixedUpdateToDie(int iCurrentState, int iFixedTick)
    {
        if (100 < iFixedTick)
        {
            SHUtils.For(0, 360, 36, (iValue) =>
            {
                var vDirection = Vector3.zero;
                vDirection.x = Mathf.Cos(iValue * Mathf.Deg2Rad);
                vDirection.z = Mathf.Sin(iValue * Mathf.Deg2Rad);
                SetAttack(vDirection);
            });

            SetActive(false);
        }
    }
    #endregion
}