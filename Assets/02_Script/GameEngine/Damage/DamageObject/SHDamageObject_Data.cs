using UnityEngine;
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
public class SHDamageSoundInfo
{
    public string   m_strClipName         = string.Empty;       // 사운드 클립 이름

    public int      m_iPlayToLifeTick     = -1;                 // 생성:  LifeTime
    public bool     m_bIsPlayToCrash      = false;              // 생성 : 충돌시
    public bool     m_bIsPlayToStart      = false;              // 생성 : 데미지 생성될때 
    public bool     m_bIsPlayToDelete     = false;              // 생성 : 데미지 제거될때 
}

[Serializable]
public class SHDamageEffectInfo
{
    public string   m_strPrefabName       = string.Empty;       // 이팩트 프리팹 이름

    public int      m_iPlayToLifeTick     = -1;                 // 생성:  LifeTime
    public bool     m_bIsPlayToCrash      = false;              // 생성 : 충돌시
    public bool     m_bIsPlayToStart      = false;              // 생성 : 데미지 생성될때 
    public bool     m_bIsPlayToDelete     = false;              // 생성 : 데미지 제거될때 

    public bool     m_bIsStartPosToDamage = true;               // 현재 데미지위치에서 생성될것인가?
    public Vector3  m_vStaticStartPosition= Vector3.zero;       // 고정 위치
    public Vector3  m_vPositionOffset     = Vector3.zero;       // 위치 오프셋
    public bool     m_bIsTraceDamage      = false;              // 데미지에 붙어 함께 움직일 것인가?
    public bool     m_bIsDeleteWithDamage = true;               // 데미지와 함께 죽을것인가?

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
public class SHDamageObjectInfo
{
    [Header("- 데미지 ID (생성시 자동 발급됩니다.) ")]
    [Header("- 각 속성의 자세한 설명은 SHDamageObject_Data.cs파일을 참고하세요.")]
    [ReadOnlyField]
    public string m_strID                               = string.Empty;

    [Header("- 충돌체크를 할 타켓 유닛의 태그 리스트")][DamageTags]
    public List<string>     m_pTargetUnitTags           = new List<string>();

    [Header("- 기본 정보 (1sec = 50Tick)")]
    public bool             m_bIsLoopLifeTick           = false;            // 라이프 타임을 계속 반복할것인가?
    public int              m_iLifeTick                 = 0;                // 라이프 타임
    public float            m_fDamageValue              = 0.0f;             // 데미지 값
    public int              m_iCheckDelayTickToCrash    = 0;                // 충돌시 얼마간 충돌체크를 하지 않을 것인가? (다단히트)
    public int              m_iCheckDelayTickToStart    = 0;                // 데미지 생성 후 얼마간 충돌체크를 하지 않을 것인가?
    public int              m_iCheckDelayTickToLate     = 0;                // 데미지 생성 뒤 얼마 후 부터 충돌체크를 하지 않을 것인가?
    public bool             m_bIsDeleteToCrash          = true;             // 충돌이 즉시 이 데미지를 제거할 것인가?
    public bool             m_bIsDeleteWithCreator      = true;             // 데미지 생성자가 Disable일때 데미지를 제거할 것인가?

    [Header("- 위치")]
    public bool             m_bIsTraceToCreator         = false;            // 데미지 발생자를 따라다닐 것인가?
    public bool             m_bIsStartPosToCreator      = true;             // 데미지 발생자위치에서 시작하게 할것인가?
    public Vector3          m_vStaticStartPosition      = Vector3.zero;     // 고정된 시작위치
    public Vector3          m_vPositionOffset           = Vector3.zero;     // 위치 오프셋
    
    // 회전도 필요할듯하네??

