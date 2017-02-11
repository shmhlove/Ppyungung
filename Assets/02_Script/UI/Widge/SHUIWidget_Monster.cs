using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SHUIWidge_Monster_Info
{
    public float   m_fSpeed       = 1.0f;
    public Vector3 m_vDieSpeed    = Vector3.zero;
    public int     m_iHP          = 1;
    public int     m_iBadScore    = 1;
    public int     m_iNormalScore = 2;
    public int     m_iGoodScore   = 3;
    public int     m_iMinCoin     = 0;
    public int     m_iMaxCoin     = 0;
    public int     m_iPrice       = 0;

    public void Copy(SHUIWidge_Monster_Info pInfo)
    {
        m_fSpeed       = pInfo.m_fSpeed;
        m_vDieSpeed    = pInfo.m_vDieSpeed;
        m_iHP          = pInfo.m_iHP;
        m_iBadScore    = pInfo.m_iBadScore;
        m_iNormalScore = pInfo.m_iNormalScore;
        m_iGoodScore   = pInfo.m_iGoodScore;
        m_iMinCoin     = pInfo.m_iMinCoin;
        m_iMaxCoin     = pInfo.m_iMaxCoin;
        m_iPrice       = pInfo.m_iPrice;
    }
}

public class SHUIWidget_Monster : SHMonoWrapper
{
    public enum eState
    {
        Idle,
        Crash,
        Die,
    }

    #region Members : Inspector
    [SerializeField]
    public SHUIWidge_Monster_Info m_pOriginalInfo = new SHUIWidge_Monster_Info();
    public TweenPosition          m_pTweenMove   = null;
    public AnimationClip          m_pDie_Left    = null;
    public AnimationClip          m_pDie_Right   = null;
    #endregion


    #region Members : Info
    public eMonsterType            m_eType       = eMonsterType.None;
    private eState                 m_eState      = eState.Idle;
    private SHUIWidge_Monster_Info m_pInfo       = new SHUIWidge_Monster_Info();
    #endregion


    #region Virtual Functions
    public override void FixedUpdate()
    {
        switch (m_eState)
        {
            case eState.Idle:   OnUpdateToIdle();  break;
            case eState.Crash:  OnUpdateToCrash(); break;
            case eState.Die:    OnUpdateToDie();   break;
        }
    }
    void ChangeState(eState _eState, params object[] pArgs)
    {
        switch ((m_eState = _eState))
        {
            case eState.Idle:   OnChangeToIdle(pArgs);  break;
            case eState.Crash:  OnChangeToCrash(pArgs); break;
            case eState.Die:    OnChangeToDie(pArgs);   break;
        }
    }
    private void OnChangeToIdle(params object[] pArgs)
    {
        m_pInfo.Copy(m_pOriginalInfo);

        if (null == m_pTweenMove)
            return;
        
        m_pTweenMove.tweenFactor = (float)pArgs[0];
        m_pTweenMove.duration    = (float)pArgs[1];
        m_pTweenMove.from.y      = ((float)pArgs[2]);
        m_pTweenMove.to.y        = ((float)pArgs[2]);
    }
    private void OnUpdateToIdle()
    {
    }
    private void OnChangeToCrash(params object[] pArgs)
    {
        InitPhysicsValue();
        StopMoveTween();
    }
    private void OnUpdateToCrash()
    {
    }
    private void OnChangeToDie(params object[] pArgs)
    {
        InitPhysicsValue();
        StopMoveTween();

		var pStick    = (SHUIWidget_Stick)pArgs[0];
		var eCrashDir = Single.Balance.GetDirection(pStick, this);
        PlayAnim(eDirection.Front, gameObject, (eDirection.Left == eCrashDir) ?
                                                m_pDie_Left : m_pDie_Right, null);

		var eAccuracy  = Single.Balance.GetDecision(pStick, this);
		var iBonusCoin = GetBonusCoin (eAccuracy);
		SHUtils.For(0, iBonusCoin, (iIndex) => 
        {
            var pHUD = Single.UI.GetPanel<SHUIPanel_HUD>("Panel_HUD");
            Single.Damage.AddDamage("Dmg_Coin", new SHAddDamageParam(this, pHUD.GetCoinTarget(), null, null));
        });

		if ((0 != iBonusCoin) && (eDecision.Good == eAccuracy))
			PlayParticle("Particle_Crash_Dust_Big");
        else
			PlayParticle("Particle_Crash_Dust_Small");

        m_fSpeed       = m_pInfo.m_vDieSpeed.magnitude;
        m_vDirection   = m_pInfo.m_vDieSpeed.normalized;
        m_vDirection.x = (eDirection.Left == eCrashDir) ? -m_vDirection.x : m_vDirection.x;
        m_vDirection.x *= SHMath.Random(0.0f, 2.0f);
    }
    private void OnUpdateToDie()
    {
        var vSpeed = GetSpeed();
        {
            SetLocalPosition(
                SHPhysics.CalculationEuler(
                    SHPhysics.m_vGravity * 500.0f, GetLocalPosition(), ref vSpeed, 1.0f));
        }
        SetSpeed(vSpeed);

        SetLocalScale(GetLocalScale() * 0.99f);

        if (-1000.0f > GetLocalPosition().y)
            SetActive(false);
    }
    #endregion


    #region Interface Functions
    public void Initialize(eMonsterType eType, float fFactor, float fSpeed, float fStartPosY)
    {
        m_eType = eType;
        ChangeState(eState.Idle, fFactor, fSpeed, fStartPosY);
    }
    [FuncButton] public void PlayMoveTween()
    {
        if (null == m_pTweenMove)
            return;

        m_pTweenMove.enabled = true;
    }
    [FuncButton] public void StopMoveTween()
    {
        if (null == m_pTweenMove)
            return;

        m_pTweenMove.enabled = false;
    }
    public void SetCrash(SHUIWidget_Stick pStick)
    {
        if (0 == (--m_pInfo.m_iHP))
            ChangeState(eState.Die, pStick);
        else
            ChangeState(eState.Crash);
    }
    public bool IsDie()
    {
        return (eState.Die == m_eState);
    }
    public int GetScore(eDecision eDec)
    {
        switch (eDec)
        {
            case eDecision.Bad:     return m_pInfo.m_iBadScore;
            case eDecision.Normal:  return m_pInfo.m_iNormalScore;
            case eDecision.Good:    return m_pInfo.m_iGoodScore;
        }

        return 0;
    }
	public int GetBonusCoin(eDecision eDec)
	{
		switch (eDec)
		{
            case eDecision.Bad:     return m_pInfo.m_iMinCoin;
		    case eDecision.Normal:  return SHMath.Lerp(m_pInfo.m_iMinCoin, m_pInfo.m_iMaxCoin, 0.5f);
            case eDecision.Good:    return m_pInfo.m_iMaxCoin;
		}
		return 0;
	}
    public int GetPrice()
    {
        if (null == m_pInfo)
            return 0;

        return m_pInfo.m_iPrice;
    }
    #endregion


    #region Utility Functions
    void PlayParticle(string strPrefabName)
    {
        var pEffect = Single.ObjectPool.Get(strPrefabName);
        pEffect.transform.SetParent(Single.UI.GetRootToScene());
        pEffect.transform.localPosition = GetLocalPosition();
        pEffect.transform.localScale    = Vector3.one;
        pEffect.SetActive(true);
    }
    #endregion


    #region Event Handler
    #endregion
}
