using UnityEngine;
using System.Collections;

public enum eGLOW_TYPE
{
    RED = 0,
    BLUE,
    GREEN,
}

public class SHChar_Glow : MonoBehaviour
{


    public Color m_GlowColor = new Color(0,0,0,0);
    public float m_time = 0f;
    
    public eGLOW_TYPE eGlowType;
    public bool m_bState = false;


    float Delaytime;
    Color glowColor;
    Color oldGlowColor;
    float DecTime;
    float tmpTime;

    Renderer[] renderers;
    SkinnedMeshRenderer[] skinRenderer;
    //Shader shader = new Shader();
    

   
    void Awake()
    {
        
        renderers = transform.GetComponentsInChildren<Renderer>();
        skinRenderer = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        
    }
    void Start()
    {
        getGlow();
        resetGlow();
    }
    void getGlow()
    {
        foreach (Renderer tmpRenderer in renderers)
        {
			if(tmpRenderer.material.HasProperty("_GlowColor"))
            	oldGlowColor = tmpRenderer.material.GetColor("_GlowColor");
        }
        foreach (SkinnedMeshRenderer tmpskinRenderer in skinRenderer)
        {
			if(tmpskinRenderer.material.HasProperty("_GlowColor"))
            	oldGlowColor = tmpskinRenderer.material.GetColor("_GlowColor");
        }
    }

    
    public void resetGlow()
    {
        Delaytime = 0f;
        tmpTime = 0f;
        glowColor = new Color(0, 0, 0, 0);
        foreach (Renderer tmpRenderer in renderers)
            tmpRenderer.material.SetColor("_GlowColor", oldGlowColor);
        foreach (SkinnedMeshRenderer tmpskinRenderer in skinRenderer)
            tmpskinRenderer.material.SetColor("_GlowColor", oldGlowColor);

    }

    void GetColorType()
    {
        switch (eGlowType)
        {
            case eGLOW_TYPE.RED:
                {
                    m_GlowColor = new Color(0.157f, 0.091f, 0.034f, 1.0f);
                    m_time = 0.11f;
                    break;
                }
            case eGLOW_TYPE.BLUE:
                {
                    m_GlowColor = new Color(0f, 0f, 1f, 1.0f);
                    m_time = 0.11f;
                    break;
                }
            case eGLOW_TYPE.GREEN:
                {
                    m_GlowColor = new Color(0f, 1f, 0f, 1.0f);
                    m_time = 0.11f;
                    break;
                }
        }
    }

    void Update()
    {

        if (m_bState)
        {
            GetColorType();
            Delaytime += Time.deltaTime;
            if (Delaytime < m_time)
            {
                tmpTime = (Delaytime / m_time);
                DecTime = Delaytime;

            }
            else
            {
                DecTime -= Time.deltaTime;
                tmpTime = (DecTime / m_time);
                if (DecTime < 0)
                    Delaytime = 0;

            }
            glowColor = new Color(m_GlowColor.r * tmpTime, m_GlowColor.g * tmpTime, m_GlowColor.b * tmpTime);
            foreach (Renderer tmpRenderer in renderers)
                tmpRenderer.material.SetColor("_GlowColor", glowColor);
            foreach (SkinnedMeshRenderer tmpskinRenderer in skinRenderer)
                tmpskinRenderer.material.SetColor("_GlowColor", glowColor);
        }
        else
        {
            //Debug.Log(oldGlowColor);
            resetGlow();
            //Debug.Log(oldGlowColor);
        }


    }
}
