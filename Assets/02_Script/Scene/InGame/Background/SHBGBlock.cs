using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHBGBlock : SHMonoWrapper
{
    #region Members : Inspector
    public float m_fWidth  = 12800;
    public float m_fHeight = 7200;
    #endregion


    #region Members : Inspector
    [ReadOnlyField] public int m_iBlockID = 0;
    #endregion


    #region Interface Functions
    public void Initialize(int iBlockID)
    {
        m_iBlockID = iBlockID;
    }
    public Bounds GetBounds()
    {
        return new Bounds(GetLocalPosition(), new Vector3((m_fWidth*2.0f), (m_fHeight*2.0f), 0.0f));
    }
    #endregion
}
