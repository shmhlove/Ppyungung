using UnityEngine;
using System.Collections;

// [AddComponentMenu("SHTechnical/Effect/Billboard")]
public class SHBillboard : MonoBehaviour
{
    public enum eBillboardType
    {
        None = 0, 
        ALL, 
        X, 
        Y, 
        Z, 
        LocalY, 
        Bar
    };
    private Vector3 m_v3Rotate;
    public eBillboardType m_Axis = eBillboardType.ALL;
    public bool m_Mirror = false;


    public Vector2 m_RandomRotateX;
    public Vector2 m_RandomRotateY;
    public Vector2 m_RandomRotateZ = new Vector2(180f, -180f);

    private bool m_bBarState = false;
    private Vector3 m_v3YRotate;
    private Transform m_trMainCamera;

    void Start()
    {
        m_v3Rotate.x = Random.Range(m_RandomRotateX.x, m_RandomRotateX.y);
        m_v3Rotate.y = Random.Range(m_RandomRotateY.x, m_RandomRotateY.y);
        m_v3Rotate.z = Random.Range(m_RandomRotateZ.x, m_RandomRotateZ.y);

        if (m_Mirror)
            transform.localScale = transform.localScale * -1f;

        m_trMainCamera = Camera.main.transform;
    }
    void Update()
    {
        if (null == m_trMainCamera)
            return;

        switch (m_Axis)
        {
            case eBillboardType.ALL:
                {
                    transform.LookAt(m_trMainCamera);
                    transform.Rotate(m_v3Rotate);
                }
                break;

            case eBillboardType.X:
                {
                    Vector3 cam_pos = m_trMainCamera.position - transform.position;
                    cam_pos.x = 0.0f;
                    cam_pos.Normalize();
                    transform.forward = cam_pos;
                    transform.Rotate(m_v3Rotate);
                }
                break;

            case eBillboardType.Y:
                {
                    Vector3 cam_pos = m_trMainCamera.position - transform.position;
                    cam_pos.y = 0.0f;
                    transform.forward = cam_pos;
                    transform.Rotate(m_v3Rotate);
                }
                break;

            case eBillboardType.Z:
                {
                    Vector3 cam_pos = m_trMainCamera.position - transform.position;
                    cam_pos.z = 0.0f;
                    transform.forward = cam_pos;
                    transform.Rotate(m_v3Rotate);
                }
                break;

            case eBillboardType.LocalY:
                {
                    Vector3 cam_pos = m_trMainCamera.position - transform.position;
                    cam_pos = transform.InverseTransformDirection(cam_pos);
                    cam_pos.Normalize();

                    Vector2 objToCamProj = new Vector2(cam_pos.x, cam_pos.z);
                    float angleToRot = Vector2.Angle(Vector2.right, objToCamProj);

                    if (objToCamProj.y > 0f)
                    {
                        angleToRot *= -1f;
                    }
                    m_v3YRotate = (new Vector3(0f, angleToRot + 90.0f + m_v3Rotate.y, 0f)) ;
                    transform.Rotate(m_v3YRotate);

                }
                break;

            case eBillboardType.Bar:
                {
                    if (!m_bBarState)
                    {
                        m_bBarState = true;
                        transform.localScale = transform.localScale * -1f;
                    }
                    transform.eulerAngles = m_v3Rotate + m_trMainCamera.rotation.eulerAngles;
                }
                break;
        }
    }
}


