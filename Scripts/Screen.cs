using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Screen_c : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected void SetScreen(bool open)
    {
        _canvasGroup.interactable = open;
        _canvasGroup.blocksRaycasts = open;
        _canvasGroup.alpha = open ? 1 : 0;
    }
}
