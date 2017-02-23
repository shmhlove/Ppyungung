using UnityEngine;
using System;
using System.Collections;

public class SHPopolo : SHState
{
    #region Members : Inspector
    [Header("Player Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos = null;
    #endregion


    public enum eState
    {
        None,
        Idle,
    }
    
    public override void FixedUpdate()
    {
        base.FrameMove();
    }

    public override void RegisterState()
    {
        var pState = CreateState(eState.Idle);
        pState.m_OnEnterState  = OnEnterStateToIdle;
        pState.m_OnFixedUpdate = OnFixedUpdateToIdle;
        ChangeState(eState.Idle);
    }
    
    SHStateInfo CreateState(eState eType)
    {
        return base.CreateState((int)eType);
    }

    void ChangeState(eState eType)
    {
        base.ChangeState((int)eType);
    }

    void OnEnterStateToIdle(int iBeforeState, int iCurrentState)
    {
        var pCtrlUI = Single.UI.GetPanel<SHUIPanel_CtrlPad>("Panel_CtrlPad");
        pCtrlUI.AddEventToDrag((vCenter, vThumb, vDirection) => 
        {
            AddLocalPositionX(SHHard.m_fPlayerMoveSpeed * vDirection.x);
            AddLocalPositionZ(SHHard.m_fPlayerMoveSpeed * vDirection.y);
            SetLocalRotateY(SHMath.GetAngleToPosition(Vector3.forward, -1.0f, Vector3.up, vDirection));
        });
        pCtrlUI.AddEventToShoot(() =>
        {
            SH3DRoot.PlayCameraShake();
            Single.Damage.AddDamage("Dmg_Bullet",
                            new SHAddDamageParam(m_pShootPos, null, null, null));
        });
    }

    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {

    }
}