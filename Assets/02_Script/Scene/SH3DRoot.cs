using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : MonoBehaviour
{
    #region Members : Inspector
    [SerializeField] private Transform m_pLocalRootToDMG   = null;
    [SerializeField] private Transform m_pLocalRootCamera  = null;
    [SerializeField] private Transform m_pLocalRootPlayer  = null;
    [SerializeField] private Transform m_pLocalRootMonster = null;
    [SerializeField] private Transform m_pLocalRootBG      = null;
    [SerializeField] private Camera    m_pLocalMainCamera  = null;
    #endregion


    #region Members : Root
    [HideInInspector] private static Transform m_pRoot          = null;
    [HideInInspector] private static Transform m_pRootToDamage  = null;
    [HideInInspector] private static Transform m_pRootToCamera  = null;
    [HideInInspector] private static Transform m_pRootToPlayer  = null;
    [HideInInspector] private static Transform m_pRootToMonster = null;
    [HideInInspector] private static Transform m_pRootToBG      = null;
    #endregion


    #region Members : Component
    [HideInInspector] private static Camera    m_pMainCamera = null;
    #endregion


    #region System Functions
    void Awake()
    {
        m_pRoot          = transform;
        m_pRootToDamage  = m_pLocalRootToDMG;
        m_pRootToCamera  = m_pLocalRootCamera;
        m_pRootToPlayer  = m_pLocalRootPlayer;
        m_pRootToMonster = m_pLocalRootMonster;
        m_pRootToBG      = m_pLocalRootBG;
        m_pMainCamera    = m_pLocalMainCamera;

        SetResoultion(m_pMainCamera);
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
        m_pRootToBG      = null;
        m_pMainCamera    = null;
    }
    void Update()
    {
        var vPlayerPos = Single.Player.GetLocalPosition();
        SetCameraPosX(vPlayerPos.x);
        SetCameraPosY(vPlayerPos.y);
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
    public static Transform GetRootToBG()
    {
        return m_pRootToBG;
    }
    public static Camera GetMainCamera()
    {
        return m_pMainCamera;
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
    #endregion


    #region Utility Functions
    Vector3 GetCameraPos()
    {
        if (null == m_pRootToCamera)
            return Vector3.zero;

        return m_pRootToCamera.localPosition;
    }
    void SetCameraPos(Vector3 vPos)
    {
        if (null == m_pRootToCamera)
            return;

        m_pRootToCamera.localPosition = vPos;
    }
    void SetCameraPosX(float fX)
    {
        if (null == m_pRootToCamera)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.x = fX;
        SetCameraPos(vLocalPos);
    }
    void SetCameraPosY(float fY)
    {
        if (null == m_pRootToCamera)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.y = fY;
        SetCameraPos(vLocalPos);
    }
    void SetCameraPosZ(float fZ)
    {
        if (null == m_pRootToCamera)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.z = fZ;
        SetCameraPos(vLocalPos);
    }
    void SetResoultion(Camera pCamera)
    {
        if (null == pCamera)
            return;
            
        Debug.LogFormat("카메라 세팅 전 해상도 : {0}", pCamera.rect);
        float fResolutionX = Single.AppInfo.GetScreenWidth() / 3.0f;
        float fResolutionY = Single.AppInfo.GetScreenHeight() / 2.0f;
        if (fResolutionX > fResolutionY)
        {
            float fValue = (fResolutionX - fResolutionY) * 0.5f;
            fValue = fValue / fResolutionX ;
            pCamera.rect = new Rect( fResolutionX * fValue / fResolutionX + pCamera.rect.x * (1.0f - 2.0f * fValue), 
                                     pCamera.rect.y, 
                                     pCamera.rect.width * (1.0f - 2.0f * fValue), 
                                     pCamera.rect.height );
        }
        else if(fResolutionX < fResolutionY)
        {
            float fValue = (fResolutionY - fResolutionX) * 0.5f;
            fValue = fValue / fResolutionY ;
                pCamera.rect = new Rect( pCamera.rect.x, 
                                         fResolutionY * fValue / fResolutionY + pCamera.rect.y * (1.0f - 2.0f * fValue), 
                                         pCamera.rect.width,
                                         pCamera.rect.height * (1.0f - 2.0f * fValue));
        }
        Debug.LogFormat("카메라 세팅 후 해상도 : {0}", pCamera.rect);
    }
    #endregion
}
