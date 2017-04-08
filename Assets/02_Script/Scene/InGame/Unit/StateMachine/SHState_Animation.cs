using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHState : SHMonoWrapper
{
    public void PlayAnimation(SHStateInfo pInfo)
    {
        if (null == pInfo)
            return;

        if (null == m_pAnimRoot)
            return;
        
        StopAnimCoroutine();
        base.PlayAnim(eDirection.Front, m_pAnimRoot, pInfo.m_strAnimClip, pInfo.OnEndAnimation);
    }

    public bool IsAnimPlaying(int iStateID)
    {
        var pState = GetStateInfo(iStateID);
        if (null == pState)
            return false;

        return base.IsPlaying(pState.m_strAnimClip);
    }

    public void SetPauseAnimation(bool bIsPause)
    {
        var pState = GetCurrentState();
        if (null == pState)
            return;

        base.SetPauseAnim(bIsPause, pState.m_strAnimClip);
    }
}