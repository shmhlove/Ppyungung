using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Singleton
    private static Transform    m_pRoot       = null;
    private static Transform    m_pCameraRoot = null;
    private static Camera       m_pCamera     = null;
    private static Camera       m_pBlurCamera = null;
    #endregion


    #region Members : Inspector
    public Transform    m_pLocalCameraRoot = null;
    public Camera       m_pLocalMainCamera = null;
    public Camera       m_pLocalBlurCamera = null;
    #endregion


    #region System Functions
    void Awake()
    {
        m_pRoot         = transform;
        m_pCameraRoot   = m_pLocalCameraRoot;
        m_pCamera       = m_pLocalMainCamera;
        m_pBlurCamera   = m_pLocalBlurCamera;
    }
    void OnDestroy()
    {
        if (m_pRoot != transform)
            return;

        m_pRoot       = null;
        m_pCamera     = null;
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

    public static Camera GetMainCamera()
    {
        return m_pCamera;
    }
    public static Camera GetBlurCamera()
    {
        return m_pBlurCamera;
    }
    public static void PlayCameraShake()
    {
        if (null == m_pCamera)
            return;

        var pAnim = m_pCamera.GetComponent<Animation>();
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
