﻿using System.Collections;
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
    [SerializeField] private Camera    m_pLocalBlurCamera  = null;
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
    [HideInInspector] private static Camera    m_pBlurCamera = null;
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
        m_pBlurCamera    = m_pLocalBlurCamera;
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
        m_pBlurCamera    = null;
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
    #endregion
}
