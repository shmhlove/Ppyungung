using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SHGameStep_Component
{
    #region Members
    public eGameStep m_eStep = eGameStep.None;
    #endregion


    #region Virtual Functions
    public virtual void Awake() { }                    // MoveTo가 호출되는 순간 호출
    public virtual void InitialStep() { }              // ChangeStep가 호출되고 난 다음 프레임에 호출
    public virtual void FrameMove(int iCallCnt) { }    // 매프레임 호출
    public virtual void FinalStep() { }                // ChangeStep가 호출되고 난 다음 프레임에 호출
    public virtual void Pause() { }                    // Step이 Pause될때
    public virtual void Resume() { }                   // Step이 Resume될때
    #endregion


    #region System Functions
    #endregion


    #region Interface Functions
    public void MoveTo(eGameStep eMoveStep)
    {
        Single.GameStep.MoveTo(eMoveStep);
    }
    public void DirectMoveTo(eGameStep eMoveStep)
    {
        Single.GameStep.DirectMoveTo(eMoveStep);
    }
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    #endregion
}