using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHMonster : SHInGame_Component
{
    #region Members
    public List<SHState> m_pMonsters    = new List<SHState>();
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
        var pMonsters = new List<SHState>(m_pMonsters);
        SHUtils.ForToList(pMonsters, (pMonster) =>
        {
            pMonster.FrameMove();
        });
    }
    public override void SetPause(bool bIsPause)
    {
        base.SetPause(bIsPause);

        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.SetPauseAnimation(bIsPause);
        });
    }
    #endregion


    #region Interface Functions
    public void StartMonster()
    {
        SHCoroutine.Instance.StopRoutine(m_pCorOnCheckGen);
        SHCoroutine.Instance.StartRoutine(m_pCorOnCheckGen = OnCoroutineToCheckGen());
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.SetPause(false);
        });
    }
    public void StopMonster()
    {
        SHCoroutine.Instance.StopRoutine(m_pCorOnCheckGen);
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.SetPause(true);
        });
    }
    public void AllKillMonster()
    {
        var pMonsters = new List<SHState>(m_pMonsters);
        SHUtils.ForToList(pMonsters, (pMonster) =>
        {
            pMonster.ChangeState(pMonster.GetKillState());
        });
    }
    public void DeleteMonster(SHState pMonster)
    {
        Single.ObjectPool.Return(pMonster.gameObject);
        m_pMonsters.Remove(pMonster);
    }
    public void AllDeleteMonster()
    {
        var pMonsters = new List<SHState>(m_pMonsters);
        SHUtils.ForToList(pMonsters, (pMonster) =>
        {
            DeleteMonster(pMonster);
        });
    }
    public SHState GetNearMonster(Vector3 vPos)
    {
        float   fMinDest     = float.MaxValue;
        SHState pNearMonster = null;
        foreach(var pMonster in m_pMonsters)
        {
            var fDest = Vector3.Distance(pMonster.GetLocalPosition(), vPos);
            if (fDest < fMinDest)
            {
                fMinDest     = fDest;
                pNearMonster = pMonster;
            }
        }

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
                if (SHHard.m_iMonMaxCount <= m_pMonsters.Count)
                    break;

                GenMonster();
                
                yield return new WaitForSeconds(SHMath.Random(0.0f, SHHard.m_fMonGenDaly));
            }

            yield return new WaitForSeconds(SHHard.m_fMonGenDaly);
        }
    }
    #endregion


    #region Utility Functions
    void GenMonster()
    {
        var pMonster = Single.ObjectPool.Get<SHMonMouse>("MonMouse");
        {
            pMonster.SetParent(SH3DRoot.GetRootToMonster());
            pMonster.SetLocalPosition(GetGenPosition(pMonster));
            pMonster.SetLocalScale(pMonster.m_vStartScale * SHHard.m_fUnitScale);
            pMonster.SetActive(true);
            pMonster.InitMonster(pMonster.GetInstanceID());
            pMonster.SetName(string.Format("MonMouse_{0}", pMonster.m_iMonsterID));
        }
        m_pMonsters.Add(pMonster);
    }
    Vector3 GetGenPosition(SHState pMonster)
    {
        if (null == pMonster)
            return Vector3.zero;

        var pMainCamera = SH3DRoot.GetMainCamera();
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
            vGenPosition.z = vGenPosition.z + (SHMath.Random(-Math.Abs(vSides[1].z), Math.Abs(vSides[1].z)));

        vGenPosition.y = 0.0f;

        return vGenPosition;
    }
    #endregion
}