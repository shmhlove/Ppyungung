using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class SHDamageObject : SHMonoWrapper
{
    // public bool     m_bIsDeleteWithDamage = true;               // 데미지와 함께 죽을것인가?

    GameObject CreateEffect(string strPrefabName)
    {
        var pEffect = Single.ObjectPool.Get(strPrefabName);

        if (null == pEffect)
        {
            Debug.LogErrorFormat("SHDamageObject::CreateEffect - Not Found Prefab : {0}", strPrefabName);
            return null;
        }

        SHGameObject.SetParent(pEffect, Single.InGame.GetGameObject());

        return pEffect;
    }

    void SetupEffectTransform(SHDamageEffectInfo pEffectInfo, GameObject pEffect)
    {
        if ((null == pEffect) || (null == pEffectInfo))
            return;
        
        Vector3 vPosition = pEffectInfo.m_vStaticStartPosition;
        
        if (true == pEffectInfo.m_bIsTraceDamage)
        {
            SHGameObject.SetParent(pEffect, GetGameObject());
            vPosition = Vector3.zero;
        }
        else if (true == pEffectInfo.m_bIsStartPosToDamage)
        {
            vPosition = GetPosition();
        }

        pEffect.transform.localPosition = (vPosition + pEffectInfo.m_vPositionOffset);
    }
}