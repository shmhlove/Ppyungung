using UnityEngine;
using System;
using System.Collections;

public class SHMonoWrapper : MonoBehaviour
{
    #region Members : Transform
    [HideInInspector] public Vector3     m_vStartPosition    = Vector3.zero;
    [HideInInspector] public Vector3     m_vStartScale       = Vector3.zero;
    [HideInInspector] public Quaternion  m_qStartRotation    = Quaternion.identity;
    #endregion


    #region Members : Physics
    [HideInInspector] public float       m_fSpeed            = 0.0f;
    [HideInInspector] public Vector3     m_vDirection        = Vector3.zero;
    [HideInInspector] public Collider    m_pCollider         = null;
    #endregion


    #region Members : Animation
    [HideInInspector] public Animation   m_pAnim             = null;
    [HideInInspector] public bool        m_bIsAnimPlaying    = false;
    #endregion

    
    #region System Functions
    public virtual void Awake()
    {
        m_vStartPosition = transform.localPosition;
        m_vStartScale    = transform.localScale;
        m_qStartRotation = transform.localRotation;
    }
    public virtual void Start() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void OnDestroy() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void OnCrashDamage(SHMonoWrapper pCrashObject) { }
    #endregion


    #region Interface : Physics
    public void InitPhysics()
    {
        m_fSpeed     = 0.0f;
        m_vDirection = Vector3.zero;
    }
    public Vector3 GetSpeed()
    {
        return m_vDirection * m_fSpeed;
    }
    public void SetSpeed(float fSpeed)
    {
        m_fSpeed     = fSpeed;
    }
    public void SetSpeed(Vector3 vSpeed)
    {
        m_fSpeed     = vSpeed.magnitude;
        m_vDirection = vSpeed.normalized;
    }
    public Collider GetCollider()
    {
        if (null == m_pCollider)
            m_pCollider = SHGameObject.GetComponent<Collider>(gameObject);

        return m_pCollider;
    }
    #endregion


    #region Interface : Active
    public void SetParent(GameObject pParent)
    {
        if (null == pParent)
            return;

        transform.SetParent(pParent.transform);
    }
    public void SetParent(Transform pParent)
    {
        if (null == pParent)
            return;

        transform.SetParent(pParent);
    }
    #endregion


    #region Interface : Active
    public void SetActive(bool bIsActive)
    {
        if (IsActive() == bIsActive)
            return;
        
        gameObject.SetActive(bIsActive);
    }
    public bool IsActive()
    {
        return gameObject.activeInHierarchy;
    }
    #endregion

    
    #region Interface : Position
    public void SetPosition(Vector3 vPos)
    {
        if (true == SHUtils.IsNan(vPos))
            return;

        gameObject.transform.position = vPos;
    }
    public void SetPositionX(float fX)
    {
        Vector3 vPos = GetPosition();
        vPos.x = fX;
        SetPosition(vPos);
    }
    public void SetPositionY(float fY)
    {
        Vector3 vPos = GetPosition();
        vPos.y = fY;
        SetPosition(vPos);
    }
    public void AddPosition(Vector3 vPos)
    {
        gameObject.transform.position = (GetPosition() + vPos);
    }
    public void AddPositionX(float fX)
    {
        Vector3 vPos = GetPosition();
        vPos.x += fX;
        SetPosition(vPos);
    }
    public void AddPositionY(float fY)
    {
        Vector3 vPos = GetPosition();
        vPos.y += fY;
        SetPosition(vPos);
    }
    public void SetLocalPosition(Vector3 vPos)
    {
        if (true == SHUtils.IsNan(vPos))
            return;

        gameObject.transform.localPosition = vPos;
    }
    public void SetLocalPositionX(float fX)
    {
        Vector3 vPos = GetLocalPosition();
        vPos.x = fX;
        SetLocalPosition(vPos);
    }
    public void SetLocalPositionY(float fY)
    {
        Vector3 vPos = GetLocalPosition();
        vPos.y = fY;
        SetLocalPosition(vPos);
    }
    public void AddLocalPosition(Vector3 vPos)
    {
        gameObject.transform.localPosition = (GetLocalPosition() + vPos);
    }
    public void AddLocalPositionX(float fX)
    {
        Vector3 vPos = GetLocalPosition();
        vPos.x += fX;
        SetLocalPosition(vPos);
    }
    public void AddLocalPositionY(float fY)
    {
        Vector3 vPos = GetLocalPosition();
        vPos.y += fY;
        SetLocalPosition(vPos);
    }
    public void AddLocalPositionZ(float fZ)
    {
        Vector3 vPos = GetLocalPosition();
        vPos.z += fZ;
        SetLocalPosition(vPos);
    }
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
    public Vector3 GetLocalPosition()
    {
        return gameObject.transform.localPosition;
    }
    #endregion


