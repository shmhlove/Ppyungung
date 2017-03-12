using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicStep = System.Collections.Generic.Dictionary<eGameStep, SHGameStep_Component>;

public enum eGameStep
{
    None,
    Start,
    Play,
    ChangePhase,
    Result,
}

public class SHGameStep : SHInGame_Component
{
    #region Members
    private DicStep     m_dicSteps      = new DicStep();
    public int          m_iCallCnt      = 0;
    public eGameStep    m_eBeforeStep   = eGameStep.None;
    public eGameStep    m_eCurrentStep  = eGameStep.None;
    public eGameStep    m_eMoveTo       = eGameStep.None;
    #endregion
    

    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        m_dicSteps.Clear();
        m_dicSteps.Add(eGameStep.Start,       new SHGameStep_Start());
        m_dicSteps.Add(eGameStep.Play,        new SHGameStep_Play());
        m_dicSteps.Add(eGameStep.ChangePhase, new SHGameStep_ChangePhase());
        m_dicSteps.Add(eGameStep.Result,      new SHGameStep_Result());

        MoveTo(eGameStep.Start);
    }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        if (0 == m_dicSteps.Count)
            return;

        ChangeStep();

        if (false == IsExistStep(m_eCurrentStep))
            return;

        m_dicSteps[m_eCurrentStep].FrameMove(++m_iCallCnt);
    }
    public override void SetPause(bool bIsPause)
    {
        m_bIsPause = bIsPause;

        if (false == IsExistStep(m_eCurrentStep))
            return;

        if (true == m_bIsPause)
            m_dicSteps[m_eCurrentStep].Pause();
        else
            m_dicSteps[m_eCurrentStep].Resume();
    }
    #endregion


    #region Interface : Control
    public void MoveTo(eGameStep eStep)
    {
        if (false == IsExistStep(eStep))
        {
            Debug.LogWarningFormat("SHGameStep:MoveStep() - Not Register Step : {0}", eStep);
            return;
        }

        m_eMoveTo = eStep;
        m_dicSteps[m_eMoveTo].m_eStep = m_eMoveTo;
        m_dicSteps[m_eMoveTo].Awake();
    }
    public void DirectMoveTo(eGameStep eStep)
    {
        MoveTo(eStep);
        ChangeStep();
    }
    #endregion


    #region Interface : Helpper
    public bool IsStep(eGameStep eType)
    {
        return (eType == m_eCurrentStep);
    }
    #endregion


    #region Utility Functions
    private void ChangeStep()
    {
        if (eGameStep.None == m_eMoveTo)
            return;

        if (false == IsExistStep(m_eMoveTo))
            return;

        if (true == IsExistStep(m_eCurrentStep))
            m_dicSteps[m_eCurrentStep].FinalStep();

        m_iCallCnt      = 0;
        m_eBeforeStep   = m_eCurrentStep;
        m_eCurrentStep  = m_eMoveTo;
        m_eMoveTo       = eGameStep.None;

        m_dicSteps[m_eCurrentStep].InitialStep();
    }
    private bool IsExistStep(eGameStep eStep)
    {
        return m_dicSteps.ContainsKey(eStep);
    }
    #endregion


    #region Event Handler
    #endregion
}