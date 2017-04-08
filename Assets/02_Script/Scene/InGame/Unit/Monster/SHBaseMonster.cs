using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHBaseMonster : SHState
{
    #region Members : Inspector
    [Header("[Monster State]")]
    [ReadOnlyField]  public int            m_iMonsterID        = 0;
    [ReadOnlyField]  public string         m_strCurrentState   = string.Empty;
    [Header("[Monster Info]")]
    [SerializeField] private SHMonoWrapper m_pShootPos         = null;
    [SerializeField] private float         m_fSpeedRatio       = 1.0f;
    [SerializeField] private float         m_fStartHealthPoint = 1.0f;
    #endregion


    #region Members : Monster Status Data
    private Vector3        m_vDirection     = Vector3.zero;
    private float          m_fHommingAngle  = 1.0f;
    #endregion


    #region Interface Functions
    public void InitMonster(int iID)
    {
        m_iMonsterID    = iID;
        m_fHommingAngle = SHMath.Random(1.0f, 2.0f);
        m_fHealthPoint  = m_fStartHealthPoint;

        ChangeState(0);
    }
    #endregion


    #region Virtual Functions
    public virtual string GetState() { return m_iCurrentStateID.ToString(); }
    public virtual int GetDieState() { return 0; }
    public virtual int GetKillState() { return 0; }
    #endregion


    #region Override Functions
    public override void FrameMove()
    {
        base.FrameMove();
        m_strCurrentState = GetState();
    }
    public override bool IsPassDMGCollision()
    {
        return IsState(GetDieState());
    }
    public override void OnCrashDamage(SHMonoWrapper pObject)
    {
        if (null == pObject)
            return;

        var pDamage = pObject as SHDamageObject;
        {
            AddHP(-pDamage.m_pInfo.m_fDamageValue);
            PlayParticle("Particle_Crash_Dust_Big");
        }

        if (false == IsRemainHP())
        {
            ChangeState(GetDieState());
        }
    }
    #endregion


    #region Utility Functions
    protected void SetMove()
    {
        if (Vector3.zero == m_vDirection)
        {
            m_vDirection = (Single.Player.GetLocalPosition() - GetLocalPosition()).normalized;
        }

        var fMoveSpeed = (SHHard.m_fMonMoveSpeed * m_fSpeedRatio);
        if (true == Single.Monster.m_bIsStopMonster)
        {
            fMoveSpeed = 0.0f;
        }

        var vPos = SHPhysics.GuidedMissile(GetLocalPosition(), ref m_vDirection, 
        Single.Player.GetLocalPosition(),
        m_fHommingAngle, fMoveSpeed);

        SetLocalLookZ(m_vDirection);
        SetLocalPosition(GetLimitInGround(vPos));
    }
    protected void SetAttack(string strDmgName, Vector3 vDirection)
    {
        var pDamage = Single.Damage.AddDamage(strDmgName, new SHDamageParam(m_pShootPos));
        {
            pDamage.SetDMGSpeed(SHHard.m_fMonDamageSpeed);
            pDamage.SetDMGDirection(vDirection);
        }
    }
    protected void SetExplosionDie()
    {
        var pParticle = PlayParticle("Particle_Crash_Dust_Big", SH3DRoot.GetRootToEffect());
        if (null != pParticle)
        {
            pParticle.transform.localPosition = GetLocalPosition();
            pParticle.transform.localScale    = GetLocalScale();
        }

        Single.Sound.PlayEffect("Audio_Effect_Explosion");

        SetActive(false);
        Single.Monster.DeleteMonster(this);
    }
    protected Vector3 GetLimitInGround(Vector3 vPosition)
    {
        return SHPhysics.IncludePointInRect(new Vector4(
            -SHHard.m_fMoveLimitX, -SHHard.m_fMoveLimitY,
             SHHard.m_fMoveLimitX,  SHHard.m_fMoveLimitY), vPosition);
    }
    protected void AddHP(float fAddValue)
    {
        m_fHealthPoint += fAddValue;
        m_fHealthPoint = Mathf.Clamp(m_fHealthPoint, 0.0f, m_fStartHealthPoint);
    }
    protected bool IsRemainHP()
    {
        return (0.0f < m_fHealthPoint);
    }
    #endregion
}