    #region Interface : Scale
    public void SetLocalScale(Vector3 vScale)
    {
        if (true == SHUtils.IsNan(vScale))
            return;

        gameObject.transform.localScale = vScale;
    }
    public void SetLocalScaleZ(float fScale)
    {
        Vector3 vScale = GetLocalScale();
        vScale.z = fScale;
        SetLocalScale(vScale);
    }
    public Vector3 GetLocalScale()
    {
        return gameObject.transform.localScale;
    }
    public bool IsZero2Scale()
    {
        Vector3 vScale = GetLocalScale();
        return ((0.0f == vScale.x) && (0.0f == vScale.y));
    }
    #endregion


    #region Interface : Rotate
    public void SetRotate(Vector3 vRotate)
    {
        var pRotation = GetRotate();
        pRotation.eulerAngles = vRotate;
        SetRotate(pRotation);
    }
    public void SetRotate(Quaternion qRotate)
    {
        if (true == SHUtils.IsNan(qRotate))
            return;

        gameObject.transform.rotation = qRotate;
    }
    public void SetLocalRotate(Vector3 vRotate)
    {
        var pRotation = GetLocalRotate();
        pRotation.eulerAngles = vRotate;
        SetLocalRotate(pRotation);
    }
    public void SetLocalRotate(Quaternion qRotate)
    {
        gameObject.transform.localRotation = qRotate;
    }
    public void SetLocalRotateZ(float fValue)
    {
        Quaternion  qRot = GetLocalRotate();
        Vector3     vRet = qRot.eulerAngles;
        vRet.z = fValue;
        SetLocalRotate(vRet);
    }
    public void AddLocalRotateZ(float fValue)
    {
        SetLocalRotateZ(GetLocalRotateZ() + fValue);
    }
    public float GetLocalRotateZ()
    {
        return GetLocalRotate().eulerAngles.z;
    }
    public void SetLocalRotateX(float fValue)
    {
        Quaternion  qRot = GetLocalRotate();
        Vector3     vRet = qRot.eulerAngles;
        vRet.x = fValue;
        SetLocalRotate(vRet);
    }
    public void AddLocalRotateX(float fValue)
    {
        SetLocalRotateX(GetLocalRotateX() + fValue);
    }
    public float GetLocalRotateX()
    {
        return GetLocalRotate().eulerAngles.x;
    }
    public void SetLocalRotateY(float fValue)
    {
        Quaternion  qRot = GetLocalRotate();
        Vector3     vRet = qRot.eulerAngles;
        vRet.y = fValue;
        SetLocalRotate(vRet);
    }
    public void AddLocalRotateY(float fValue)
    {
        SetLocalRotateY(GetLocalRotateY() + fValue);
    }
    public float GetLocalRotateY()
    {
        return GetLocalRotate().eulerAngles.y;
    }
    public Quaternion GetRotate()
    {
        return gameObject.transform.rotation;
    }
    public Quaternion GetLocalRotate()
    {
        return gameObject.transform.localRotation;
    }
    #endregion


    #region Interface : Direction
    public void SetLook(Vector3 vLookPos)
    {
        if (Vector3.zero == vLookPos)
            return;
        
        Vector3 vDirection = (vLookPos - GetPosition()).normalized;
        if (Vector3.zero == vDirection)
            return;

        SetRotate(vDirection);
    }
    public Vector3 GetDirection()
    {
        return GetRotate() * Vector3.forward;
    }
    public Vector3 GetLocalDirection()
    {
        return GetLocalRotate() * Vector3.forward;
    }
    #endregion


