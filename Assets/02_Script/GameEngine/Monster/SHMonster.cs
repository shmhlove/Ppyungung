using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Monsters = System.Collections.Generic.List<SHUIWidget_Monster>;

public class SHMonster : SHBaseEngine
{
    #region Members : Object
    private Monsters m_pMonsters = new Monsters();
    #endregion


    #region Members : Info
    private bool bIsCheckCreateMonster = false;
    #endregion


    #region Members : Constants
    private readonly float MIN_CREATE_POS_Y = 50;
    private readonly float MAX_CREATE_POS_Y = 320;
    #endregion


    #region Virtual Functions
    public override void OnFrameMove()
    {
        if (false == bIsCheckCreateMonster)
            return;

        CheckDeleteMonster();
        CheckCreateMonster();
    }
    #endregion


    #region Interface Functions
    public void StartMonster()
    {
        bIsCheckCreateMonster = true;
        ClearMonster();

        var eFirstMon = Single.Balance.GenMonsterTypeForFirst();
        var pMonster  = AddMonster(CreateMonster(eFirstMon, 0.5f, 250.0f)); 
        pMonster.StopMoveTween();
    }
    public void Stop()
    {
        bIsCheckCreateMonster = false;
    }
    public Monsters GetMonsters()
    {
        return m_pMonsters;
    }
    #endregion


    #region Utility Functions
    private void CheckDeleteMonster()
    {
        var pDelete = new Monsters(m_pMonsters);
        SHUtils.ForToList(pDelete, (pMonster) =>
        {
            if (false == pMonster.IsDie())
                return;

            m_pMonsters.Remove(pMonster);
        });
    }
    private void CheckCreateMonster()
    {
        if (0 != m_pMonsters.Count)
            return;

        var pMonster = CreateMonster(
            Single.Balance.GenMonsterType(), GetRandomFactor(), SHMath.Random(MIN_CREATE_POS_Y, MAX_CREATE_POS_Y));
        pMonster.PlayMoveTween();
        AddMonster(pMonster);
    }
    private SHUIWidget_Monster AddMonster(SHUIWidget_Monster pMonster)
    {
        if (null != pMonster)
        {
            m_pMonsters.Add(pMonster);
        }
        return pMonster;
    }
    private SHUIWidget_Monster CreateMonster(eMonsterType eType, float fFactor, float fStartPosY)
    {
        var pMonster = Single.ObjectPool.Get<SHUIWidget_Monster>(SHHard.GetMonsterName(eType));
        if (null == pMonster)
            return null;

        SHGameObject.SetParent(pMonster.transform, Single.UI.GetRootToScene());
        Single.ObjectPool.SetStartTransform(pMonster.gameObject);
        pMonster.SetActive(true);
        pMonster.Initialize(eType, fFactor, Single.Balance.GetMonsterSpeed(), fStartPosY);
        return pMonster;
    }
    private float GetRandomFactor()
    {
        return SHMath.RandomN(new List<float>(){0.0f, 1.0f});
    }
    void ClearMonster()
    {
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.SetActive(false);
        });

        m_pMonsters.Clear();
    }
    #endregion


    #region Event Handler
    #endregion
}