    [Header("- 기본이동")]
    public float            m_fMass                     = 1.0f;             // 데미지 무게 ( Force가 없으면 의미없음 )
    public float            m_fStartSpeed               = 0.0f;             // 데미지의 이동속도
    public float            m_fAddSpeed                 = 0.0f;             // 가산 속도
    public Vector3          m_vStartDirection           = Vector3.zero;     // 데미지의 이동방향
    public bool             m_bIsRandomStartDirection   = false;            // 초기 방향 랜덤으로 결정할 것인가?
    public Vector3          m_vForce                    = Vector3.zero;     // 힘

    [Header("- 유도이동")]
    public bool             m_bIsUseGuideSystem         = false;            // 유도기능을 사용할 것인가?
    public int              m_iNotGuideTick             = 0;                // 지정된 Tick동안 가이드하지 않는다.
    public float            m_fGuideAngleSpeed          = 0.0f;             // 쫓아갈 회전속도
    public bool             m_bIsUseCuvGuideAngleSpeed  = false;            // 커브 이동속도를 사용할 것인가? (커브)
    public AnimationCurve   m_pGuideCuvAngleSpeed       = AnimationCurve.Linear(0, 0, 1, 1); // 쫓아갈 회전 속도

    [Header("- 애니메이션")]
    public AnimationClip    m_pAnimationClip            = null;             // 데미지 애니메이션
    public GameObject       m_pAnimTarget               = null;             // 애니메이션 타켓 오브젝트

    [Header("- 사운드")]
    public List<SHDamageSoundInfo> m_pSoundInfo         = new List<SHDamageSoundInfo>();  // 충돌시 재생시킬 사운드 이름
    
    [Header("- 이펙트")]
    public List<SHDamageEffectInfo> m_pEffectInfo       = new List<SHDamageEffectInfo>(); // 재생시킬 파티클이펙트 정보

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
        m_pTargetUnitTags           = new List<string>(pCopy.m_pTargetUnitTags);

        m_bIsLoopLifeTick           = pCopy.m_bIsLoopLifeTick;
        m_iLifeTick                 = pCopy.m_iLifeTick;
        m_fDamageValue              = pCopy.m_fDamageValue;
        m_iCheckDelayTickToCrash    = pCopy.m_iCheckDelayTickToCrash;
        m_iCheckDelayTickToStart    = pCopy.m_iCheckDelayTickToStart;
        m_iCheckDelayTickToLate     = pCopy.m_iCheckDelayTickToLate;
        m_bIsDeleteToCrash          = pCopy.m_bIsDeleteToCrash;
        m_bIsDeleteWithCreator      = pCopy.m_bIsDeleteWithCreator;

        m_bIsTraceToCreator         = pCopy.m_bIsTraceToCreator;
        m_bIsStartPosToCreator      = pCopy.m_bIsStartPosToCreator;
        m_vStaticStartPosition      = pCopy.m_vStaticStartPosition;
        m_vPositionOffset           = pCopy.m_vPositionOffset;

        m_fMass                     = pCopy.m_fMass;
        m_fStartSpeed               = pCopy.m_fStartSpeed;
        m_fAddSpeed                 = pCopy.m_fAddSpeed;
        m_vStartDirection           = pCopy.m_vStartDirection;
        m_bIsRandomStartDirection   = pCopy.m_bIsRandomStartDirection;
        m_vForce                    = pCopy.m_vForce;

        m_bIsUseGuideSystem         = pCopy.m_bIsUseGuideSystem;
        m_iNotGuideTick             = pCopy.m_iNotGuideTick;
        m_fGuideAngleSpeed          = pCopy.m_fGuideAngleSpeed;
        m_bIsUseCuvGuideAngleSpeed  = pCopy.m_bIsUseCuvGuideAngleSpeed;
        m_pGuideCuvAngleSpeed       = pCopy.m_pGuideCuvAngleSpeed;

        m_pAnimationClip            = pCopy.m_pAnimationClip;
        m_pAnimTarget               = pCopy.m_pAnimTarget;

        m_pSoundInfo                = new List<SHDamageSoundInfo>(pCopy.m_pSoundInfo);
        m_pEffectInfo               = new List<SHDamageEffectInfo>(pCopy.m_pEffectInfo);
    }
}