﻿using UnityEngine;
using System;
using System.Collections;

public class SHUIButtonAnimation : SHMonoWrapper
{
    #region Members : Inspector
	[SerializeField] private AnimationClip  m_pAnimClipOnTouch  = null;
	[SerializeField] private AnimationClip  m_pAnimClipOnHover  = null;
    [SerializeField] private AnimationClip  m_pAnimClipOnIdle   = null;
    [SerializeField] private AnimationClip  m_pAnimClipOnClick  = null;
    [SerializeField] private AnimationClip  m_pAnimClipOnPress  = null;
    [SerializeField] private GameObject     m_pAnimTarget       = null;
    #endregion


    #region Members : Info
    private bool        m_bIsPress       = false;
    private bool        m_bIsHover       = false;
    private Vector3     m_vScale         = Vector3.one;
    #endregion


    #region System Functions
    public override void Awake()
    {
        base.Awake();

        m_vScale = GetTarget().transform.localScale;
    }
    public override void Start()
    {
        base.Start();

        if (true == UICamera.IsHighlighted(GetTarget()))
            OnHover(true);
        else
            Play(m_pAnimClipOnIdle);
    }
    public override void OnDisable()
    {
        base.OnDisable();

        if (true == SHApplicationInfo.m_bIsAppQuit)
            return;

        GetTarget().transform.localScale = m_vScale;
    }
    #endregion


    #region Utility Functions
    GameObject GetTarget()
    {
        return (null == m_pAnimTarget) ? gameObject : m_pAnimTarget;
    }
    bool Play(AnimationClip pClip, bool bForward = true, Action pOnPlayEnd = null)
    {
        StopAnimCoroutine();
        PlayAnim((true == bForward) ? eDirection.Front : eDirection.Back, GetTarget(), pClip, pOnPlayEnd);
        return true;
    }
    void CheckHighlighted()
    {
        m_bIsHover = UICamera.IsHighlighted(gameObject);

        if (true == m_bIsHover)
            Play(m_pAnimClipOnHover);
        else
            Play(m_pAnimClipOnIdle);
    }
    #endregion


    #region Coroutine Functions
    IEnumerator CoroutineToPressAnim()
    {
        while(true == m_bIsPress)
        {
            Play(m_pAnimClipOnPress, pOnPlayEnd: CheckHighlighted);
            yield return new WaitForSeconds(0.2f);
        }
    }
    #endregion


    #region Event Handler
    void OnPress(bool bIsPressed)
    {
        if (false == enabled)
            return;
        
        if (true == (m_bIsPress = bIsPressed))
        {
            Play(m_pAnimClipOnTouch, pOnPlayEnd: CheckHighlighted);

            if (null != m_pAnimClipOnPress)
                StartCoroutine(CoroutineToPressAnim());
        }
        else if (false == m_bIsAnimPlaying)
        {
            OnHover(UICamera.IsHighlighted(gameObject));
        }
    }
    void OnHover(bool bIsOver)
    {
        if (false == enabled)
            return;

        if (bIsOver == m_bIsHover)
            return;
        
        if (true == bIsOver)
        {
            Play(m_pAnimClipOnHover, true);
        }
        else
        {
            Play(m_pAnimClipOnHover, false, () => Play(m_pAnimClipOnIdle));
        }

        m_bIsHover = bIsOver;
    }
    void OnSelect(bool bSelected)
    {
        if (false == enabled)
            return;

        if ((false == bSelected) || 
            (UICamera.ControlScheme.Controller == UICamera.currentScheme))
        {
            OnHover(bSelected);
        }
    }
    void OnClick()
    {
        if (false == enabled)
            return;

        Play(m_pAnimClipOnClick, pOnPlayEnd: () => Play(m_pAnimClipOnIdle));
    }
    #endregion
}