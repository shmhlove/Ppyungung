using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH3DRoot : SHSingleton<SH3DRoot>
{
    #region Members : Inspector
    [SerializeField]
    private float m_fBasicWidth = 1280.0f;
    [SerializeField]
    private float m_fBasicHeight = 720.0f;
    [SerializeField]
    private Transform m_pRootToDMG = null;
    [SerializeField]
    private Transform m_pRootCamera = null;
    [SerializeField]
    private Transform m_pRootPlayer = null;
    [SerializeField]
    private Transform m_pRootMonster = null;
    [SerializeField]
    private Transform m_pRootEffect = null;
    [SerializeField]
    private Transform m_pRootBG = null;
    [SerializeField]
    private SH3DCamera m_pMainCamera = null;
    #endregion


    #region Override Functions
    public override void Awake()
    {
        SetResoultion(GetMainCamera().GetCamera());
    }
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    #endregion


    #region Interface Function
    public Transform GetRootToDMG()
    {
        return m_pRootToDMG;
    }
    public Transform GetRootCamera()
    {
        return m_pRootCamera;
    }
    public Transform GetRootPlayer()
    {
        return m_pRootPlayer;
    }
    public Transform GetRootMonster()
    {
        return m_pRootMonster;
    }
    public Transform GetRootEffect()
    {
        return m_pRootEffect;
    }
    public Transform GetRootBG()
    {
        return m_pRootBG;
    }
    public SH3DCamera GetMainCamera()
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
        float fScaleHeight = fWindowAspect / fTargetAspect;

        if (1.0f > fScaleHeight)
        {
            var pRect = pCamera.rect;
            pRect.width = 1.0f;
            pRect.height = fScaleHeight;
            pRect.x = 0.0f;
            pRect.y = (1.0f - fScaleHeight) / 2.0f;
            pCamera.rect = pRect;
        }
        else
        {
            var pRect = pCamera.rect;
            pRect.width = fScaleHeight;
            pRect.height = 1.0f;
            pRect.x = (1.0f - fScaleHeight) / 2.0f;
            pRect.y = 0.0f;
            pCamera.rect = pRect;
        }
    }
    #endregion
}
