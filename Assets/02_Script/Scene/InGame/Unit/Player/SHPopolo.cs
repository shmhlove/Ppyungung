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
    public override void FixedUpdate()
    {
        base.FrameMove();
    }
    public override void RegisterState()
    {
        var pState = CreateState(eState.Idle);
        pState.m_OnEnterState  = OnEnterStateToIdle;
        pState.m_OnExitState   = OnExitStateToIdle;
        pState.m_OnFixedUpdate = OnFixedUpdateToIdle;
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
#endregion


#region State Functions
    void OnEnterStateToIdle(int iBeforeState, int iCurrentState)
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        pCtrlUI.AddEventToMove((vDirection) => 
        {
            AddLocalPositionX(SHHard.m_fPlayerMoveSpeed * vDirection.x);
            AddLocalPositionZ(SHHard.m_fPlayerMoveSpeed * vDirection.y);
        });
        pCtrlUI.AddEventToDirection((vDirection) => 
        {
            SetLocalRotateY(SHMath.GetAngleToPosition(Vector3.forward, -1.0f, Vector3.up, vDirection));
        });
        pCtrlUI.AddEventToShoot(() =>
        {
            SH3DRoot.PlayCameraShake();
            Single.Damage.AddDamage("Dmg_Bullet",
                            new SHAddDamageParam(m_pShootPos, null, null, null));
        });
    }
    void OnExitStateToIdle(int iBeforeState, int iCurrentState)
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        if (null == pCtrlUI)
            return;

        pCtrlUI.Clear();
    }
    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {

    }
#endregion
}