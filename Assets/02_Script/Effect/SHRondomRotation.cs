using UnityEngine;
using System.Collections;

public class SHRondomRotation : MonoBehaviour 
{
    public Vector2 ScaleXYZ;
    public Vector2 RotX;
    public Vector2 RotY;
    public Vector2 RotZ;

    private Vector3 rotate;
    private float fScale;
	// Use this for initialization
	void Start () {
        fScale = Random.Range(ScaleXYZ.x, ScaleXYZ.y);
        rotate.x = Random.Range(RotX.x, RotX.y);
        rotate.y = Random.Range(RotY.x, RotY.y);
        rotate.z = Random.Range(RotZ.x, RotZ.y);
        transform.Rotate(rotate);
        if (fScale != 0)
            transform.localScale = new Vector3(fScale, fScale, fScale);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
