using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamage : SHBaseEngine
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
        var pDamage = Single.ObjectPool.Get<SHDamageObject>(
                strPrefabName, ePoolReturnType.ChangeScene, ePoolDestroyType.ChangeScene);

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
        
        SHUtils.ForToList(GetTargets(pDamage), (pTarget) => 
        {
            var pDamageCollider = pDamage.GetCollider();
            var pTargetCollider = pTarget.GetCollider();
            if ((null == pDamageCollider) || (null == pTargetCollider))
                return;

            var bIsCollistion = false;
            var bBounds       = pDamage.m_pBeforeBounds;
            for(float fRatio = 0.0f; fRatio <= 1.0f; fRatio += 0.1f)
            {
                bBounds.center = SHMath.Lerp(pDamage.m_pBeforeBounds.center, pDamageCollider.bounds.center, fRatio);
                if (true == bBounds.Intersects(pTargetCollider.bounds))
                {
                    bIsCollistion = true;
                    break;
                }
            }

            if (false == bIsCollistion)
                return;

            pTarget.OnCrashDamage(pDamage);
            pDamage.OnCrashDamage(pTarget);
        });
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
