using UnityEngine;
using System;
using System.Collections;

public class SHUIWidget_Stick : SHMonoWrapper
{
    public enum eState
    {
        Idle,       // 대기상태
        Shooting,   // 날아가는 중
        Crash,      // 충돌 시 : 몬스터를 죽였을때
        Flick,      // 충돌 시 : 몬스터를 못 죽였을때
    }


    #region Members : Inspector
    public float        m_fMoveSpeed     = 1.0f;
    public float        m_fStartPosition = -450.0f;
    public float        m_fEndPosition   = 725.0f;
    public float        m_fReCreateTime  = 0.5f;
    public int          m_iPrice         = 0;
    #endregion


    #region Members : Info
    private eState      m_eState         = eState.Idle;
    public  eStickType  m_eType          = eStickType.None;
    #endregion


    #region Members : Event
    private Action      m_pEventToPass   = null;
    #endregion


    #region System Functions
    public override void FixedUpdate()
    {
        switch (m_eState)
        {
            case eState.Idle:       OnUpdateToIdle();       break;
            case eState.Shooting:   OnUpdateToShooting();   break;
            case eState.Crash:      OnUpdateToCrash();      break;
            case eState.Flick:      OnUpdateToFlick();      break;
        }
    }
    public void ChangeState(eState _eState, params object[] pArgs)
    {
        switch(m_eState = _eState)
        {
            case eState.Idle:       OnChangeToIdle(pArgs);     break;
            case eState.Shooting:   OnChangeToShooting(pArgs); break;
            case eState.Crash:      OnChangeToCrash(pArgs);    break;
            case eState.Flick:      OnChangeToFlick(pArgs);    break;
        }
    }
    private void OnChangeToIdle(params object[] pArgs)
    {
    }
    private void OnUpdateToIdle()
    {
    }
    private void OnChangeToShooting(params object[] pArgs)
    {
    }
    private void OnUpdateToShooting()
    {
        AddLocalPositionY(m_fMoveSpeed);
        CheckCollision();
        CheckPass();
    }
    private void OnChangeToCrash(params object[] pArgs)
    {
        SHGameObject.SetParent(gameObject, ((SHUIWidget_Monster)pArgs[0]).gameObject);
    }
    private void OnUpdateToCrash()
    {
        if (false == IsActive())
        {
            Single.ObjectPool.Return(gameObject);
        }
    }
    private void OnChangeToFlick(params object[] pArgs)
    {

    }
    private void OnUpdateToFlick()
    {
    }
    #endregion


    #region Interface Functions
    public void Initialize(eStickType eType)
    {
        m_eType = eType;
        ChangeState(eState.Idle);
        m_pEventToPass = null;
        SetLocalPositionY(m_fStartPosition);
    }
    public void Shoot(Action pEventPass)
    {
        ChangeState(eState.Shooting);
        m_pEventToPass = pEventPass;
    }
    public int GetPrice()
    {
        return m_iPrice;
    }
    public void SetMovePosForIdle(Vector3 vMovePos)
    {
        if (eState.Idle != m_eState)
            return;

        vMovePos = vMovePos.normalized;

        AddLocalPositionX(5.0f * -vMovePos.x);
        AddLocalPositionY(5.0f * -vMovePos.y);
    }
    #endregion


    #region Utility Functions
    void CheckCollision()
    {
        SHUtils.ForToList(Single.Monster.GetMonsters(), (pMonster) =>
        {
            if (true == pMonster.IsDie())
                return;

            // 충돌체크
            var pMonCollider = pMonster.GetCollider();
            if (false == pMonCollider.bounds.Intersects(m_pCollider.bounds))
                return;

            // 점수처리
            var eDecision = Single.Balance.GetDecision(this, pMonster);
            Single.ScoreBoard.AddScore(pMonster.GetScore(eDecision));
            
            // 상태처리
            pMonster.SetCrash(this);
            this.SetCrash(pMonster);

            // 효과음
            Single.Sound.PlayEffect("Audio_Effect_Crash");
        });
    }
    void CheckPass()
    {
        if (GetLocalPosition().y < m_fEndPosition)
            return;

        if (null != m_pEventToPass)
            m_pEventToPass();

        SetActive(false);
    }
    void SetCrash(SHUIWidget_Monster pMonster)
    {
        if (null == pMonster)
            return;

        if (false == pMonster.IsDie())
            ChangeState(eState.Flick);
        else
            ChangeState(eState.Crash, pMonster);
    }
    #endregion
}
