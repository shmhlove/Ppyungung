using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Singleton
    private static Transform    m_pRoot     = null;
    private static Camera       m_pCamera   = null;
    #endregion
    

    #region System Functions
    void Awake()
    {
        m_pRoot   = transform;
        m_pCamera = m_pRoot.GetComponentInChildren<Camera>();
    }
    void OnDestroy()
    {
        if (m_pRoot != transform)
            return;

        m_pRoot   = null;
        m_pCamera = null;
    }
    #endregion


    #region Interface Functions
    public static Transform GetRoot()
    {
        return m_pRoot;
    }

    public static Camera GetCamera()
    {
        return m_pCamera;
    }
    public static void PlayCameraShake()
    {
        var pAnim = m_pCamera.GetComponent<Animation>();
        if (null == pAnim)
            return;

        pAnim.Play();
    }
    #endregion
}
