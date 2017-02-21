using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using DicStep = System.Collections.Generic.Dictionary<eStep, SHStep_Component>;

public enum eStep
{
    None,
    Start,
    Play,
    Result,
}

public class SHStep : SHInGame_Component
{
    #region Members
    private DicStep     m_dicSteps      = new DicStep();
    public int          m_iCallCnt      = 0;
    public eStep        m_eBeforeStep   = eStep.None;
    public eStep        m_eCurrentStep  = eStep.None;
    public eStep        m_eMoveTo       = eStep.None;
    #endregion
    

    #region System Functions
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        m_dicSteps.Clear();
        m_dicSteps.Add(eStep.Start,  new SHStep_Start());
        m_dicSteps.Add(eStep.Play,   new SHStep_Play());
        m_dicSteps.Add(eStep.Result, new SHStep_Result());

        MoveTo(eStep.Start);
    }
    public override void OnFinalize() { }
    public override void OnFrameMove()
    {
        if (true == m_bIsPause)
            return;

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
    public void MoveTo(eStep eStep)
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
    public void DirectMoveTo(eStep eStep)
    {
        MoveTo(eStep);
        ChangeStep();
    }
    #endregion


    #region Interface : Helpper
    #endregion


    #region Utility Functions
    private void ChangeStep()
    {
        if (eStep.None == m_eMoveTo)
            return;

        if (false == IsExistStep(m_eMoveTo))
            return;

        if (true == IsExistStep(m_eCurrentStep))
            m_dicSteps[m_eCurrentStep].FinalStep();

        m_iCallCnt      = 0;
        m_eBeforeStep   = m_eCurrentStep;
        m_eCurrentStep  = m_eMoveTo;
        m_eMoveTo       = eStep.None;

        m_dicSteps[m_eCurrentStep].InitialStep();
    }
    private bool IsExistStep(eStep eStep)
    {
        return m_dicSteps.ContainsKey(eStep);
    }
    #endregion


    #region Event Handler
    #endregion
}