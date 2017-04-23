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
                strPrefabName, true, ePoolReturnType.Disable, ePoolDestroyType.Return);
#else
        var pDamage = Single.ObjectPool.Get<SHDamageObject>(
                strPrefabName, true, ePoolReturnType.Disable, ePoolDestroyType.ChangeScene);
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
        if (true == m_bIsLockCheckCollision)
            return;

        if (null == pDamage)
            return;

        if (false == pDamage.IsCheckCrash())
            return;

        int iLayerMask = pDamage.GetTargetLayerMask();
        if (0 == iLayerMask)
            return;

        var vPosition       = pDamage.GetPosition();
        var vBeforePosition = pDamage.GetBeforePosition();
        var vExtents        = pDamage.GetDMGCollider().bounds.extents;
        var fDistance       = Vector3.Distance(vPosition, vBeforePosition);
        var vDirection      = (vBeforePosition - vPosition).normalized;
        vDirection          = (Vector3.zero == vDirection) ? Vector3.forward : vDirection;

        var pHits           = Physics.BoxCastAll(vPosition, vExtents, vDirection, Quaternion.identity, fDistance, iLayerMask);
        if ((null == pHits) || (0 == pHits.Length))
            return;
        
        foreach(var pHit in pHits)
        {
            if (false == pDamage.IsTarget(pHit.transform.tag))
                continue;

            if (true == pDamage.IsIgnoreTarget(pHit.transform.name))
                continue;

            var pTarget = pHit.transform.GetComponent<SHMonoWrapper>();
            if (null == pTarget)
                return;

            if (0 < pTarget.GetDMGCrashHitTick(pDamage.m_pInfo.m_strID))
                return;

            if (true == pTarget.IsPassDMGCollision())
                return;
            
            pTarget.OnCrashDamage(pDamage);
            pDamage.OnCrashDamage(pTarget);

            pTarget.SetDMGCrashHitTick(
                pDamage.m_pInfo.m_strID, 
                pDamage.m_pInfo.m_iCheckDelayTickToCrash);

            // 데미지HP 0이될때 Destroy처리
            if (0 < pDamage.m_pInfo.m_iDamageHP)
            {
                if (0 == --pDamage.m_pInfo.m_iDamageHP)
                {
                    DelDamage(pDamage);
                    break;
                }
            }
        }
    }
    #endregion
}
