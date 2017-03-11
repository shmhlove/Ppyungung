﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicBGMChannels    = System.Collections.Generic.Dictionary<eSoundBGMChannel, UnityEngine.AudioSource>;
using DicEffectChannels = System.Collections.Generic.Dictionary<eSoundEffectChannel, UnityEngine.AudioSource>;
using DicTables         = System.Collections.Generic.Dictionary<string, SHSound_Table>;

public enum eSoundBGMChannel
{
    Main,
    Sub_1,
}

public enum eSoundEffectChannel
{
    Main,
    Sub_Dash,
    Sub_Explosion,
    Sub_GetCoin,
    Sub_GetItem,
    Sub_Shoot,
}

public class SHSound : SHSingleton<SHSound>
{
    #region Members : Channels
    private DicBGMChannels    m_dicBGMChannel    = new DicBGMChannels();
    private DicEffectChannels m_dicEffectChannel = new DicEffectChannels();
    
    #endregion


    #region Members : Table
    private DicTables      m_dicSoundTable = new DicTables();
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        CreateTable();
        CreateBGMChannel();
        CreateEffectChannel();
    }
    public override void OnFinalize()
    {
        StopAllCoroutines();
    }
    #endregion


    #region Interface Functions
    public void PlayBGM(string strName)
    {
        if (false == m_dicSoundTable.ContainsKey(strName))
        {
            Debug.LogError("SHSound::PlayBGM() - Not Found TableInfo!!");
            return;
        }
        
        var pInfo = m_dicSoundTable[strName];
        m_dicBGMChannel[pInfo.m_eBGMChannel].clip   = Single.Resource.GetSound(pInfo.m_strFileName);
        m_dicBGMChannel[pInfo.m_eBGMChannel].loop   = pInfo.m_bIsLoop;
        m_dicBGMChannel[pInfo.m_eBGMChannel].volume = 0.0f;
        m_dicBGMChannel[pInfo.m_eBGMChannel].Play();
        m_dicBGMChannel[pInfo.m_eBGMChannel].Pause();
        m_dicBGMChannel[pInfo.m_eBGMChannel].Play();
        StartCoroutine(CoroutineVolumeUP(m_dicBGMChannel[pInfo.m_eBGMChannel]));
    }
    public void StopBGM(string strName)
    {
        if (false == m_dicSoundTable.ContainsKey(strName))
        {
            Debug.LogError("SHSound::StopBGM() - Not Found TableInfo!!");
            return;
        }

        var pInfo = m_dicSoundTable[strName];
        StartCoroutine(CoroutineVolumeDown(
            m_dicBGMChannel[pInfo.m_eBGMChannel], m_dicBGMChannel[pInfo.m_eBGMChannel].Stop));
    }
    public void PlayEffect(string strName)
    {
        if (false == m_dicSoundTable.ContainsKey(strName))
        {
            Debug.LogError("SHSound::PlayEffect() - Not Found TableInfo!!");
            return;
        }
        
        var pInfo = m_dicSoundTable[strName];
        m_dicEffectChannel[pInfo.m_eEffectChannel].clip   = Single.Resource.GetSound(pInfo.m_strFileName);
        m_dicEffectChannel[pInfo.m_eEffectChannel].loop   = pInfo.m_bIsLoop;
        m_dicEffectChannel[pInfo.m_eEffectChannel].volume = 1.0f;
        m_dicEffectChannel[pInfo.m_eEffectChannel].Play();
        m_dicEffectChannel[pInfo.m_eEffectChannel].Pause();
        m_dicEffectChannel[pInfo.m_eEffectChannel].Play();
    }
    public void StopEffect(string strName)
    {
        if (false == m_dicSoundTable.ContainsKey(strName))
        {
            Debug.LogError("SHSound::StopEffect() - Not Found TableInfo!!");
            return;
        }

        var pInfo = m_dicSoundTable[strName];
        m_dicEffectChannel[pInfo.m_eEffectChannel].Stop();
    }
    #endregion


    #region Utility Functions
    void ClearChannel<T1, T2>(Dictionary<T1, T2> dicChannel) where T2 : UnityEngine.Object
    {
        SHUtils.ForToDic(dicChannel, (pKey, pValue) =>
        {
            GameObject.DestroyObject(pValue);
        });
        dicChannel.Clear();
    }
    void CreateTable()
    {
        m_dicSoundTable.Clear();
        m_dicSoundTable.Add("Audio_BGM_InGame",         new SHSound_BGM_InGame());
        m_dicSoundTable.Add("Audio_BGM_OutGame",        new SHSound_BGM_OutGame());
        m_dicSoundTable.Add("Audio_BGM_GameOver",       new SHSound_BGM_GameOver());
        m_dicSoundTable.Add("Audio_Effect_Dash",        new SHSound_Effect_Dash());
        m_dicSoundTable.Add("Audio_Effect_Explosion",   new SHSound_Effect_Explosion());
        m_dicSoundTable.Add("Audio_Effect_GetCoin",     new SHSound_Effect_GetCoin());
        m_dicSoundTable.Add("Audio_Effect_GetItem",     new SHSound_Effect_GetItem());
        m_dicSoundTable.Add("Audio_Effect_Shoot",       new SHSound_Effect_Shoot());
    }
    void CreateBGMChannel()
    {
        ClearChannel(m_dicBGMChannel);
        m_dicBGMChannel.Clear();
        SHUtils.ForToEnum<eSoundBGMChannel>((eBGMChannel) =>
        {
            var pObject = SHGameObject.CreateEmptyObject(eBGMChannel.ToString());
            pObject.transform.SetParent(transform);
            m_dicBGMChannel.Add(eBGMChannel, SHGameObject.GetComponent<AudioSource>(pObject));
        });
    }
    void CreateEffectChannel()
    {
        ClearChannel(m_dicEffectChannel);
        m_dicEffectChannel.Clear();
        SHUtils.ForToEnum<eSoundEffectChannel>((eEffectChannel) =>
        {
            var pObject = SHGameObject.CreateEmptyObject(eEffectChannel.ToString());
            pObject.transform.SetParent(transform);
            m_dicEffectChannel.Add(eEffectChannel, SHGameObject.GetComponent<AudioSource>(pObject));
        });
    }
    #endregion


    #region Coroutine Functions
    private IEnumerator CoroutineVolumeUP(AudioSource pAudio)
    {
        if (null == pAudio)
            yield break;
        
        while(1.0f != pAudio.volume)
        {
            pAudio.volume += 0.02f;
            Mathf.Clamp(pAudio.volume, 0.0f, 1.0f);
            yield return null;
        }
    }
    private IEnumerator CoroutineVolumeDown(AudioSource pAudio, Action pEndCallback)
    {
        if (null == pAudio)
            yield break;

        while (0.0f != pAudio.volume)
        {
            pAudio.volume -= 0.02f;
            Mathf.Clamp(pAudio.volume, 0.0f, 1.0f);
            yield return null;
        }

        pEndCallback();
    }
    #endregion
}
