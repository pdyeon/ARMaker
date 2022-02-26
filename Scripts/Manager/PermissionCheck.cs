using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;

public class PermissionCheck : MonoBehaviour
{

#if UNITY_EDITOR
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
#else
    void Start()
    {
        StartCoroutine(AskForPermission());
    }
    //    bool onCheck = false;


    //    public void PressBtnCapture()
    //    {
    //        if (onCheck == false)
    //        {
    //            StartCoroutine("PermissionCheckCoroutine");
    //        }
    //    }


    //    IEnumerator PermissionCheckCoroutine()
    //    {
    //        onCheck = true;

    //        yield return new WaitForEndOfFrame();
    //        if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
    //        {
    //            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

    //            yield return new WaitForSeconds(0.2f); // 0.2초의 딜레이 후 focus를 체크하자.
    //            yield return new WaitUntil(() => Application.isFocused == true);

    //            if (Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) == false)
    //            {
    //                // 다이얼로그를 위해 별도의 플러그인을 사용했기 때문에 주석처리. 그냥 별도의 UI를 만들어주면 됨.
    //                //AGAlertDialog.ShowMessageDialog("권한 필요", "스크린샷을 저장하기 위해 저장소 권한이 필요합니다.",
    //                //"Ok", () => OpenAppSetting(),
    //                //"No!", () => AGUIMisc.ShowToast("저장소 요청 거절됨"));

    //                OpenAppSetting(); // 원래는 다이얼로그 선택에서 Yes를 누르면 호출됨.

    //                onCheck = false;
    //                yield break;
    //            }
    //        }

    //        // 권한을 사용해서 처리하는 부분. 스크린샷이나, 파일 저장 등등.

    //        onCheck = false;
    //    }


    //    // 해당 앱의 설정창을 호출한다.
    //    // https://forum.unity.com/threads/redirect-to-app-settings.461140/
    //    private void OpenAppSetting()
    //    {
    //        try
    //        {
    //#if UNITY_ANDROID
    //            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    //            using (AndroidJavaObject currentActivityObject = unityClass.GetStatic<AndroidJavaObject>("currentActivity"))
    //            {
    //                string packageName = currentActivityObject.Call<string>("getPackageName");

    //                using (var uriClass = new AndroidJavaClass("android.net.Uri"))
    //                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromParts", "package", packageName, null))
    //                using (var intentObject = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS", uriObject))
    //                {
    //                    intentObject.Call<AndroidJavaObject>("addCategory", "android.intent.category.DEFAULT");
    //                    intentObject.Call<AndroidJavaObject>("setFlags", 0x10000000);
    //                    currentActivityObject.Call("startActivity", intentObject);
    //                }
    //            }
    //#endif
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogException(ex);
    //        }
    //    }
#endif

    IEnumerator AskForPermission()
    {
#if UNITY_ANDROID
        while (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite) || Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            Permission.HasUserAuthorizedPermission(Permission.Microphone) || Permission.HasUserAuthorizedPermission(Permission.Camera) || !Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {

            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                Permission.RequestUserPermission(Permission.Camera);
            }
            List<bool> permissions = new List<bool>() { false, false, false, false, false };
            List<bool> permissionsAsked = new List<bool>() { false, false, false, false, false };
            List<Action> actions = new List<Action>()
    {
        new Action(() => {
            permissions[0] = Permission.HasUserAuthorizedPermission(Permission.FineLocation);
            if (!permissions[0] && !permissionsAsked[0])
            {
                Permission.RequestUserPermission(Permission.FineLocation);
                permissionsAsked[0] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[1] = Permission.HasUserAuthorizedPermission(Permission.Camera);
            if (!permissions[1] && !permissionsAsked[1])
            {
                Permission.RequestUserPermission(Permission.Camera);
                permissionsAsked[1] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[2] = Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite);
            if (!permissions[2] && !permissionsAsked[2])
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
                permissionsAsked[2] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[3] = Permission.HasUserAuthorizedPermission(Permission.Microphone);
            if (!permissions[3] && !permissionsAsked[3])
            {
                Permission.RequestUserPermission(Permission.Microphone);
                permissionsAsked[3] = true;
                
                return;
            }
        }),
        new Action(() => {
            permissions[4] = Permission.HasUserAuthorizedPermission(Permission.Microphone);
            if (!permissions[4] && !permissionsAsked[4])
            {
                Permission.RequestUserPermission(Permission.Microphone);
                permissionsAsked[4] = true;
                //------------------------------
                PermissionCheckData.instance.SaveData();
                SceneManager.LoadScene("LumAr");
                //------------------------------
                return;
            }
        })
    };
            for (int i = 0; i < permissionsAsked.Count;)
            {
                actions[i].Invoke();
                if (permissions[i])
                {
                    ++i;
                }
                yield return new WaitForEndOfFrame();
            }
        }

#endif
        yield return null;
    }
}