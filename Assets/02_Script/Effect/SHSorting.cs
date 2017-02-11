using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct MatInfo
{
    public Material mat;
    public int nQNum;
    public Transform tran;
}



public class SHSorting : MonoBehaviour
{
    GameObject Cam;
    MeshRenderer meshrender;
    List<MatInfo> ListGobjs;

    // Use this for initialization
    void Start()
    {

        Transform[]  trans = transform.GetComponentsInChildren<Transform>();
        Cam = Camera.main.transform.gameObject;
        ListGobjs = new List<MatInfo>();
        foreach (Transform tran in trans)
        {
            meshrender = tran.gameObject.GetComponent<MeshRenderer>();
            if (meshrender != null)
            {
                MatInfo Mat = new MatInfo();
                //////////
                //Material mat = meshrender.sharedMaterial;
                //Material instMaterial = Instantiate(mat) as Material;
                //meshrender.sharedMaterial = instMaterial;
                //////////////////////
                Mat.nQNum = meshrender.sharedMaterial.renderQueue - 3000;
                meshrender.sharedMaterial = Instantiate(meshrender.sharedMaterial) as Material;
                Mat.mat = meshrender.sharedMaterial;

                Mat.tran = transform;

                ListGobjs.Add(Mat);

            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ListGobjs.Count; i++)
        {
            //Debug.Log(ListGobjs[i].mat.name + "          " + ListGobjs[i].nQNum);

            //Debug.Log(meshrender.sharedMaterial);
            Vector3 CamPos = Cam.transform.position;
            float dist = Vector3.Distance(CamPos, ListGobjs[i].tran.position);
            int dist_Q = (int)(dist / 0.5);

            if (dist_Q >= 89)
                dist_Q = 89;

            dist_Q = 3900 - (dist_Q * 10);
            ListGobjs[i].mat.renderQueue = dist_Q + ListGobjs[i].nQNum;
            //Debug.Log(dist_Q + "         " + mat.renderQueue + "        " + ListGobjs[i].tran.name);



        }

    }
}
