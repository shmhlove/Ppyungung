using UnityEngine;
using System;
using System.Collections;

public partial class SHMonMouse : SHState
{
    public enum eState
    {
        Move,
        Die,
        Kill,
    }


    #region State : Register
    public override void RegisterState()
    {
        var pState = CreateState(eState.Move);
        {
            pState.m_strAnimClip   = "Anim_Char_Move";
            pState.m_OnFixedUpdate = OnFixedUpdateToMove;
        }
        
        pState = CreateState(eState.Kill);
        {
            pState.m_strAnimClip    = "Anim_Char_Die";
            pState.m_OnEnter        = OnEnterToKill;
        }

        pState = CreateState(eState.Die);
        {
            pState.m_strAnimClip   = "Anim_Char_Die";
            pState.m_OnEnter       = OnEnterToDie;
            pState.m_OnFixedUpdate = OnFixedUpdateToDie;
        }

        ChangeState(eState.Move);
    }
    public override int GetKillState()
    {
        return (int)eState.Kill;
    }
    #endregion


    #region State : Move
    void OnFixedUpdateToMove(int iCurrentState, int iFixedTick)
    {
        SetMove();
    }
    #endregion


    #region State : Die
    void OnEnterToDie(int iBeforeState, int iCurrentState)
    {
        PlayParticle("Particle_Crash_Dust_Big");
    }
    void OnFixedUpdateToDie(int iCurrentState, int iFixedTick)
    {
        if (100 < iFixedTick)
        {
            SHUtils.For(0, 360, 60, (iValue) =>
            {
                var vDirection = Vector3.zero;
                vDirection.x = Mathf.Cos(iValue * Mathf.Deg2Rad);
                vDirection.z = Mathf.Sin(iValue * Mathf.Deg2Rad);
                SetAttack(vDirection);
            });
            
            SetActive(false);
            Single.Monster.DeleteMonster(this);
        }
    }
    #endregion


    #region State : Kill
    void OnEnterToKill(int iBeforeState, int iCurrentState)
    {
        OnEnterToDie(iBeforeState, iCurrentState);

        SetActive(false);
        Single.Monster.DeleteMonster(this);
    }
    #endregion
}