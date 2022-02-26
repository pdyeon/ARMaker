using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public int gpsAccuracyInMeters = 10;
    public Text latitude;
    public Text longitude;
    public Text altitude;

    readonly WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
    readonly WaitForSecondsRealtime waitUpdate = new WaitForSecondsRealtime(5f);


    private void OnEnable()
    {
        StartCoroutine(StartGPS());
    }
    //36.402
    //127.4007
    //109.6

    IEnumerator StartGPS()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return wait;
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to GPS");
            yield break;
        }
        else
        {
            while (enabled)
            {
                Debug.Log("Update to GPS");
                // Access granted and location value could be retrieved
                latitude.text = Input.location.lastData.latitude.ToString();
                longitude.text = Input.location.lastData.longitude.ToString();
                altitude.text = Input.location.lastData.altitude.ToString();
                yield return waitUpdate;
            }
            //horizontalAccuracy.text = Input.location.lastData.horizontalAccuracy.ToString();
            //timestamp.text = Input.location.lastData.timestamp.ToString();
            //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
        
        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }


    private void OnDisable()
    {
        Input.location.Stop();
    }
}
