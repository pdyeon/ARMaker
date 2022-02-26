using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediaPlayer : Screen_c
{
    [SerializeField]
    RawImage mediaImage;

    protected override void Start()
    {
        base.Start();
        CloseScreen();
    }


    public void OpenScreen(Texture imageTex)
    {
        gameObject.SetActive(true);
        mediaImage.texture = imageTex;
        SetScreen(true);
    }


    public void CloseScreen()
    {
        gameObject.SetActive(false);
        SetScreen(false);
    }
}
