using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHSound_Table
{
    public string               m_strFileName    = string.Empty;
    public bool                 m_bIsLoop        = false;
    public eSoundBGMChannel     m_eBGMChannel    = eSoundBGMChannel.Main;
    public eSoundEffectChannel  m_eEffectChannel = eSoundEffectChannel.Main;
}

public class SHSound_BGM_InGame : SHSound_Table
{
    public SHSound_BGM_InGame()
    {
        m_strFileName = "Audio_BGM_InGame";
        m_bIsLoop     = true;
        m_eBGMChannel = eSoundBGMChannel.Main;
    }
}

public class SHSound_BGM_OutGame : SHSound_Table
{
    public SHSound_BGM_OutGame()
    {
        m_strFileName = "Audio_BGM_OutGame";
        m_bIsLoop     = true;
        m_eBGMChannel = eSoundBGMChannel.Main;
    }
}

public class SHSound_BGM_GameOver : SHSound_Table
{
    public SHSound_BGM_GameOver()
    {
        m_strFileName = "Audio_BGM_GameOver";
        m_bIsLoop     = false;
        m_eBGMChannel = eSoundBGMChannel.Main;
    }
}

public class SHSound_Effect_Dash : SHSound_Table
{
    public SHSound_Effect_Dash()
    {
        m_strFileName    = "Audio_Effect_Dash";
        m_bIsLoop        = false;
        m_eEffectChannel = eSoundEffectChannel.Sub_Dash;
    }
}

public class SHSound_Effect_Explosion : SHSound_Table
{
    public SHSound_Effect_Explosion()
    {
        m_strFileName    = "Audio_Effect_Explosion";
        m_bIsLoop        = false;
        m_eEffectChannel = eSoundEffectChannel.Sub_Explosion;
    }
}

public class SHSound_Effect_GetCoin : SHSound_Table
{
    public SHSound_Effect_GetCoin()
    {
        m_strFileName    = "Audio_Effect_GetCoin";
        m_bIsLoop        = false;
        m_eEffectChannel = eSoundEffectChannel.Sub_GetCoin;
    }
}

public class SHSound_Effect_GetItem : SHSound_Table
{
    public SHSound_Effect_GetItem()
    {
        m_strFileName    = "Audio_Effect_GetItem";
        m_bIsLoop        = false;
        m_eEffectChannel = eSoundEffectChannel.Sub_GetItem;
    }
}

public class SHSound_Effect_Shoot : SHSound_Table
{
    public SHSound_Effect_Shoot()
    {
        m_strFileName    = "Audio_Effect_Shoot";
        m_bIsLoop        = false;
        m_eEffectChannel = eSoundEffectChannel.Sub_Shoot;
    }
}