using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterDetectionZoneController : MonoBehaviour
{
    private bool isAllowedToTrigger = true;
    [SerializeField] Light spotLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        if (isAllowedToTrigger) {
            Debug.Log("ON LANCE LE SCAN DES LUMIERES");
            isAllowedToTrigger = false;

            spotLight.intensity = 0.0f;
        }
    }
}
