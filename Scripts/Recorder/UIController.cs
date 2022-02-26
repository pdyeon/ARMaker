using NatSuite.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIController : MonoBehaviour
{
    //private AndroidUtils androidUtils;
    [SerializeField] private List<GameObject> uselessBtns = new List<GameObject>();
    [SerializeField] private GameObject stopRecordBtn;
    ReplayCam _replayCam;

    private void Start()
    {
        if (ReplayCam.Instance != null) _replayCam = ReplayCam.Instance;
    }


    public void OnClickStartRecord()
    {
        foreach (var less in uselessBtns)
        {
            less.SetActive(false);
        }
        stopRecordBtn.SetActive(true);
        ReplayCam.Instance.StartRecording();
        StartCoroutine(DelayCallRecord());
    }
    private IEnumerator DelayCallRecord()
    {
        yield return new WaitForSeconds(0.1f);
        ReplayCam.Instance.StartRecording();
    }
    public void OnClickStopRecord()
    {
        foreach (var less in uselessBtns)
        {
            less.SetActive(true);
        }
        stopRecordBtn.SetActive(false);
        ReplayCam.Instance.StopRecording();
        StartCoroutine(DelaySaveVideo());
    }

    private IEnumerator DelaySaveVideo()
    {
        yield return new WaitForSeconds(1);
    }
    public void OnClickOpenGallery()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaClass.GetStatic<AndroidJavaObject>("currentActivity").Call("openGallery");
        }
#endif
    }
}
