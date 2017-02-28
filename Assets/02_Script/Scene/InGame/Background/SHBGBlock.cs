using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHBGBlock : SHMonoWrapper
{
    #region Members : Inspector
    public float m_fWidth  = 25600;
    public float m_fHeight = 14400;
    #endregion


    #region Interface Functions
    public float GetHalfWidth()
    {
        return m_fWidth * 0.5f;
    }
    public float GetHalfHeight()
    {
        return m_fHeight * 0.5f;
    }
    public Bounds GetBounds()
    {
        return new Bounds(GetLocalPosition(), new Vector3(m_fWidth, 0.0f, m_fHeight));
    }
    #endregion
}
