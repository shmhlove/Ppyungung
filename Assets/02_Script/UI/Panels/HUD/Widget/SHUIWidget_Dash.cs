using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SHUIWidget_Dash : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private UISlider m_pSlider       = null;
    [SerializeField] private UILabel  m_pLabelPercent = null;
    #endregion


    #region Members : Info
    private float m_fTargetPercent  = 0.0f;
    private float m_fCurrentPercent = 0.0f;
    private float m_fSpeedWeight    = 0.0f;
    #endregion


    #region Members : Constants
    private float MOVE_SPEED = 0.05f;
    #endregion


    #region Virtual Functions
    public override void Start()
    {
        SetSlider();
        SetLabel();
    }
    #endregion


    #region Interface Functions
    public void FrameMove()
    {
        if (false == CalcCurrentPercent())
            return;

        SetSlider();
        SetLabel();
    }
    #endregion


    #region Utility Functions
    bool CalcCurrentPercent()
    {
        m_fTargetPercent = Single.Player.GetDPPercent();

        if (m_fTargetPercent == m_fCurrentPercent)
        {
            m_fSpeedWeight = 0.0f;
            return false;
        }
        
        if (m_fCurrentPercent < m_fTargetPercent)
            m_fCurrentPercent = m_fCurrentPercent + (m_fSpeedWeight += MOVE_SPEED);
        else
            m_fCurrentPercent = m_fCurrentPercent - (m_fSpeedWeight += MOVE_SPEED);

        if (m_fSpeedWeight >= Mathf.Abs(m_fTargetPercent - m_fCurrentPercent))
            m_fCurrentPercent = m_fTargetPercent;

        return true;
    }
    void SetSlider()
    {
        if (null == m_pSlider)
            return;

        m_pSlider.value = (m_fCurrentPercent / 100.0f);
    }
    void SetLabel()
    {
        if (null == m_pLabelPercent)
            return;

        m_pLabelPercent.text = string.Format("{0}%", m_fCurrentPercent.ToString("N2"));
    }
    #endregion
}
