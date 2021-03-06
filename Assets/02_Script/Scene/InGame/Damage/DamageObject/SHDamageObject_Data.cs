﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum eDamageEvent
{
    None,
    Tick,
    Start,
    Delete,
    Crash,
}

[Serializable]
public class SHDamageTimmingInfo
{
    public int      m_iPlayToLifeTick     = -1;                 // 생성:  LifeTime
    public bool     m_bIsPlayToCrash      = false;              // 생성 : 충돌시
    public bool     m_bIsPlayToStart      = false;              // 생성 : 데미지 생성될때 
    public bool     m_bIsPlayToDelete     = false;              // 생성 : 데미지 제거될때

    public bool IsTimming(SHDamageObjectInfo pInfo, eDamageEvent eEvent)
    {
        if (null == pInfo)
            return false;

        switch (eEvent)
        {
            case eDamageEvent.Tick:   return (pInfo.m_iLifeTick == m_iPlayToLifeTick);
            case eDamageEvent.Start:  return m_bIsPlayToStart;
            case eDamageEvent.Delete: return m_bIsPlayToDelete;
            case eDamageEvent.Crash:  return m_bIsPlayToCrash;
        }

        return false;
    }
}

[Serializable]
public class SHDamageSoundInfo
{
    public string              m_strClipName = string.Empty;       // 사운드 클립 이름
    public SHDamageTimmingInfo m_pTimming    = new SHDamageTimmingInfo();
}

[Serializable]
public class SHDamageEffectInfo
{
    public string               m_strPrefabName        = string.Empty;       // 이팩트 프리팹 이름
    public SHDamageTimmingInfo  m_pTimming             = new SHDamageTimmingInfo();

    public bool                 m_bIsStartPosToDamage  = true;               // 현재 데미지위치에서 생성될것인가?
    public Vector3              m_vStaticStartPosition = Vector3.zero;       // 고정 위치
    public Vector3              m_vPositionOffset      = Vector3.zero;       // 위치 오프셋
    public bool                 m_bIsTraceDamage       = false;              // 데미지에 붙어 함께 움직일 것인가?
    public bool                 m_bIsDeleteWithDamage  = true;               // 데미지와 함께 죽을것인가?

    [HideInInspector]
    public GameObject m_pEffectObject = null;

    public void ClearEffectObject()
    {
        m_pEffectObject = null;
    }

    public void SetEffectObject(GameObject pObject)
    {
        m_pEffectObject = pObject;
    }

    public void SetDisableObject()
    {
        if (null == m_pEffectObject)
            return;

        m_pEffectObject.SetActive(false);
    }
}

[Serializable]
public class SHAddDamageInfo
{
    public string              m_strPrefabName = string.Empty;
    public SHDamageTimmingInfo m_pTimming      = new SHDamageTimmingInfo();
}

[Serializable]
public class SHDamageObjectInfo
{
    [Header("- 각 속성의 자세한 설명은 SHDamageObject_Data.cs파일을 참고하세요.")]
    [Header("[데미지 ID (생성시 자동 발급됩니다.)]")]
    [ReadOnlyField] public string m_strID               = string.Empty;
    [ReadOnlyField] public string m_strPrefabName       = string.Empty;

    [Header("[충돌체크를 할 타켓 유닛의 태그 리스트]")]
    [DamageTags] public List<string> m_pTargetUnitTags  = new List<string>();
    [Header("[충돌체크를 무시할 타켓 유닛의 이름(프리팹) 리스트]")]
    public List<string>     m_pIgnoreTargetName         = new List<string>();

    [Header("[기본 정보 (1sec = 50Tick)]")]
    public bool             m_bIsLoopLifeTick           = false;            // 라이프 타임을 계속 반복할것인가?
    public int              m_iLifeTick                 = 0;                // 라이프 타임
    public float            m_fDamageValue              = 0.0f;             // 데미지 값
    public int              m_iCheckDelayTickToCrash    = 0;                // 충돌시 얼마간 충돌체크를 하지 않을 것인가? (다단히트)
    public int              m_iDamageHP                 = 0;                // 데미지 HP
    public bool             m_bIsDeleteToCrash          = true;             // 충돌이 즉시 이 데미지를 제거할 것인가?
    public bool             m_bIsDeleteWithCreator      = true;             // 데미지 생성자가 Disable일때 데미지를 제거할 것인가?
    
    [Header("[위치]")]
    public bool             m_bIsParentToUI             = false;            // UI Root를 부모 오브젝트로 설정할 것인가?
    public bool             m_bIsTraceToCreator         = false;            // 데미지 발생자를 따라다닐 것인가?
    public bool             m_bIsStartPosToCreator      = true;             // 데미지 발생자위치에서 시작하게 할것인가?
    public Vector3          m_vStaticStartPosition      = Vector3.zero;     // 고정된 시작위치
    public Vector3          m_vPositionOffset           = Vector3.zero;     // 위치 오프셋
    public Vector3          m_vRotationOffset           = Vector3.zero;     // 위치 오프셋 (회전 적용된 위치)

    [Header("[스케일]")]
    public int              m_iScaleLifeTic             = 0;                // 스케일링 속도
    public Vector3          m_vStartScale               = Vector3.zero;     // 시작 스케일 값
    public Vector3          m_vEndScale                 = Vector3.zero;     // 종료 스케일 값
    [HideInInspector] public Vector3 m_vScaleSpeed      = Vector3.zero;
    [HideInInspector] public Vector3 m_vScaleValue      = Vector3.zero;

