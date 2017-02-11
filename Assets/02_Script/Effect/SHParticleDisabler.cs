using UnityEngine;
using System.Collections;

public class SHParticleDisabler : MonoBehaviour
{
    public ParticleSystem m_pParticle = null;

    public void Update()
    {
        if (null == m_pParticle)
            return;

        if (false == m_pParticle.isPlaying)
            gameObject.SetActive(false);
    }
}
