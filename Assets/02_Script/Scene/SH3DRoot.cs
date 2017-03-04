using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Singleton
    private static Transform    m_pRoot       = null;
    private static Transform    m_pDamageRoot = null;
    private static Transform    m_pCameraRoot = null;
    private static Camera       m_pMainCamera = null;
    private static Camera       m_pBlurCamera = null;
    #endregion


    #region Members : Inspector
    public Transform    m_pLocalDMGRoot    = null;
    public Transform    m_pLocalCameraRoot = null;
    public Camera       m_pLocalMainCamera = null;
    public Camera       m_pLocalBlurCamera = null;
    #endregion


    #region System Functions
    void Awake()
    {
        m_pRoot         = transform;
        m_pDamageRoot   = m_pLocalDMGRoot;
        m_pCameraRoot   = m_pLocalCameraRoot;
        m_pMainCamera   = m_pLocalMainCamera;
        m_pBlurCamera   = m_pLocalBlurCamera;
    }
    void OnDestroy()
    {
        if (m_pRoot != transform)
            return;

        m_pRoot       = null;
        m_pDamageRoot = null;
        m_pMainCamera = null;
        m_pBlurCamera = null;
    }
    void Update()
    {
        var vPlayerPos = Single.Player.GetLocalPosition();
        SetCameraPosX(vPlayerPos.x);
        SetCameraPosZ(vPlayerPos.z);
    }
    #endregion


    #region Interface Functions
    public static Transform GetRoot()
    {
        return m_pRoot;
    }
    public static Transform GetDMGRoot()
    {
        return m_pDamageRoot;
    }
    public static Transform GetCameraRoot()
    {
        return m_pCameraRoot;
    }
    public static Camera GetMainCamera()
    {
        return m_pMainCamera;
    }
    public static Camera GetBlurCamera()
    {
        return m_pBlurCamera;
    }
    public static void PlayCameraShake()
    {
        if (null == m_pMainCamera)
            return;

        var pAnim = m_pMainCamera.GetComponent<Animation>();
        if (null == pAnim)
            return;

        pAnim.Play();
    }
    public static void SetActiveBlurCamera(bool bIsActive)
    {
        if (null == m_pBlurCamera)
            return;

        m_pBlurCamera.gameObject.SetActive(bIsActive);
    }
    #endregion


    #region Utility Functions
    void SetCameraPosX(float fX)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = m_pCameraRoot.localPosition;
        vLocalPos.x = fX;
        m_pCameraRoot.localPosition = vLocalPos;
    }
    void SetCameraPosY(float fY)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = m_pCameraRoot.localPosition;
        vLocalPos.y = fY;
        m_pCameraRoot.localPosition = vLocalPos;
    }
    void SetCameraPosZ(float fZ)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = m_pCameraRoot.localPosition;
        vLocalPos.z = fZ;
        m_pCameraRoot.localPosition = vLocalPos;
    }
    #endregion
}