    // 회전도 필요할듯하네??

    [Header("[기본 움직임]")]
    public float                m_fMass                     = 1.0f;             // 데미지 무게 ( Force가 없으면 의미없음 )
    public float                m_fStartSpeed               = 0.0f;             // 데미지의 이동속도
    public float                m_fAddSpeed                 = 0.0f;             // 가산 속도
    public Vector3              m_vStartDirection           = Vector3.zero;     // 데미지의 이동방향
    public float                m_fOffsetAngle              = 0.0f;             // 데미지의 추가 이동방향
    public bool                 m_bIsStartDirectionToCreator= true;             // 초기 방향을 데미지 생성자의 방향으로 할것인가?
    public bool                 m_bIsRandomStartDirection   = false;            // 초기 방향 랜덤으로 결정할 것인가?
    public Vector3              m_vForce                    = Vector3.zero;     // 힘

    [Header("[유도 움직임]")]
    public bool                 m_bIsUseGuideSystem         = false;            // 유도기능을 사용할 것인가?
    public int                  m_iNotGuideTick             = 0;                // 지정된 Tick동안 가이드하지 않는다.
    public AnimationCurve       m_pGuideAngleSpeed          = AnimationCurve.Linear(0, 0, 10, 10); // 쫓아갈 회전 속도

    [Header("[애니메이션]")]
    public AnimationClip        m_pAnimationClip            = null;             // 데미지 애니메이션
    public GameObject           m_pAnimTarget               = null;             // 애니메이션 타켓 오브젝트

    [Header("[사운드]")]
    public List<SHDamageSoundInfo> m_pSoundInfo         = new List<SHDamageSoundInfo>();  // 충돌시 재생시킬 사운드 이름
    
    [Header("[이펙트]")]
    public List<SHDamageEffectInfo> m_pEffectInfo       = new List<SHDamageEffectInfo>(); // 재생시킬 파티클이펙트 정보

    [Header("[추가 데미지]")]
    public List<SHAddDamageInfo> m_pAddDamageInfo       = new List<SHAddDamageInfo>();    // 추가 데미지 정보  

    public SHDamageObjectInfo() { }
    public SHDamageObjectInfo(SHDamageObjectInfo pCopy)
    {
        CopyTo(pCopy);
    }
    public void CopyTo(SHDamageObjectInfo pCopy)
    {
        if (null == pCopy)
            return;

        m_strID                     = pCopy.m_strID;
        m_strPrefabName             = pCopy.m_strPrefabName;
        m_pTargetUnitTags           = new List<string>(pCopy.m_pTargetUnitTags);
        m_pIgnoreTargetName         = new List<string>(pCopy.m_pIgnoreTargetName);

        m_bIsLoopLifeTick           = pCopy.m_bIsLoopLifeTick;
        m_iLifeTick                 = pCopy.m_iLifeTick;
        m_fDamageValue              = pCopy.m_fDamageValue;
        m_iCheckDelayTickToCrash    = pCopy.m_iCheckDelayTickToCrash;
        m_iDamageHP                 = pCopy.m_iDamageHP;
        m_bIsDeleteToCrash          = pCopy.m_bIsDeleteToCrash;
        m_bIsDeleteWithCreator      = pCopy.m_bIsDeleteWithCreator;

        m_bIsParentToUI             = pCopy.m_bIsParentToUI;
        m_bIsTraceToCreator         = pCopy.m_bIsTraceToCreator;
        m_bIsStartPosToCreator      = pCopy.m_bIsStartPosToCreator;
        m_vStaticStartPosition      = pCopy.m_vStaticStartPosition;
        m_vPositionOffset           = pCopy.m_vPositionOffset;
        m_vRotationOffset           = pCopy.m_vRotationOffset;

        m_iScaleLifeTic             = pCopy.m_iScaleLifeTic;
        m_vStartScale               = pCopy.m_vStartScale;
        m_vEndScale                 = pCopy.m_vEndScale;
        m_vScaleSpeed               = pCopy.m_vScaleSpeed;
        m_vScaleValue               = pCopy.m_vScaleValue;

        m_fMass                     = pCopy.m_fMass;
        m_fStartSpeed               = pCopy.m_fStartSpeed;
        m_fAddSpeed                 = pCopy.m_fAddSpeed;
        m_vStartDirection           = pCopy.m_vStartDirection;
        m_fOffsetAngle              = pCopy.m_fOffsetAngle;
        m_bIsStartDirectionToCreator= pCopy.m_bIsStartDirectionToCreator;
        m_bIsRandomStartDirection   = pCopy.m_bIsRandomStartDirection;
        m_vForce                    = pCopy.m_vForce;

        m_bIsUseGuideSystem         = pCopy.m_bIsUseGuideSystem;
        m_iNotGuideTick             = pCopy.m_iNotGuideTick;
        m_pGuideAngleSpeed          = pCopy.m_pGuideAngleSpeed;

        m_pAnimationClip            = pCopy.m_pAnimationClip;
        m_pAnimTarget               = pCopy.m_pAnimTarget;

        m_pSoundInfo                = new List<SHDamageSoundInfo>(pCopy.m_pSoundInfo);
        m_pEffectInfo               = new List<SHDamageEffectInfo>(pCopy.m_pEffectInfo);
        m_pAddDamageInfo            = new List<SHAddDamageInfo>(pCopy.m_pAddDamageInfo);
    }
}