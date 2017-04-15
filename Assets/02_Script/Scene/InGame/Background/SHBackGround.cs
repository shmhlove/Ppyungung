using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHBackGround : SHInGame_Component
{
    #region Members
    private List<SHBGBlock> m_pBlocks = new List<SHBGBlock>();
    #endregion


    #region Virtual Functions
    public override void OnInitialize()
    {
        SHUtils.For(0, 4, (iIndex) =>
        {
            var pSpace = Single.ObjectPool.Get<SHBGBlock>("BGBlock");
            pSpace.SetActive(true);
            pSpace.SetParent(Single.Root3D.GetRootBG());
            pSpace.Initialize(iIndex);
            pSpace.SetName(string.Format("{0}_{1}", pSpace.GetName(), pSpace.m_iBlockID));
            m_pBlocks.Add(pSpace);
        });
    }
    public override void OnFinalize()
    {
    }
    public override void OnFrameMove()
    {
        Repositioning(Single.Root3D.GetMainCamera().GetLocalPosition());
    }
    #endregion


    #region Utility Functions
    void Repositioning(Vector3 vCenter)
    {
        vCenter.z = 0.0f;

        SHBGBlock       pCenterBlock = null;
        List<SHBGBlock> pRemainders = new List<SHBGBlock>();
        GetDecompositionBlocks(vCenter, ref pCenterBlock, ref pRemainders);
        
        if (null == pCenterBlock)
            return;
        
        var vCenterBlock  = pCenterBlock.GetLocalPosition();
        var vCenterWidth  = pCenterBlock.m_fWidth;
        var vCenterHeight = pCenterBlock.m_fHeight;
        var vDirection    = (vCenter - vCenterBlock).normalized;
        var fSignX        = SHMath.Sign(vDirection.x);
        var fSignY        = SHMath.Sign(vDirection.y);

        pRemainders[0].SetLocalPosition(
            new Vector3(vCenterBlock.x + (vCenterWidth * fSignX), vCenterBlock.y, 0.0f));
        pRemainders[1].SetLocalPosition(
            new Vector3(vCenterBlock.x, vCenterBlock.y + (vCenterHeight * fSignY), 0.0f));
        pRemainders[2].SetLocalPosition(
            new Vector3(vCenterBlock.x + (vCenterWidth * fSignX), vCenterBlock.y + (vCenterHeight * fSignY), 0.0f));
    }
    void GetDecompositionBlocks(Vector3 vCenter, ref SHBGBlock pCenter, ref List<SHBGBlock> pRemainders)
    {
        foreach(var pBlock in m_pBlocks)
        {
            if ((null == pCenter) &&
                (true == SHPhysics.IsInBoundToPoint(pBlock.GetBounds(), vCenter)))
            {
                pCenter = pBlock;
                continue;
            }

            pRemainders.Add(pBlock);
        }
    }
    #endregion
}
