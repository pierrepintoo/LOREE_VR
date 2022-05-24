using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CenterDetectionZoneController : MonoBehaviour
{
    private bool isAllowedToTrigger = true;
    [SerializeField] Light spotLight;
    [SerializeField] Material RoomMaterial;
    [SerializeField] GameObject BarScanGameObject;

    private Transform barScan = null;

    private Vector3 positionBarScan = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        barScan = BarScanGameObject.GetComponent<Transform>();
        positionBarScan = barScan.position;
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

            Sequence scan = DOTween.Sequence();

            scan.Append(BarScanGameObject.transform.DOMoveY(9.86f, 1.5f).SetEase(Ease.InOutExpo))
                .Append(BarScanGameObject.transform.DOMoveY(0.14f, 1.5f).SetEase(Ease.InOutExpo ))
                .Append(RoomMaterial.DOColor(new Color(0.9528301f, 0f, 0f), "_colorA", 0.3f))
                .Append(RoomMaterial.DOColor(new Color(0.1011229f, 0f, 1f), "_colorB", 0.3f));
            
            spotLight.intensity = 0.0f;
        }
    }
}
