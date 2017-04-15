using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHMonster : SHInGame_Component
{
    #region Members : Info
    public List<SHBaseMonster> m_pMonsters = new List<SHBaseMonster>();
    #endregion


    #region Members : Cheat
    public bool m_bIsStopAutoGen = false;
    public bool m_bIsStopMonster = false;
    #endregion


    #region Members : Coroutine
    private IEnumerator m_pCorOnCheckGen = null;
    #endregion


    #region Virtual Functions
    public override void OnInitialize() { }
    public override void OnFinalize()
    {
        AllDeleteMonster();
    }
    public override void OnFrameMove()
    {
        ForMonsters((pMonster) =>
        {
            pMonster.FrameMove();
        });
    }
    public override void SetPause(bool bIsPause)
    {
        base.SetPause(bIsPause);

        ForMonsters((pMonster) =>
        {
            pMonster.SetPauseAnimation(bIsPause);
        });
    }
    #endregion


    #region Interface Functions
    public void StartMonster()
    {
        ForMonsters((pMonster) =>
        {
            pMonster.SetPause(false);
        });
    }
    public void StopMonster()
    {        
        ForMonsters((pMonster) =>
        {
            pMonster.SetPause(true);
        });
    }
    public void StartGen()
    {
        StopGen();
        SHCoroutine.Instance.StartRoutine(m_pCorOnCheckGen = OnCoroutineToCheckGen());
    }
    public void StopGen()
    {
        SHCoroutine.Instance.StopRoutine(m_pCorOnCheckGen);
        m_pCorOnCheckGen = null;
    }
    public void AllKillMonster()
    {
        ForMonsters((pMonster) =>
        {
            pMonster.ChangeState(pMonster.GetKillState());
        });
    }
    public void AllDieMonster()
    {
        ForMonsters((pMonster) =>
        {
            pMonster.ChangeState(pMonster.GetDieState());
        });
    }
    public void DeleteMonster(SHBaseMonster pMonster)
    {
        Single.ObjectPool.Return(pMonster.gameObject);
        m_pMonsters.Remove(pMonster);
    }
    public void AllDeleteMonster()
    {
        ForMonsters((pMonster) =>
        {
            DeleteMonster(pMonster);
        });
    }
    public SHBaseMonster GenMonster(string strMonster, Vector3 vGetPosition)
    {
        var pMonster = Single.ObjectPool.Get<SHBaseMonster>(strMonster);
        {
            pMonster.SetParent(SH3DRoot.GetRootToMonster());
            pMonster.SetLocalPosition(vGetPosition);
            pMonster.SetLocalScale(pMonster.m_vStartScale * SHHard.m_fUnitScale);
            pMonster.SetActive(true);
            pMonster.InitMonster(pMonster.GetInstanceID());
            pMonster.SetName(string.Format("{0}_{1}", strMonster, pMonster.m_iMonsterID));
        }
        m_pMonsters.Add(pMonster);

        return pMonster;
    }
    public SHState GetNearMonster(Vector3 vPos)
    {
        float   fMinDest     = float.MaxValue;
        SHState pNearMonster = null;
        ForMonsters((pMonster) =>
        {
            var fDest = Vector3.Distance(pMonster.GetLocalPosition(), vPos);
            if (fDest < fMinDest)
            {
                fMinDest     = fDest;
                pNearMonster = pMonster;
            }
        });

        return pNearMonster;
    }
    public int GetMonsterCount()
    {
        return m_pMonsters.Count;
    }
    #endregion
    

    #region Coroutine Functions
    IEnumerator OnCoroutineToCheckGen()
    {
        while (true)
        {
            for (int iLoop = 0; iLoop < SHHard.m_iMonMaxGen; ++iLoop)
            {
                if (true == m_bIsStopAutoGen)
                    break;

                if (SHHard.m_iMonMaxCount <= m_pMonsters.Count)
                    break;
                
                var pPhaseInfo = Single.GameState.GetCurrentPhaseInfo();
                var strMonster = SHMath.RandomW(pPhaseInfo.GetMonsterList(), pPhaseInfo.GetWeightList());
                GenMonster(strMonster, GetGenPosition());
                
                yield return new WaitForSeconds(SHMath.Random(0.0f, SHHard.m_fMonGenDaly));
            }

            yield return new WaitForSeconds(SHHard.m_fMonGenDaly);
        }
    }
    #endregion


    #region Utility Functions
    Vector3 GetGenPosition()
    {
        var pMainCamera = Single.MainCamera.GetCamera();
        if (null == pMainCamera)
            return Vector3.zero;

        var vSides       = pMainCamera.GetSides(Mathf.Lerp(pMainCamera.nearClipPlane, pMainCamera.farClipPlane, 0.5f), null);
        var iRandom      = SHMath.Random(0, 4);
        var vDirection   = vSides[iRandom].normalized;
        var vGenPosition = vSides[iRandom] + (vDirection * 500.0f);
        
        // Top or Bottom
        if ((1 == iRandom) || (3 == iRandom))
            vGenPosition.x = vGenPosition.x + (SHMath.Random(-Math.Abs(vSides[0].x), Math.Abs(vSides[0].x)));
        
        // Left of Right
        if ((0 == iRandom) || (2 == iRandom))
            vGenPosition.y = vGenPosition.y + (SHMath.Random(-Math.Abs(vSides[1].y), Math.Abs(vSides[1].y)));

        vGenPosition.z = 0.0f;

        return vGenPosition;
    }
    void ForMonsters(Action<SHBaseMonster> pCallback)
    {
        SHUtils.ForToList(new List<SHBaseMonster>(m_pMonsters), (pMonster) =>
        {
            pCallback(pMonster);
        });
    }
    #endregion
}