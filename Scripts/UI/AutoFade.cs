using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class AutoFade : MonoBehaviour
{
    public GameObject[] TargetObj;
    public GameObject OffObj;
    CanvasGroup _cg;


    private void Start()
    {
        _cg = GetComponent<CanvasGroup>();
    }


    private void OnEnable()
    {
        if (_cg != null)
        {
            _cg.alpha = 0f;
            _cg.DOFade(1f, 1f);
        }
    }


    public void FadeOut()
    {
        if (_cg)
        {
            _cg.alpha = 1f;
            _cg.DOFade(0f, 1f).OnComplete(() =>
            {
                foreach (var target in TargetObj)
                {
                    target.SetActive(true);
                }
                OffObj.SetActive(false);
            });
        }
    }
}
