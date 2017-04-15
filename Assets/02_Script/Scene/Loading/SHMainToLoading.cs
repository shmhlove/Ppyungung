using UnityEngine;
using System.Collections;

public class SHMainToLoading : MonoBehaviour
{
    void Start()
    {
        Single.AppInfo.CreateSingleton();
    }
}
