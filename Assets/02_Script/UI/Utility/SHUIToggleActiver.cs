using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHUIToggleActiver : SHMonoWrapper
{
    #region Members : Inspector
    public List<GameObject> m_pOnToGroup       = new List<GameObject>(); // Toggle Value가 true  일때 켤 오브젝트
    public List<GameObject> m_pOffToGroup      = new List<GameObject>(); // Toggle Value가 false 일때 켤 오브젝트
    #endregion


    #region Utility Functions
    void SetActiveToOnObjects(bool bIsActive)
    {
        SHUtils.ForToList(m_pOnToGroup, (pItem) => pItem.SetActive(bIsActive));
    }
    void SetActiveToOffObjects(bool bIsActive)
    {
        SHUtils.ForToList(m_pOffToGroup, (pItem) => pItem.SetActive(bIsActive));
    }
    #endregion


    #region Event Functions
    public void OnEventToToggle(bool bIsToggle)
    {
        SetActiveToOnObjects(true == bIsToggle);
        SetActiveToOffObjects(false == bIsToggle);
    }
    #endregion
}
