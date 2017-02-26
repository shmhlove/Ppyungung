using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Singleton
    private static Transform    m_pRoot       = null;
    private static Camera       m_pCamera     = null;
    private static Camera       m_pBlurCamera = null;
    #endregion


    #region Members : Inspector
    public Camera m_pLocalMainCamera = null;
    public Camera m_pLocalBlurCamera = null;
    #endregion


    #region System Functions
    void Awake()
    {
        m_pRoot         = transform;
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
}
