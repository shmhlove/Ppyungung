using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class SH3DCamera : SHMonoWrapper
{
    #region Members
    [SerializeField] private Camera m_pCamera = null;
    #endregion


    #region System Functions
    public override void Update()
    {
        if (false == Single.Scene.IsCurrentScene(eSceneType.InGame))
            return;

        var vPlayerPos = Single.Player.GetLocalPosition();
        SetLocalPositionX(vPlayerPos.x);
        SetLocalPositionY(vPlayerPos.y);
    }
    #endregion


    #region Interface Functions
    public Camera GetCamera()
    {
        return m_pCamera;
    }
    public void PlayCameraShake(Action pEndCallback)
    {
        PlayAnim(eDirection.Front, gameObject, "Anim_Camera_Shake", pEndCallback);
    }
    public void PlayCameraGameOver(Action pEndCallback)
    {
        PlayAnim(eDirection.Front, gameObject, "Anim_Camera_GameOver", pEndCallback);
    }
    public void PlayCameraPhase(Action pEndCallback)
    {
        PlayAnim(eDirection.Front, gameObject, "Anim_Camera_Phase", pEndCallback);
    }
    #endregion
}
