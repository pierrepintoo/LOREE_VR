using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightController : MonoBehaviour
{
    public Light spotLight = null;
    // Start is called before the first frame update
    void Start()
    {
        spotLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
