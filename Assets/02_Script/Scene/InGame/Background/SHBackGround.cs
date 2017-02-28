using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHBackGround : SHInGame_Component
{
    #region Members
    private List<SHBGBlock> m_pBlocks      = new List<SHBGBlock>();
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        SHUtils.For(0, 4, (iIndex) =>
        {
            var pSpace = Single.ObjectPool.Get<SHBGBlock>("BGBlock");
            pSpace.SetActive(true);
            pSpace.SetParent(SH3DRoot.GetRoot());
            pSpace.m_iBlockID = iIndex;
            m_pBlocks.Add(pSpace);
        });
    }
    public override void OnFinalize()
    {
    }
    public override void OnFrameMove()
    {
        Repositioning(Single.Player.GetLocalPosition());
    }
    #endregion


    #region Utility Functions
    void Repositioning(Vector3 vCenter)
    {
        SHBGBlock       pCenterBlock = null;
        List<SHBGBlock> pRemainders = new List<SHBGBlock>();
        GetDecompositionBlocks(vCenter, ref pCenterBlock, ref pRemainders);
        
        if (null == pCenterBlock)
            return;
        
        var vCenterBlock  = pCenterBlock.GetLocalPosition();
        var vCenterWidth  = pCenterBlock.GetHalfWidth();
        var vCenterHeight = pCenterBlock.GetHalfHeight();
        var vDirection    = (vCenter - vCenterBlock).normalized;
        var fSignX        = SHMath.Sign(vDirection.x);
        var fSignZ        = SHMath.Sign(vDirection.z);

        pRemainders[0].SetLocalPosition(
            new Vector3(vCenterBlock.x + (vCenterWidth * fSignX), 0.0f, vCenterBlock.z));
        pRemainders[1].SetLocalPosition(
            new Vector3(vCenterBlock.x, 0.0f, vCenterBlock.z + (vCenterHeight * fSignZ)));
        pRemainders[2].SetLocalPosition(
            new Vector3(vCenterBlock.x + (vCenterWidth * fSignX), 0.0f, vCenterBlock.z + (vCenterHeight * fSignZ)));
    }
    void GetDecompositionBlocks(Vector3 vCenter, ref SHBGBlock pCenter, ref List<SHBGBlock> pRemainders)
    {
        foreach(var pObject in m_pBlocks)
        {
            if ((null == pCenter) &&
                (true == SHPhysics.IsInBoundToPoint(pObject.GetBounds(), vCenter)))
            {
                pCenter = pObject;
                continue;
            }

            pRemainders.Add(pObject);
        }
    }
    #endregion
}
