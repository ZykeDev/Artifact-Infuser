using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    private static Delay Instance;
    private List<Coroutine> activeCoroutines;
    private float defaultTime = 1f;


    void Awake()
    {
        Instance = this;
        activeCoroutines = new List<Coroutine>();
    }


    public static void DelayAction(Action action) => DelayAction(action, Instance.defaultTime);
    public static void DelayAction(Action action, float time) => Instance?.DelayThisAction(action, time);




    private void DelayThisAction(Action action, float time)
    {
        CoroutineBox box = new CoroutineBox();

        IEnumerator thisCoroutine = DelayCoroutine(time, action, box);
        box.coroutine = StartCoroutine(thisCoroutine);
    }


    private IEnumerator DelayCoroutine(float time, Action callback, CoroutineBox box)
    {
        yield return new WaitForEndOfFrame();

        if (time == 0) time = 0.1f;

        Coroutine self = box.coroutine;
        activeCoroutines.Add(self);

        yield return new WaitForSeconds(time);

        activeCoroutines.Remove(self);
        callback?.Invoke();
    }

    private class CoroutineBox
    {
        public Coroutine coroutine;
    }




}
