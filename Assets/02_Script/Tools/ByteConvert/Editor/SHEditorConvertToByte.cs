#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SHConvertToByte))]
public class SHEditorConvertToByte : Editor
{
    #region Interface Functions
    [MenuItem("SHTools/Convert Table To Byte", false, 1)]
    [MenuItem("Assets/SHTools/Convert Table To Byte", false, 1)]
    static void SelectToMenu()
    {
        EditorUtility.DisplayDialog("[SHTools] Convert Table To ByteFile",
            "Bytes파일을 변환/생성 합니다!!", "확인");

        DateTime pStartTime = DateTime.Now;

        Run(false);

        GameObject.DestroyImmediate(GameObject.Find("SHSingletons(Destroy)"));
        
        EditorUtility.DisplayDialog("[SHTools] Convert Table To ByteFile",
            string.Format("Bytes파일이 변환/생성 되었습니다.!!\n시간 : {0:F2}sec", 
            ((DateTime.Now - pStartTime).TotalMilliseconds / 1000.0f)), "확인");
    }
    public static void Run(bool bIsDelete)
    {
        var pTable = new SHConvertToByte();
        pTable.RunEditorToConvert(bIsDelete);
    }
    #endregion
}
#endif