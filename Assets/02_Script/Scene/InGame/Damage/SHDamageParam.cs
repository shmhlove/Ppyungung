using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using ListEventDelete    = System.Collections.Generic.List<System.Action<SHDamageObject>>;
using ListEventCollision = System.Collections.Generic.List<System.Action<SHDamageObject, SHMonoWrapper>>;

[Serializable]
public class SHDamageParam
{
    [ReadOnlyField]   public SHMonoWrapper      m_pWho              = null;                     // 데미지 발생자
    [ReadOnlyField]   public Vector3            m_pStartPosition    = Vector3.zero;             // 시작위치
    [ReadOnlyField]   public GameObject         m_pGuideTarget      = null;                     // 쫓아갈 타켓 ( 유도 옵션이 있을경우 )
    [HideInInspector] public ListEventDelete    m_pEventToDelete    = new ListEventDelete();    // 데미지가 제거될때 콜백
    [HideInInspector] public ListEventCollision m_pEventToCollision = new ListEventCollision(); // 데미지가 충돌될때 콜백

    public SHDamageParam() { }
    public SHDamageParam(
        SHMonoWrapper                         pWho,
        Vector3                               vStartPos,
        GameObject                            pGuideTarget    = null, 
        Action<SHDamageObject>                pEventDelete    = null, 
        Action<SHDamageObject, SHMonoWrapper> pEventCollision = null)
    {
        m_pWho           = pWho;
        m_pStartPosition = vStartPos;
        m_pGuideTarget   = pGuideTarget;
        AddEventToDelete(pEventDelete);
        AddEventToCollision(pEventCollision);
    }
    public SHDamageParam(SHDamageParam pParam)
    {
        if (null == pParam)
            return;

        m_pWho              = pParam.m_pWho;
        m_pStartPosition    = pParam.m_pStartPosition;
        m_pGuideTarget      = pParam.m_pGuideTarget;
        m_pEventToDelete    = new ListEventDelete(pParam.m_pEventToDelete);
        m_pEventToCollision = new ListEventCollision(pParam.m_pEventToCollision);
    }

    public void AddEventToDelete(Action<SHDamageObject> pEvent)
    {
        if (null == pEvent)
            return;

        m_pEventToDelete.Add(pEvent);
    }

    public void AddEventToCollision(Action<SHDamageObject, SHMonoWrapper> pEvent)
    {
        if (null == pEvent)
            return;

        m_pEventToCollision.Add(pEvent);
    }

    public void SendEventToDelete(SHDamageObject pDamage)
    {
        SHUtils.ForToList(m_pEventToDelete, (pCallback) => pCallback(pDamage));
    }

    public void SendEventToCollision(SHDamageObject pDamage, SHMonoWrapper pTarget)
    {
        SHUtils.ForToList(m_pEventToCollision, (pCallback) => pCallback(pDamage, pTarget));
    }
}