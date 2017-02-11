using UnityEngine;
using System.Collections;

public class SHBlobShadow : MonoBehaviour 
{
    public Transform m_trRefDummy;
    private Quaternion m_qOriginalRotation = Quaternion.identity;

	void Start()
    {
        //m_qOriginalRotation = transform.rotation;
        // 무조건 아래를 보도록..
        m_qOriginalRotation = Quaternion.Euler(90f, 0f, 0f);
	}
	
	void Update() 
    {
	    if(null != m_trRefDummy)
        {
            transform.position = m_trRefDummy.position;
            transform.rotation = m_qOriginalRotation;
        }
	}
}
