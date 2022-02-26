using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking_Runtime : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager _trackedImageManager;

    XRReferenceImageLibrary _xrReferenceImageLibrary;

    public Vector3 scaleFactor = new Vector3(.1f, .1f, .1f);

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
        _trackedImageManager.referenceLibrary = _trackedImageManager.CreateRuntimeLibrary(_xrReferenceImageLibrary);
        _trackedImageManager.maxNumberOfMovingImages = 3;
        _trackedImageManager.trackedImagePrefab = placeablePrefabs[0];

        _trackedImageManager.enabled = true;
    }


    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += ImageChanged;
    }


    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= ImageChanged;
    }


    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImg in eventArgs.added)
        {
            //test
            trackedImg.transform.localScale = new Vector3(-trackedImg.referenceImage.size.x, .005f, -trackedImg.referenceImage.size.y);
        }

        foreach (var trackedImg in eventArgs.updated)
        {
            if (trackedImg.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking)
            {
                //test
                trackedImg.transform.localScale = new Vector3(-trackedImg.referenceImage.size.x, .005f, -trackedImg.referenceImage.size.y);
            }
        }
    }
}
