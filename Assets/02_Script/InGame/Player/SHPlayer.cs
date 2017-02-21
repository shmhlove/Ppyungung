using UnityEngine;
using System;
using System.Collections;

public class SHPlayer : SHState
{
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
            AddLocalPositionY(SHHard.m_fPlayerMoveSpeed * vDirection.y);
            SetRotateZ(SHMath.GetAngleToPosition(Vector3.forward, Vector3.up, vDirection));
        });
    }

    void OnFixedUpdateToIdle(int iCurrentState, int iFixedTick)
    {

    }
}