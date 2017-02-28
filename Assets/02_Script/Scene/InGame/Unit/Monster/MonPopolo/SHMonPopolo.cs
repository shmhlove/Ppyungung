using UnityEngine;
using System;
using System.Collections;

/* partial Information
 * 
 * SHMonPopolo
 * SHMonPopolo_State
 * SHMonPopolo_UI
 * SHMonPopolo_Utility
 * 
*/

public partial class SHMonPopolo : SHState
{
    #region Members : Inspector
    [Header("Player Info")]
    [SerializeField] private SHMonoWrapper m_pShootPos     = null;
    [SerializeField] private float         m_fMoveSpeed    = 50.0f;
    #endregion


    #region Members : User Interaction Data
    private Vector3 m_vMoveDirection = Vector3.zero;
    private Vector3 m_vLookDirection = Vector3.zero;
    private Vector3 m_vDashDirection = Vector3.zero;
    #endregion


    #region System Functions
    public override void Start()
    {
        base.Start();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void FixedUpdate()
    {
        base.FrameMove();
    }
    #endregion


    #region Interface Functions
    #endregion
}