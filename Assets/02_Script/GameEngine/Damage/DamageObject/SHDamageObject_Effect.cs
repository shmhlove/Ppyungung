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

        SHGameObject.SetParent(pEffect, Single.Engine.GetGameObject());

        return pEffect;
    }

    void SetupEffectTransform(GameObject pEffect, SHDamageEffectInfo pEffectInfo)
    {
        if ((null == pEffect) || (null == pEffectInfo))
            return;
        
        Vector3 vLocalPosition = pEffectInfo.m_vStaticStartPosition;
        
        if (true == pEffectInfo.m_bIsTraceDamage)
        {
            SHGameObject.SetParent(pEffect, GetGameObject());
            vLocalPosition = Vector3.zero;
        }
        else if (true == pEffectInfo.m_bIsStartPosToDamage)
        {
            vLocalPosition = GetPosition();
        }

        pEffect.transform.localPosition = (vLocalPosition + pEffectInfo.m_vPositionOffset);
    }
}