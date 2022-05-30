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
    [SerializeField] GameObject BodySourceView;
    [SerializeField] GameObject VoixOffGameObject;

    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;

    private Transform barScan = null;

    private Vector3 positionBarScan = new Vector3(0f, 0f, 0f);

    private bool isDetected = false;

    private AudioSource voixOff;

    // Start is called before the first frame update
    void Start()
    {
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        barScan = BarScanGameObject.GetComponent<Transform>();
        positionBarScan = barScan.position;
    
        voixOff = VoixOffGameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        mainBodyPosition = _BodySourceViewManager.mainBodyPosition;

        if (mainBodyPosition.x > 2.9f && mainBodyPosition.x < 3.3f && mainBodyPosition.z > 19f && mainBodyPosition.z < 21f && !isDetected) {
            voixOff.Play(0);
            
            Sequence scan = DOTween.Sequence();

            scan.Append(BarScanGameObject.transform.DOMoveY(9.86f, 1.5f).SetEase(Ease.InOutExpo))
                .Append(BarScanGameObject.transform.DOMoveY(-0.1f, 1.5f).SetEase(Ease.InOutExpo));

                // .Append(voixOff.Play())
                // .Append(RoomMaterial.DOColor(new Color(0.9528301f, 0f, 0f), "_colorA", 0.3f))
                // .Append(RoomMaterial.DOColor(new Color(0.1011229f, 0f, 1f), "_colorB", 0.3f));

            isDetected = true;
        }

    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (isAllowedToTrigger) {
            Debug.Log("ON LANCE LE SCAN DES LUMIERES");
            isAllowedToTrigger = false;

            Sequence scan = DOTween.Sequence();

            scan.Append(BarScanGameObject.transform.DOMoveY(9.80f, 1.5f).SetEase(Ease.InOutExpo))
                .Append(BarScanGameObject.transform.DOMoveY(-0.2f, 1.5f).SetEase(Ease.InOutExpo));
                // .Append(voixOff.Play(0));

                yield return new WaitForSeconds(3.5f);

                voixOff.Play(0);
            
                // .Append(RoomMaterial.DOColor(new Color(0.9528301f, 0f, 0f), "_colorA", 0.3f))
                // .Append(RoomMaterial.DOColor(new Color(0.1011229f, 0f, 1f), "_colorB", 0.3f));
            
            spotLight.intensity = 0.0f;
        }
    }
}
