using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using DicTouch = System.Collections.Generic.Dictionary<int, UnityEngine.Vector2>;


public class SHNativeInputManager : SHSingleton<SHNativeInputManager>
{
    #region Member : TouchInfo
    [HideInInspector] public DicTouch m_dicTouchEnter       = new DicTouch();
    [HideInInspector] public DicTouch m_dicTouchEnd         = new DicTouch();
    [HideInInspector] public DicTouch m_dicCurrentTouchMove = new DicTouch();
    [HideInInspector] public DicTouch m_dicBeforeTouchMove  = new DicTouch();
    #endregion


    #region Member : TouchOrder
    [HideInInspector] public List<int> m_pTouchOrders = new List<int>();
    #endregion
    

    #region Member : Event
    [HideInInspector] public Action<int, Vector2> m_pEventToEnter = null;
    [HideInInspector] public Action<int, Vector2> m_pEventToDrag  = null;
    [HideInInspector] public Action<int, Vector2> m_pEventToEnd   = null;
    #endregion


    #region System Functions
    public override void OnInitialize() { }
    public override void OnFinalize() { }
    public override void Update()
    {
        base.Update();

#if UNITY_EDITOR
        // Mobile
        if (0 < Input.touchCount)
        {
            for (int iLoop = 0; iLoop < Input.touchCount; ++iLoop)
            {
                Touch pTouch = Input.GetTouch(iLoop);
                switch(pTouch.phase)
                {
                    case TouchPhase.Began:      SetTouchEnter(pTouch.fingerId, pTouch.position); break;
                    case TouchPhase.Ended: 
                    case TouchPhase.Canceled:   SetTouchEnd(pTouch.fingerId, pTouch.position);   break;
                    case TouchPhase.Moved:      SetTouchMove(pTouch.fingerId, pTouch.position);  break;
                }
            }
        }
        // PC
        else
        {
            switch(GetMousePhase())
            {
                case TouchPhase.Began:      SetTouchEnter(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y)); break;
                case TouchPhase.Ended: 
                case TouchPhase.Canceled:   SetTouchEnd(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y));   break;
                case TouchPhase.Moved:      SetTouchMove(0, new Vector2(Input.mousePosition.x, Input.mousePosition.y));  break;
            }
        }
#endif
    }
    #endregion


    #region Interface Functions
    public void Refash()
    {
        Update();
    }
    public int GetFirstFingerID()
    {
        if (0 == m_pTouchOrders.Count)
            return -1;

        return m_pTouchOrders[0];
    }
    public int GetLastFingerID()
    {
        if (0 == m_pTouchOrders.Count)
            return -1;

        return m_pTouchOrders[(m_pTouchOrders.Count - 1)];
    }
    public Vector2 GetBeforeDragPos(int iFingerID)
    {
        if (false == m_dicBeforeTouchMove.ContainsKey(iFingerID))
            return Vector2.zero;

        return m_dicBeforeTouchMove[iFingerID];
    }
    public Vector2 GetCurrentDragPos(int iFingerID)
    {
        if (false == m_dicCurrentTouchMove.ContainsKey(iFingerID))
            return Vector2.zero;

        return m_dicCurrentTouchMove[iFingerID];
    }
    public bool IsTouch()
    {
        return (0 != m_pTouchOrders.Count) || (0 != m_dicTouchEnter.Count);
    }
    #endregion


    #region Utility Functions
    TouchPhase GetMousePhase()
    {
        if (true == Input.GetButtonDown("Fire1")) return TouchPhase.Began;
        if (true == Input.GetButtonUp("Fire1"))   return TouchPhase.Canceled;
        if (true == Input.GetButton("Fire1"))     return TouchPhase.Moved;
        return TouchPhase.Stationary;
    }
    void SetTouchEnter(int iFingerID, Vector2 vTouchPos)
    {
        m_dicTouchEnter[iFingerID]       = vTouchPos;
        m_dicCurrentTouchMove[iFingerID] = vTouchPos;
        m_dicBeforeTouchMove[iFingerID]  = vTouchPos;
        m_dicTouchEnd.Remove(iFingerID);
        m_pTouchOrders.Add(iFingerID);
        
        if (null != m_pEventToEnter)
            m_pEventToEnter(iFingerID, vTouchPos);
    }
    void SetTouchEnd(int iFingerID, Vector2 vTouchPos)
    {
        m_dicTouchEnd[iFingerID]        = vTouchPos;
        m_dicTouchEnter.Remove(iFingerID);
        m_dicCurrentTouchMove.Remove(iFingerID);
        m_dicBeforeTouchMove.Remove(iFingerID);
        m_pTouchOrders.Remove(iFingerID);

        if (null != m_pEventToEnter)
            m_pEventToEnd(iFingerID, vTouchPos);
    }
    void SetTouchMove(int iFingerID, Vector2 vTouchPos)
    {
        var vCurrentPos = m_dicCurrentTouchMove[iFingerID];
        m_dicBeforeTouchMove[iFingerID]  = vCurrentPos;
        m_dicCurrentTouchMove[iFingerID] = vTouchPos;

        if (null != m_pEventToEnter)
            m_pEventToDrag(iFingerID, vTouchPos);
    }
    #endregion
}
