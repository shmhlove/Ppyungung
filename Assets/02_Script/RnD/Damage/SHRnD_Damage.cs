using UnityEngine;
using System.Collections;

public class SHRnD_Damage : SHMonoWrapper
{
    #region Members
    public GameObject m_pTarget = null;
    #endregion


    #region System Functions
    public override void Start()
    {
        Single.Engine.CreateSingleton();
    }
    public override void FixedUpdate()
    {
        Single.Engine.FrameMove();
    }
    #endregion


    #region Interface Functions
    [FuncButton] void AddDamage()
    {
        Single.Damage.AddDamage("Dmg_Sample", 
            new SHAddDamageParam(this, m_pTarget, OnEventToDelete, OnEventToCollision));
    }
    #endregion


    #region Utility Functions
    #endregion


    #region Event Handler
    public void OnEventToDelete(SHDamageObject pDamage)
    {
        Debug.LogFormat("SHRnD_Damage::OnEventToDelete() - Event On Delete : {0}", pDamage.m_pInfo.m_strID);
    }
    public void OnEventToCollision(SHDamageObject pDamage)
    {
        Debug.LogFormat("SHRnD_Damage::OnEventToCollision() - Event On Collision : {0}", pDamage.m_pInfo.m_strID);
    }
    #endregion
}
