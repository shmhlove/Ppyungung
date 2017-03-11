using UnityEngine;

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using System.Xml;

public class SHXML
{
    #region Members
    public string m_strXMLData = string.Empty;
    #endregion


    #region System Functions
    public SHXML() { }
    public SHXML(string strFileName)
    {
        if (true == string.IsNullOrEmpty(strFileName))
            return;

        strFileName = Path.GetFileNameWithoutExtension(strFileName);

        // �����ͻ� ������ �ȵ� ���� ( Convert���� ���� )���� ȣ���� �Ǿ��� ���
        // SHDataManager�� �����Ǹ� ���̾��Ű�� �̱��� ��Ⱑ ���� ������ ���̷�Ʈ�� �ε��� �� �ֵ��� �Ѵ�.
        TextAsset pTextAsset = null;
        if (false == SHDataManager.IsExists)
        {
            string strResources = "Resources";
            string strFilePath  = string.Format("{0}/{1}", SHPath.GetPathToXML(), strFileName);
            strFilePath         = strFilePath.Substring(strFilePath.IndexOf(strResources) + strResources.Length + 1).Replace('\\', '/');

            pTextAsset = Resources.Load<TextAsset>(strFilePath);
        }
        else
            pTextAsset = Single.Resource.GetTextAsset(strFileName);

        if (null != pTextAsset)
            SetXMLData(pTextAsset.text);
    }
    #endregion


    #region Interface Functions
    // �������̽� : XML�����Ͱ� �ε�Ǿ����� üũ
    public bool CheckXML()
    {
        return (false == string.IsNullOrEmpty(m_strXMLData));
    }

    // �������̽� : XML������ ����
    public void SetXMLData(string strBuff)
    {
        var pStream = new MemoryStream(Encoding.UTF8.GetBytes(strBuff));
        var pReader = new StreamReader(pStream, true);
        m_strXMLData = pReader.ReadToEnd();
        pReader.Close();
        pStream.Close();
    }

    // �������̽� : XML������ ���
    public string GetXML()
    {
        return m_strXMLData;
    }

    // �������̽� : XML�����͸� �Ľ̰����� Node���·� ���
    public XmlDocument GetDocument()
    {
        var strData = GetXML();
        if (true == string.IsNullOrEmpty(strData))
            return null;

        var pDocument = new XmlDocument();
        pDocument.LoadXml(strData);
        return pDocument;
    }

    // �������̽� : XmlDocument���� Node ���
    public XmlNode GetNodeToTag(XmlDocument pDoc, string strTag, int iItemIndex)
    {
        if (null == pDoc)
            pDoc = GetDocument();

        if (null == pDoc)
            return null;

        return pDoc.GetElementsByTagName(strTag).Item(iItemIndex);
    }

    // �������̽� : XmlNode���� �ڽ� Node ����Ʈ ���
    public XmlNodeList GetNodeList(string strTagName)
    {
        return GetNodeList(GetNodeToTag(GetDocument(), strTagName, 0));
    }
    public XmlNodeList GetNodeList(XmlNode pNode)
    {
        if (null == pNode)
            return null;

        return pNode.ChildNodes;
    }
    #endregion
}