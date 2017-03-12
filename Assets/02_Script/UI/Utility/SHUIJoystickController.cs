using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHUIJoystickController : SHMonoWrapper
{
    #region Members : Inspector
    [SerializeField] private SHUIJoystick m_pJoyStick = null;
    #endregion


    #region Event Handler
    void OnPress(bool bPressed)
    {
        if (null == m_pJoyStick)
            return;

        m_pJoyStick.OnPress(bPressed);
    }
    void OnDrag(Vector2 vDelta)
    {
        if (null == m_pJoyStick)
            return;

        m_pJoyStick.OnDrag(vDelta);
    }
    #endregion
}
