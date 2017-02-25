using UnityEngine;
using System;
using System.Collections;

public class SHPopolo : SHState
{
    #region Members : State List
    public enum eState
    {
        None,
        Idle,
    }
    #endregion


    #region Members : Inspector
    [Header("Player Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos  = null;
    [SerializeField] private Animation     m_pAnimation = null;
    [SerializeField] private float         m_fMoveSpeed = 50.0f;
    #endregion


    #region System Functions
    public override void Start()
    {
#if UNITY_EDITOR
        SHHard.m_fPlayerMoveSpeed = m_fMoveSpeed;
#else
        SHHard.m_fPlayerMoveSpeed = SHPlayerPrefs.GetFloat("Player_MoveSpeed", m_fMoveSpeed);
#endif
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        DeConnectController();
    }
    public override void FixedUpdate()
    {
        base.FrameMove();
    }
    public override void RegisterState()
    {
        var pState = CreateState(eState.Idle);
        {
            pState.m_OnEnterState  = OnEnterStateToIdle;
            pState.m_OnExitState   = OnExitStateToIdle;
            pState.m_OnFixedUpdate = OnFixedUpdateToIdle;
        }
        ConnectController();
        ChangeState(eState.Idle);
    }
    #endregion


    #region Utility Functions
    SHStateInfo CreateState(eState eType)
    {
        return base.CreateState((int)eType);
    }
    void ChangeState(eState eType)
    {
        base.ChangeState((int)eType);
    }
    void ChangeAnimation(string strClipName)
    {
        if (null == m_pAnimation)
            return;

        m_pAnimation.Play(strClipName);
    }
    void ConnectController()
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        pCtrlUI.AddEventToMove((vDirection) =>
        {
            if ((false == m_pAnimation.IsPlaying("Anim_Char_Attack")) &&
                (false == m_pAnimation.IsPlaying("Anim_Char_Move")))
                ChangeAnimation("Anim_Char_Move");

            AddLocalPositionX(SHHard.m_fPlayerMoveSpeed * vDirection.x);
            AddLocalPositionZ(SHHard.m_fPlayerMoveSpeed * vDirection.y);
        });
        pCtrlUI.AddEventToDirection((vDirection) =>
        {
            SetLocalRotateY(SHMath.GetAngleToPosition(Vector3.forward, -1.0f, Vector3.up, vDirection));
        });
        pCtrlUI.AddEventToShoot(() =>
        {
            ChangeAnimation("Anim_Char_Attack");
            SH3DRoot.PlayCameraShake();
            Single.Damage.AddDamage("Dmg_Bullet",
                            new SHAddDamageParam(m_pShootPos, null, null, null));
        });
    }
    void DeConnectController()
    {
        if (false == SHUIManager.IsExists)
            return;

        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        pCtrlUI.Clear();
    }
    #endregion


    #region State Functions
    // 대기상태
    void OnEnterStateToIdle(int iBeforeState, int iCurrentState)
    {
    }
    void OnExitStateToIdle(int iBeforeState, int iCurrentState)
    {
    }
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {
        if ((false == m_pAnimation.IsPlaying("Anim_Char_Attack")) &&
            (false == m_pAnimation.IsPlaying("Anim_Char_Move")) &&
            (false == m_pAnimation.IsPlaying("Anim_Char_Wait")))
                ChangeAnimation("Anim_Char_Wait");
    }

    // 이동상태
    //void OnEnterStateToMove(int iBeforeState, int iCurrentState) { }
    //void OnExitStateToMove(int iBeforeState, int iCurrentState) { }
    //void OnFixedUpdateToMove(int iCurrentState, int iFixedTick) { }

    // 공격상태
    //void OnEnterStateToAttack(int iBeforeState, int iCurrentState) { }
    //void OnExitStateToAttack(int iBeforeState, int iCurrentState) { }
    //void OnFixedUpdateToAttack(int iCurrentState, int iFixedTick) { }
    #endregion
}