using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour
{
    Gyroscope _gyro;
    GameObject _cameraContainer;
    Quaternion _rotation;
    WebCamDevice[] devices;
    WebCamTexture _cam;
    bool _arReady;

    public RawImage background;
    public AspectRatioFitter fit;


    private void Start()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.Log("This devices dose not have GyroScope");
            return;
        }

        devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            return;
        }

        for(int i=0; i< devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                _cam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                break;
            }
        }

        if(_cam == null)
        {
            Debug.Log("This devices does not have back camera");
            return;
        }

        _cameraContainer = new GameObject("Camera Container");
        _cameraContainer.transform.position = transform.position;
        transform.SetParent(_cameraContainer.transform);

        _gyro = Input.gyro;
    }


    public void SetGyroCamera(bool turnOn)
    {
        _gyro.enabled = turnOn;

        if (turnOn)
        {
            _cameraContainer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            _rotation = new Quaternion(0f, 0f, 1f, 0f);

            _cam.Play();
            background.texture = _cam;
        }
        else
        {
            _cam.Stop();
        }
        _arReady = turnOn;
    }


    private void Update()
    {
        if (!_arReady) return;

        var ratio = (float)_cam.width / (float)_cam.height;
        fit.aspectRatio = ratio;

        var scaleY = _cam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        var orient = -_cam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        transform.localRotation = _gyro.attitude * _rotation;
    }
}
