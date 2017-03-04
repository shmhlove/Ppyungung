using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SHMonster : SHInGame_Component
{
    #region Members
    public GameObject    m_pMonsterRoot = null;
    public List<SHState> m_pMonsters    = new List<SHState>();
    #endregion


    #region Members : Coroutine
    private IEnumerator m_pCorOnCheckGen = null;
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
    }
    public override void OnFinalize()
    {
        StopMonster();
    }
    public override void OnFrameMove() { }
    #endregion


    #region Interface Functions
    public void StartMonster()
    {
        SHCoroutine.Instance.StartRoutine(m_pCorOnCheckGen = OnCoroutineToCheckGen());
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.m_bIsStop = false;
        });
    }
    public void StopMonster()
    {
        SHCoroutine.Instance.StopRoutine(m_pCorOnCheckGen);
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.m_bIsStop = true;
        });
    }
    public void AllKillMonster()
    {
        SHUtils.ForToList(m_pMonsters, (pMonster) =>
        {
            pMonster.ChangeState(4);
        });
    }
    public void DeleteMonster(SHState pMonster)
    {
        DelMonster(pMonster);
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
                yield return null;
            }
                
            yield return new WaitForSeconds(SHHard.m_fMonGenDaly);
        }
    }
    #endregion


    #region Utility Functions
    void AddMonster(SHState pMonster)
    {
        m_pMonsters.Add(pMonster);
    }
    void DelMonster(SHState pMonster)
    {
        m_pMonsters.Remove(pMonster);
    }
    void GenMonster()
    {
        var pMonster = Single.ObjectPool.Get<SHMonMouse>("MonMouse");
        pMonster.Initialize(pMonster.GetInstanceID());
        pMonster.SetActive(true);
        pMonster.SetParent(GetRoot());
        pMonster.SetLocalPosition(GetGenPosition(pMonster));
        pMonster.SetLocalScale(pMonster.m_vStartScale * SHHard.m_fUnitScale);
        pMonster.SetName(string.Format("{0}_{1}", pMonster.GetName(), pMonster.m_iMonsterID));
        AddMonster(pMonster);
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
        var vGenPosition = vSides[iRandom] + (vDirection * 700.0f);
        
        // Top or Bottom
        if ((1 == iRandom) || (3 == iRandom))
            vGenPosition.x = vGenPosition.x + (SHMath.Random(-Math.Abs(vSides[0].x), Math.Abs(vSides[0].x)));

        // Left of Right
        if ((0 == iRandom) || (2 == iRandom))
            vGenPosition.z = vGenPosition.z + (SHMath.Random(-Math.Abs(vSides[1].z), Math.Abs(vSides[1].z)));

        vGenPosition.y = 0.0f;

        return vGenPosition;
    }
    GameObject GetRoot()
    {
        if (null == m_pMonsterRoot)
        {
            m_pMonsterRoot = SHGameObject.CreateEmptyObject("Monster");
            m_pMonsterRoot.transform.SetParent(SH3DRoot.GetRoot());
        }

        return m_pMonsterRoot;
    }
    #endregion
}