using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    // 이미지를 감지하면 생성되는 모델링
    [SerializeField]
    private GameObject[] model_Address;

    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> _spawnedBools = new Dictionary<string, bool>();
    private ARTrackedImageManager _trackedImageManager;

    //public Vector3 scaleFactor = new Vector3(.1f, .1f, .1f);
    ARPlaneManager _arPlaneManager;
    Transform camTrans;

    private void Awake()
    {
        camTrans = Camera.main.transform;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        //_arPlaneManager = GetComponent<ARPlaneManager>();

        foreach (var prefab in model_Address)
        {
            var newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            _spawnedPrefabs.Add(newPrefab.name, newPrefab);
            newPrefab.SetActive(false);
        }
    }


    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged -= ImageChanged;
        _trackedImageManager.trackedImagesChanged += ImageChanged;
        //_arPlaneManager.planesChanged += PlaneChanged;
    }


    private void OnDisable()
    {
        //_trackedImageManager.trackedImagesChanged -= ImageChanged;
        //_arPlaneManager.planesChanged -= PlaneChanged;
    }


    ARPlane arPlane;
    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null)
        {
            //arPlane = args.added[0];
            foreach (var plane in args.added)
            {
                if (plane.size.magnitude > .5f)
                {
                    arPlane = plane;
                    _arPlaneManager.enabled = false;
                    break;
                }
            }
        }
        if (args.updated != null)
        {
            foreach (var plane in args.updated)
            {
                if (plane.size.magnitude > 10f)
                {
                    arPlane = plane;
                    _arPlaneManager.enabled = false;
                    break;
                }
            }
        }
    }


    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImg in eventArgs.added)
        {
            if (!_spawnedBools.ContainsKey(trackedImg.referenceImage.name))
                _spawnedBools.Add(trackedImg.referenceImage.name, false);
            UpdateImage(trackedImg);
        }

        foreach (var trackedImg in eventArgs.updated)
        {
            if (trackedImg.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                if (!_spawnedBools[trackedImg.referenceImage.name])
                {
                    UpdateImage(trackedImg);
                }
            }
        }

        foreach (var trackedImg in eventArgs.removed)
        {
            //Debug.Log("MissObj2 : " + trackedImg.name);
            _spawnedBools[trackedImg.referenceImage.name] = false;
            _spawnedPrefabs[trackedImg.referenceImage.name]?.SetActive(false);
        }
    }


    private void UpdateImage(ARTrackedImage trackedImage)
    {
        var name = trackedImage.referenceImage.name;
        var hasKey = _spawnedPrefabs.ContainsKey(name);
        if (!hasKey) return;
        var pos = trackedImage.transform.position;
        AssignGameObject(name, pos);
    }


    GameObject currentPrefab;
    void AssignGameObject(string name, Vector3 newPos)
    {
        if (model_Address != null)
        {
            var targetObj = currentPrefab = _spawnedPrefabs[name];

            targetObj.SetActive(true);
            //targetObj.transform.localScale = scaleFactor;
            targetObj.transform.position = newPos;

            foreach (var go in _spawnedPrefabs.Values)
            {
                if (go.name != name)
                {
                    go.SetActive(false);
                    _spawnedBools[go.name] = false;
                    _spawnedPrefabs[go.name]?.SetActive(false);
                }
                else
                {
                    _spawnedBools[go.name] = true;
                }
            }
        }
    }
}
