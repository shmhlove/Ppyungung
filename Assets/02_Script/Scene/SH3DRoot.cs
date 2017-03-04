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
    private static bool         m_bIsMove     = false;
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
        // 카메라 이동 : 캐릭터 움직임에 따라
        // var vPlayerPos = Single.Player.GetLocalPosition();
        // SetCameraPosX(vPlayerPos.x);
        // SetCameraPosZ(vPlayerPos.z);

        if (true == m_bIsMove)
        {
            // 카메라 이동 : 상/하로 흐름
            SetCameraPosZ(GetCameraPos().z + SHHard.m_fCameraMoveSpeed);

            // 캐릭터 위치 제한
            Single.Player.LimiteInCamera();

            // 미터 스코어 처리
            Single.ScoreBoard.SetMeter((GetCameraPos().z * 0.002f));
        }
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
    public static void StartCameraMove()
    {
        m_bIsMove = true;
    }
    public static void StopCameraMove()
    {
        m_bIsMove = false;
    }
    #endregion


    #region Utility Functions
    Vector3 GetCameraPos()
    {
        if (null == m_pCameraRoot)
            return Vector3.zero;

        return m_pCameraRoot.localPosition;
    }
    void SetCameraPos(Vector3 vPos)
    {
        if (null == m_pCameraRoot)
            return;

        m_pCameraRoot.localPosition = vPos;
    }
    void SetCameraPosX(float fX)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.x = fX;
        SetCameraPos(vLocalPos);
    }
    void SetCameraPosY(float fY)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.y = fY;
        SetCameraPos(vLocalPos);
    }
    void SetCameraPosZ(float fZ)
    {
        if (null == m_pCameraRoot)
            return;

        var vLocalPos = GetCameraPos();
        vLocalPos.z = fZ;
        SetCameraPos(vLocalPos);
    }
    #endregion
}
