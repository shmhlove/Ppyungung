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
            var pSpace = Single.ObjectPool.Get<SHBGBlock>("BGSpace");
            pSpace.SetActive(true);
            pSpace.SetParent(SH3DRoot.GetRoot());
            m_pBlocks.Add(pSpace);
        });
        Repositioning(Vector3.zero);
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
        SHBGBlock       pCenter     = null;
        List<SHBGBlock> pRemainders = new List<SHBGBlock>();
        GetDecompositionBlocks(vCenter, ref pCenter, ref pRemainders);
        
        if (null == pCenter)
            return;

        if (3 != pRemainders.Count)
            return;

        var vCenterBlock  = pCenter.GetLocalPosition();
        var vCenterWidth  = pCenter.GetHalfWidth();
        var vCenterHeight = pCenter.GetHalfHeight();
        var vDirection    = (vCenter - vCenterBlock).normalized;
        var vSignX        = SHMath.Sign(vDirection.x);
        var vSignZ        = SHMath.Sign(vDirection.z);

        pRemainders[0].SetLocalPosition(
            new Vector3(vCenterBlock.x + (vCenterWidth * vSignX), 0.0f, vCenterBlock.z));

        pRemainders[1].SetLocalPosition(
            new Vector3(vCenterBlock.x, 0.0f, vCenterBlock.z + (vCenterHeight * vSignZ)));

        pRemainders[2].SetLocalPosition(new Vector3(
                vCenterBlock.x + (vCenterWidth * vSignX),
                0.0f,
                vCenterBlock.z + (vCenterHeight * vSignZ)));
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
