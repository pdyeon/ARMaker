using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGoogleDrive;

public class GoogleDriveLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DriveLoader();
    }

    public void DriveLoader()
    {
        GoogleDriveFiles.List().Send().OnDone += fileList =>
        {
            foreach(var file in fileList.Files)
            {
                Debug.Log(file.Id + " / " + file.Name);
            }
        };

        //GoogleDriveFiles.Download("fileID").Send().OnDone += file =>
        //{

        //};
    }
}
