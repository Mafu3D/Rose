using System.Collections;
using System.Collections.Generic;
using Project;
using TMPro;
using UnityEngine;

internal struct CalloutStruct
{
    internal string text;
    internal Color color;
    internal float duration;
    internal CalloutStruct(string text, Color color, float duration)
    {
        this.text = text;
        this.color = color;
        this.duration = duration;
    }
}

public class CalloutUI : Singleton<CalloutUI>
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameObject container;

    List<CalloutStruct> calloutQueue = new();

    Coroutine currentCalloutRoutine = null;

    public void QueueCallout(string text, Color color, float duration)
    {
        CalloutStruct nextCallout = new CalloutStruct(text, color, duration);
        calloutQueue.Add(nextCallout);
        Debug.Log("queueing callout");
    }
    public void QueueCallout(string text, float duration)
    {
        CalloutStruct nextCallout = new CalloutStruct(text, Color.red, duration);
        calloutQueue.Add(nextCallout);
        Debug.Log("queueing callout");
    }
    public void QueueCallout(string text, Color color)
    {
        CalloutStruct nextCallout = new CalloutStruct(text, color, 2f);
        calloutQueue.Add(nextCallout);
        Debug.Log("queueing callout");
    }
    public void QueueCallout(string text)
    {
        CalloutStruct nextCallout = new CalloutStruct(text, Color.red, 2f);
        calloutQueue.Add(nextCallout);
        Debug.Log("queueing callout");
    }

    void Start()
    {
        container.SetActive(false);
    }

    void Update()
    {
        if (calloutQueue.Count > 0 && currentCalloutRoutine == null)
        {
            currentCalloutRoutine = StartCoroutine(CalloutRoutine(calloutQueue[0]));
            calloutQueue.Remove(calloutQueue[0]);
        }
    }

    private IEnumerator CalloutRoutine(CalloutStruct calloutStruct)
    {
        container.gameObject.SetActive(true);
        text.text = calloutStruct.text;
        text.color = calloutStruct.color;
        yield return new WaitForSecondsRealtime(calloutStruct.duration);
        container.gameObject.SetActive(false);
        currentCalloutRoutine = null;
    }
}