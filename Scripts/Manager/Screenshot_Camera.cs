using DG.Tweening;
using NatSuite.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class Screenshot_Camera : MonoBehaviour
{
    [SerializeField]
    protected GameObject blink;

    private Camera cam;
    bool takeScreenshot;

#if !UNITY_EDITOR
    const string GALLERY_PATH = "/../../../../DCIM/LumAR";           // 생성될 앨범의 이름
#else
    const string GALLERY_PATH = "/Pictures";
#endif

    private void Start()
    {
        //this.enabled = false;
        cam = GetComponent<Camera>();
        _arCameraManager = GetComponent<ARCameraManager>();
    }
    ARCameraManager _arCameraManager;

    private void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }


    private void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;

    }


    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }


    public Text transformT;
    private void Update()
    {
        transformT.text =  transform.position + " / " + transform.rotation;
    }

    public void CameraSwitch()
    {
        if(_arCameraManager.requestedFacingDirection == CameraFacingDirection.World)
            _arCameraManager.requestedFacingDirection = CameraFacingDirection.User;
        else _arCameraManager.requestedFacingDirection = CameraFacingDirection.World;
    }


    private void OnPostRender()
    {
        if (takeScreenshot)
        {
            //Debug.Log("**1");

            takeScreenshot = false;

            RenderTexture renderTexture = cam.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            renderResult.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            blink.SetActive(true);

            //DOVirtual.DelayedCall(.5f, Disappear);
            Invoke("Disappear", .5f);
            byte[] bytes = renderResult.EncodeToJPG();

            var fileName = "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.jpg";
            var path = Application.persistentDataPath + GALLERY_PATH + "/" + fileName;

            if (!System.IO.Directory.Exists(Application.persistentDataPath + GALLERY_PATH))
                System.IO.Directory.CreateDirectory(Application.persistentDataPath + GALLERY_PATH);

            //Debug.Log("Save Picture path : "+ path);
            System.IO.File.WriteAllBytes(path, bytes);

            ReplayCam.RefreshGallery(path);
            //AndroidUtils.RefreshGallery(path);

            RenderTexture.ReleaseTemporary(renderTexture);
            cam.targetTexture = null;
        }
    }


    public void TakeScreenshot()
    {
        //if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        //{
        //    Permission.RequestUserPermission(Permission.Microphone);
        //}

        //if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        //{
        //    Permission.RequestUserPermission(Permission.Camera);
        //}

        cam.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        takeScreenshot = true;
    }


    void Disappear()
    {
        blink.SetActive(false);
        //this.enabled = false;
    }
}
