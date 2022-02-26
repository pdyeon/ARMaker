using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermissionCheckData : MonoBehaviour
{
    public static PermissionCheckData instance;

    private void Awake()
    {
        instance = this;

        //만약 데이터가 없다면 데이터 생성
        if(!PlayerPrefs.HasKey("PermissionData"))
        {
            CreateData();
        }
        
        if(PlayerPrefs.GetInt("PermissionData") == 1)
        {
            SceneManager.LoadScene("LumAr");
        }
    }

    private void CreateData()
    {
        PlayerPrefs.SetInt("PermissionData", 0);
        PlayerPrefs.Save();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("PermissionData", 1);
        PlayerPrefs.Save();
    }
}
