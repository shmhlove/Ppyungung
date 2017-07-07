using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHCoroutine : SHSingleton<SHCoroutine>
{
    #region Virtual Functions
    public override void OnInitialize()
    {
        SetDontDestroy();
        Single.Scene.AddEventToChangeScene(OnEventToChangeScene);
    }
    public override void OnFinalize() { }
    public override void Awake() { }
    public override void Start() { }
    public override void OnDisable() { }
    public override void OnDestroy() { }
    #endregion


    #region Event Handler
    public void OnEventToChangeScene(eSceneType eCurrentScene, eSceneType eNextScene)
    {
        StopAllCoroutines();
    }
    #endregion


    #region Interface Functions
    // yield return null : 다음 Update까지 대기
    //-----------------------------------------------
    public void NextUpdate(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextUpdate(pAction));
    }
    IEnumerator InvokeToNextUpdate(Action pAction)
    {
        yield return null;
        pAction.Invoke();
    }


    // yield return new WaitForFixedUpdate() : 다음 FixedUpdate까지 대기
    //-----------------------------------------------
    public void NextFixed(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextFixed(pAction));
    }
    IEnumerator InvokeToNextFixed(Action pAction)
    {
        yield return new WaitForFixedUpdate();
        pAction.Invoke();
    }
    

    // yield return new WaitForEndOfFrame() : 렌더링 작업이 끝날 때 까지 대기
    //-----------------------------------------------
    public void NextFrame(Action pAction)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToNextFrame(pAction));
    }
    IEnumerator InvokeToNextFrame(Action pAction)
    {
        yield return new WaitForEndOfFrame();
        pAction.Invoke();
    }
    

    // yield return new WaitForSeconds : 지정한 시간까지 대기
    //-----------------------------------------------
    public void WaitTime(Action pAction, float fDelay)
    {
        if (null == pAction)
            return;

        StartCoroutine(InvokeToWaitTime(pAction, fDelay));
    }
    IEnumerator InvokeToWaitTime(Action pAction, float fDelay)
    {
        if (0.0f >= fDelay)
            yield return null;
        else
            yield return new WaitForSeconds(fDelay);

        pAction.Invoke();
    }
    

    //yield return new WWW(string) : 웹 통신 작업이 끝날 때까지 대기
    //-----------------------------------------------
    public WWW WWW(Action<WWW> pAction, WWW pWWW)
    {
        StartCoroutine(InvokeToWWW(pAction, pWWW));
        return pWWW;
    }
    IEnumerator InvokeToWWW(Action<WWW> pAction, WWW pWWW)
    {
        yield return pWWW;

        if (null != pAction)
            pAction.Invoke(pWWW);
    }
    public WWW WWWOfSync(WWW pWWW)
    {
        InvokeToWWW(null, pWWW);
        while (false == pWWW.isDone);
        return pWWW;
    }
    

    //yield return new AsyncOperation : 비동기 작업이 끝날 때 까지 대기 (씬로딩)
    //-----------------------------------------------
    public AsyncOperation Async(Action pAction, AsyncOperation pAsync)
    {
        StartCoroutine(InvokeToAsync(pAction, pAsync));
        return pAsync;
    }
    IEnumerator InvokeToAsync(Action pAction, AsyncOperation pAsync)
    {
        while((null != pAsync) && (false == pAsync.isDone))
            yield return null;

        if (null != pAction)
            pAction.Invoke();
    }


    // 코루틴 실행
    //-----------------------------------------------
    List<IEnumerator> m_pRoutines = new List<IEnumerator>();
    public void StartRoutine(IEnumerator pRoutine)
    {
        if (true == IsRunRoutine(pRoutine))
            return;

        StartCoroutine(pRoutine);
        m_pRoutines.Add(pRoutine);
    }
    public void StopRoutine(IEnumerator pRoutine)
    {
        if (false == IsRunRoutine(pRoutine))
            return;
        
        StopCoroutine(pRoutine);
        m_pRoutines.Remove(pRoutine);
    }
    public bool IsRunRoutine(IEnumerator pRoutine)
    {
        return m_pRoutines.Contains(pRoutine);
    }
    #endregion
}