using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Inspector
    [SerializeField] private float     m_fBasicWidth       = 1280.0f;
    [SerializeField] private float     m_fBasicHeight      = 720.0f;
    [SerializeField] private Transform m_pLocalRootToDMG   = null;
    [SerializeField] private Transform m_pLocalRootCamera  = null;
    [SerializeField] private Transform m_pLocalRootPlayer  = null;
    [SerializeField] private Transform m_pLocalRootMonster = null;
    [SerializeField] private Transform m_pLocalRootEffect  = null;
    [SerializeField] private Transform m_pLocalRootBG      = null;
    [SerializeField] private SH3DCamera m_pLocalMainCamera  = null;
    #endregion


    #region Members : Root
    [HideInInspector] private static Transform m_pRoot          = null;
    [HideInInspector] private static Transform m_pRootToDamage  = null;
    [HideInInspector] private static Transform m_pRootToCamera  = null;
    [HideInInspector] private static Transform m_pRootToPlayer  = null;
    [HideInInspector] private static Transform m_pRootToMonster = null;
    [HideInInspector] private static Transform m_pRootToEffect  = null;
    [HideInInspector] private static Transform m_pRootToBG      = null;
    #endregion


    #region Members : Component
    [HideInInspector] private static SH3DCamera m_pMainCamera = null;
    #endregion


    #region System Functions
    void Awake()
    {
        m_pRoot          = transform;
        m_pRootToDamage  = m_pLocalRootToDMG;
        m_pRootToCamera  = m_pLocalRootCamera;
        m_pRootToPlayer  = m_pLocalRootPlayer;
        m_pRootToMonster = m_pLocalRootMonster;
        m_pRootToEffect  = m_pLocalRootEffect;
        m_pRootToBG      = m_pLocalRootBG;
        m_pMainCamera    = m_pLocalMainCamera;

        SetResoultion(Single.MainCamera.GetCamera());
    }
    void OnDestroy()
    {
        if (m_pRoot != transform)
            return;

        m_pRoot          = null;
        m_pRootToDamage  = null;
        m_pRootToCamera  = null;
        m_pRootToPlayer  = null;
        m_pRootToMonster = null;
        m_pRootToEffect  = null;
        m_pRootToBG      = null;
        m_pMainCamera    = null;
    }

    #endregion


    #region Interface Functions
    public static Transform GetRoot()
    {
        return m_pRoot;
    }
    public static Transform GetRootToDMG()
    {
        return m_pRootToDamage;
    }
    public static Transform GetRootToCamera()
    {
        return m_pRootToCamera;
    }
    public static Transform GetRootToPlayer()
    {
        return m_pRootToPlayer;
    }
    public static Transform GetRootToMonster()
    {
        return m_pRootToMonster;
    }
    public static Transform GetRootToEffect()
    {
        return m_pRootToEffect;
    }
    public static Transform GetRootToBG()
    {
        return m_pRootToBG;
    }
    public static SH3DCamera GetMainCamera()
    {
        return m_pMainCamera;
    }
    #endregion


    #region Utility Functions
    void SetResoultion(Camera pCamera)
    {
        if (null == pCamera)
            return;
        
        float fTargetAspect = m_fBasicWidth / m_fBasicHeight;
        float fWindowAspect = (float)Single.AppInfo.GetScreenWidth() / (float)Single.AppInfo.GetScreenHeight();
        float fScaleHeight  = fWindowAspect / fTargetAspect;
        
        if (1.0f > fScaleHeight)
        {
            var pRect = pCamera.rect;
            pRect.width  = 1.0f;
            pRect.height = fScaleHeight;
            pRect.x      = 0.0f;
            pRect.y      = (1.0f - fScaleHeight) / 2.0f;
            pCamera.rect = pRect;
        }
        else
        {
            var pRect = pCamera.rect;
            pRect.width  = fScaleHeight;
            pRect.height = 1.0f;
            pRect.x      = (1.0f - fScaleHeight) / 2.0f;
            pRect.y      = 0.0f;
            pCamera.rect = pRect;
        }
    }
    #endregion
}
