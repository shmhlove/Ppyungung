using UnityEngine;
using System.Collections;

public class SHRenderQueue : MonoBehaviour
{
    public ParticleSystem   m_ParticleSystem;
    public Material         m_Material;
    public int              m_iRenderQueue = 3000;

    Material mMat;

    void Start()
    {
        Renderer pRenderer = GetComponent<Renderer>();

        if (null == pRenderer)
        {
            ParticleSystem pParticle = null;

            if (null != m_ParticleSystem)
                pParticle = m_ParticleSystem;
            else
                pParticle = GetComponent<ParticleSystem>();

            if (pParticle != null)
                pRenderer = pParticle.GetComponent<Renderer>();
        }

        if (pRenderer != null)
        {
            if (null != m_Material)
                mMat = m_Material;
            else
                mMat = new Material(pRenderer.sharedMaterial);

            if (null == mMat)
                return;

            mMat.renderQueue = m_iRenderQueue;
            pRenderer.material = mMat;
        }
    }

    [FuncButton]
    public void RenderQueueReset()
    {
        Start();
    }
}