    #region Interface : Animation
    Animation GetAnimation(GameObject pAnimObject = null)
    {
        if (null != m_pAnim)     return m_pAnim;
        if (null == pAnimObject) pAnimObject = gameObject;

        return (m_pAnim = SHGameObject.GetComponent<Animation>(pAnimObject));
    }
    public AnimationClip GetAnimClip(GameObject pAnimObject, string strClipName)
    {
        if (true == string.IsNullOrEmpty(strClipName))
            return null;

        var pAnimClip = GetAnimation(pAnimObject).GetClip(strClipName);
        if (null == pAnimClip)
            pAnimClip = Single.Resource.GetAniamiton(strClipName);

        return pAnimClip;
    }
    public bool IsPlaying(string strClipName)
    {
        if (null == m_pAnim)
            return false;

        return m_pAnim.IsPlaying(strClipName);
    }
    public bool PlayAnim(eDirection ePlayDir, GameObject pAnimObject, string strClipName, Action pEndCallback)
    {        
        return PlayAnim(ePlayDir, pAnimObject, GetAnimClip(pAnimObject, strClipName), pEndCallback);
    }
    public bool PlayAnim(eDirection ePlayDir, GameObject pAnimObject, AnimationClip pClip, Action pEndCallback)
    {
        if (null == pClip)
        {
            if (null != pEndCallback)
                pEndCallback();
            return false;
        }

        var pAnim = GetAnimation(pAnimObject);
        if (false == pAnim.gameObject.activeInHierarchy)
        {
            if (null != pEndCallback)
                pEndCallback();
            return false;
        }
        
        if (null == pAnim.GetClip(pClip.name))
            pAnim.AddClip(pClip, pClip.name);

        if (1.0f == Time.timeScale)
        {
            var pState = pAnim[pClip.name];
            pState.normalizedTime = (eDirection.Front == ePlayDir) ? 0.0f :  1.0f;
            pState.speed          = (eDirection.Front == ePlayDir) ? 1.0f : -1.0f;

            pAnim.Stop();
            pAnim.Play(pClip.name);

            if (WrapMode.Loop != pState.wrapMode)
                StartCoroutine(CoroutinePlayAnim_WaitTime(pState.length, pEndCallback));
        }
        else
        {
            switch (ePlayDir)
            {
                case eDirection.Front:
                    StartCoroutine(CoroutinePlayAnim_UnScaledForward(pAnimObject, pAnim[pClip.name], pEndCallback));
                    break;
                case eDirection.Back:
                    StartCoroutine(CoroutinePlayAnim_UnScaledBackward(pAnimObject, pAnim[pClip.name], pEndCallback));
                    break;
            }
        }

        return true;
    }
    private IEnumerator CoroutinePlayAnim_WaitTime(float fSec, Action pCallback)
    {
        yield return new WaitForSeconds(fSec);

        if (null != pCallback)
            pCallback();
    }
    private IEnumerator CoroutinePlayAnim_UnScaledForward(GameObject pObject, AnimationState pState, Action pEndCallback)
    {
        m_bIsAnimPlaying = true;

        float fStart     = Time.unscaledTime;
        float fElapsed   = 0.0f;
        
        while (true)
        {
            if ((null == pObject) || (null == pState))
                break;
            
            if (false == pObject.activeInHierarchy)
                break;

            fElapsed = Time.unscaledTime - fStart;
            pState.clip.SampleAnimation(pObject, fElapsed);

            if (pState.length <= fElapsed)
            {
                fStart = Time.unscaledTime;
                if (WrapMode.Loop != pState.wrapMode)
                    break;
            }

            yield return null;
        }

        if ((null != pObject) || (null != pState))
            pState.clip.SampleAnimation(pObject, pState.length);

        if (null != pEndCallback)
            pEndCallback();

        m_bIsAnimPlaying = false;
    }
    private IEnumerator CoroutinePlayAnim_UnScaledBackward(GameObject pObject, AnimationState pState, Action pEndCallback)
    {
        m_bIsAnimPlaying = true;

        float fStart    = Time.unscaledTime;
        float fElapsed  = 0.0f;

        while (true)
        {
            if ((null == pObject) || (null == pState))
                break;

            if (false == pObject.activeInHierarchy)
                break;

            fElapsed = pState.length - (Time.unscaledTime - fStart);
            pState.clip.SampleAnimation(pObject, fElapsed);

            if (0.0f >= fElapsed)
            {
                fStart = Time.unscaledTime;
                if (WrapMode.Loop != pState.wrapMode)
                    break;
            }

            yield return null;
        }

        if ((null != pObject) || (null != pState))
            pState.clip.SampleAnimation(pObject, 0.0f);

        if (null != pEndCallback)
            pEndCallback();

        m_bIsAnimPlaying = false;
    }
    #endregion


    #region Interface : Object
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    #endregion


    #region Interface : Helpper
    public void SetStartTransform()
    {
        SetLocalPosition(m_vStartPosition);
        SetLocalRotate(m_qStartRotation);
        SetLocalScale(m_vStartScale);
    }
    #endregion
}