using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
#endif

// 인스펙터에 데미지가 가질 수 있는 타켓의 태그를 콤보박스 형태로 노출합니다.
[CustomPropertyDrawer(typeof(DamageTags))]
public class SHDamageAttributeDrawers : PropertyDrawer
{
#if UNITY_EDITOR
    int m_iIndex = 0;
    SHDamageObject m_pDamageObject = null;

    public override void OnGUI(Rect pRect, SerializedProperty pProperty, GUIContent pLabel)
    {
        if (SerializedPropertyType.String != pProperty.propertyType)
        {
            EditorGUI.LabelField(pRect, "ERROR:", "May only apply to type string");
            return;
        }

        if (false == pProperty.displayName.Contains("Element"))
        {
            EditorGUI.LabelField(pRect, "ERROR:", "May only apply to type List<string>");
            return;
        }

        m_iIndex = int.Parse(pProperty.displayName.Split(' ')[1]);

        if (pProperty.serializedObject.targetObject is SHDamageObject)
        {
            Component pComponent = pProperty.serializedObject.targetObject as Component;
            m_pDamageObject = pComponent.GetComponent<SHDamageObject>();
        }

        if (null == m_pDamageObject)
        {
            EditorGUI.LabelField(pRect, "ERROR:", "Must have reference to a SHDamage_Unit.SHTarget");
            return;
        }

        string strButtonName = pProperty.stringValue;
        if (true == string.IsNullOrEmpty(strButtonName))
            strButtonName = "Select To Tag";

        pRect = EditorGUI.PrefixLabel(pRect, pLabel);
        if (GUI.Button(pRect, strButtonName, EditorStyles.popup))
        {
            Selector(pProperty);
        }
    }

    void Selector(SerializedProperty property)
    {
        var pMenu = new GenericMenu();
        var pTags = UnityEditorInternal.InternalEditorUtility.tags;
        foreach (string strMenu in pTags)
        {
            pMenu.AddItem(new GUIContent(strMenu), strMenu == property.stringValue, HandleSelect, strMenu);
        }

        pMenu.ShowAsContext();
    }

    void HandleSelect(object val)
    {
        if (null == m_pDamageObject)
            return;

        m_pDamageObject.AddTarget(m_iIndex, (string)val);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18;
    }
#endif
}