using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamage : SHInGame_Component
{
    #region Utility : Object
    string GetNewDamageID(string strPrefabName)
    {
        if (int.MaxValue == ++m_iAddCount)
            m_iAddCount = 0;

        return string.Format("{0}_{1}", strPrefabName, m_iAddCount);
    }
    SHDamageObject CreateDamage(string strPrefabName)
    {
#if UNITY_EDITOR
        var pDamage = Single.ObjectPool.Get<SHDamageObject>(
                strPrefabName, true, ePoolReturnType.ChangeScene, ePoolDestroyType.Return);
#else
        var pDamage = Single.ObjectPool.Get<SHDamageObject>(
                strPrefabName, true, ePoolReturnType.ChangeScene, ePoolDestroyType.ChangeScene);
#endif
        if (null == pDamage)
        {
            Debug.LogErrorFormat("SHDamage::AddDamage - Not Found Prefab : {0}", strPrefabName);
            return null;
        }
        
        return pDamage;
    }
    void ReturnDamage(SHDamageObject pDamage)
    {
        if (null == pDamage)
            return;

        Single.ObjectPool.Return(pDamage.gameObject);
    }
#endregion


#region Utility : Collision
    void CheckCollision(SHDamageObject pDamage)
    {
        if (null == pDamage)
            return;

        if (false == pDamage.IsCheckCrash())
            return;

        int iLayerMask      = pDamage.GetTargetLayerMask();
        if (0 == iLayerMask)
            return;

        var vCenter         = pDamage.GetDMGCollider().bounds.center;
        var vBeforeCenter   = pDamage.m_pBeforeBounds.center;
        var vExtents        = pDamage.GetDMGCollider().bounds.extents;
        var vDist           = Vector3.Distance(vCenter, vBeforeCenter);
        var vDirection      = (vBeforeCenter - vCenter).normalized;
        var pHits           = Physics.BoxCastAll(vCenter, vExtents, vDirection, Quaternion.identity, vDist, iLayerMask);
        
        if ((null == pHits) || (0 == pHits.Length))
            return;
        
        foreach(var pHit in pHits)
        {
            if (false == pDamage.IsTarget(pHit.transform.tag))
                continue;

            var pTarget = pHit.transform.GetComponent<SHMonoWrapper>();
            if (true == pTarget.IsOffDamage())
                return;

            pTarget.OnCrashDamage(pDamage);
            pDamage.OnCrashDamage(pTarget);
        }
    }
    #endregion


    #region Utility : Helpper
    List<SHMonoWrapper> GetTargets(SHDamageObject pDamage)
    {
        if (null == pDamage)
            return null;
        
        var pTargets = new List<SHMonoWrapper>();
        SHUtils.ForToDic(m_dicUnits, (pKey, pValue) =>
        {
            SHUtils.ForToList(pValue, (pUnit) =>
            {
                if (false == pDamage.IsTarget(pUnit.tag))
                    return;

                pTargets.Add(pUnit);
            });
        });
        
        return pTargets;
    }
    #endregion
}
