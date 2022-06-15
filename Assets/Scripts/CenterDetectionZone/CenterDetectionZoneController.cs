using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using UnityEditor.VFX;

public class CenterDetectionZoneController : MonoBehaviour
{
    private bool isAllowedToTrigger = true;
    [SerializeField] Light spotLight;
    [SerializeField] Material RoomMaterial;
    [SerializeField] GameObject BarScanGameObject;
    [SerializeField] GameObject BodySourceView;
    [SerializeField] GameObject VoixOffGameObject;
    [SerializeField] GameObject ScanSoundGameObject;
    [SerializeField] GameObject rightSoundAmbiance;
    [SerializeField] GameObject leftSoundAmbiance;
    [SerializeField] GameObject contextVoiceGameObject;
    [SerializeField] Transform player;
    [SerializeField] GameObject PermanenteEphemere;
    [SerializeField] GameObject LiberteSecurite;
    [SerializeField] LiberteSecuriteController LiberteSecuriteController;
    [SerializeField] AudioSource onImmobileAudio;
    [SerializeField] AudioSource introBruit;
    [SerializeField] AudioSource bruitBlanc;

    [SerializeField] UnityEngine.VFX.VisualEffect voixOffVFX;

    private float playerZ;
    public float playerX;
    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;

    private Transform barScan = null;

    private Vector3 positionBarScan = new Vector3(0f, 0f, 0f);
    private Vector3 oldPlayerPosition = new Vector3(0f, 0f, 0f);
    private Vector3 oldCharacterPosition = new Vector3(0f, 0f, 0f);
    private bool isDetected = false;

    private AudioSource voixOff;
    private AudioSource scanSound;
    private AudioSource rightSound;
    private AudioSource leftSound;
    private AudioSource contexteVoice;

    private VisualEffect _visualEffect;

    private Transform voixOffTransform;

    private bool isImmobile = false;
    
    private float timerImmobile = 0f;

    public bool isCheckingPosition = false;
    private bool isCheckingImmobile = false;
    public bool canPlayImmobileAudio = false;
    // Start is called before the first frame update
    void Start()
    {        
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        barScan = BarScanGameObject.GetComponent<Transform>();
        positionBarScan = barScan.position;
    
        voixOff = VoixOffGameObject.GetComponent<AudioSource>();
        scanSound = ScanSoundGameObject.GetComponent<AudioSource>();
        rightSound = rightSoundAmbiance.GetComponent<AudioSource>();
        leftSound = leftSoundAmbiance.GetComponent<AudioSource>();
        contexteVoice = contextVoiceGameObject.GetComponent<AudioSource>();

        _visualEffect = VoixOffGameObject.GetComponent<UnityEngine.VFX.VisualEffect>();
        voixOffTransform = VoixOffGameObject.GetComponent<Transform>();
    }

    private IEnumerator ScanUser() {
        Sequence scan = DOTween.Sequence();

        scan.Append(BarScanGameObject.transform.DOMoveY(9.86f, 1.5f).SetEase(Ease.InOutExpo))
            .Append(BarScanGameObject.transform.DOMoveY(-0.1f, 1.5f).SetEase(Ease.InOutExpo));

        yield return new WaitForSeconds(2f);
        bruitBlanc.DOFade(0f, 1.5f);
        introBruit.Play();

        yield return new WaitForSeconds(1.5f);
        bruitBlanc.Stop();
        VoixOffGameObject.SetActive(true);
        voixOffVFX.SetFloat("Count", 100000);
        // voixOffTransform.DOScale(1, .3f);
        voixOff.Play(0);

        yield return new WaitForSeconds(27.5f);
        introBruit.DOFade(0f, .5f);
        LiberteSecurite.SetActive(true);
        // PermanenteEphemere.SetActive(true);
        isCheckingPosition = true;
        isCheckingImmobile = true;
        voixOffVFX.SetFloat("Count", 0);
        
        rightSound.Play();
        leftSound.Play();

        yield return new WaitForSeconds(3f);
        canPlayImmobileAudio = true;
        // Sequence displayColors = DOTween.Sequence();
        // displayColors.Append(RoomMaterial.DOColor(new Color(0.9528301f, 0f, 0f), "_colorA", 0.3f))
        // .Append(RoomMaterial.DOColor(new Color(0.1011229f, 0f, 1f), "_colorB", 0.3f));

        // yield return new WaitForSeconds(15f);
        // contexteVoice.Play(1);
        // Sequence putBlack = DOTween.Sequence();
        // putBlack.Append(RoomMaterial.DOColor(new Color(0f, 0f, 0f), "_colorA", 0.3f))
        // .Append(RoomMaterial.DOColor(new Color(0f, 0f, 0f), "_colorB", 0.3f));

        // onImmobileAudio.PlayDelayed(2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckingPosition) {
            playerZ = player.position.z;
            playerX = player.position.x;
            // Debug.Log("HEY" + playerZ);
            mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
            // TO DO : POSITIONS DE DETECTIONS A REGLER POUR QUE CA MARCHE BIEN
            // if (mainBodyPosition.x > -2f && mainBodyPosition.x < 2f && mainBodyPosition.z > 10f && mainBodyPosition.z < 21f && !isDetected) {
            //     isDetected = true;
            //     scanSound.Play(0);
            //     StartCoroutine(ScanUser());
            // }

            if (playerZ <= 6.5f && playerZ >= 6f && !isDetected) {
                isDetected = true;
                scanSound.Play(0);
                StartCoroutine(ScanUser());
                isCheckingPosition = false;
            }

            if (player.position == oldPlayerPosition) {
                isImmobile = true;
            } else {
                timerImmobile = 0;
                isImmobile = false;
                if (onImmobileAudio.isPlaying == true) {
                    onImmobileAudio.Stop();
                    onImmobileAudio.volume = 0f;
                    // rightSound.DOFade(0.2f, 1f);
                    // leftSound.DOFade(0.2f, 1f);
                    Debug.Log("non immobile and is playing");
                }
                
            }

            // if (mainBodyPosition.x > oldCharacterPosition.x - 0.1f && mainBodyPosition.x < oldCharacterPosition.x + 0.1f) {
            //     isImmobile = true;
            // } else {
            //     timerImmobile = 0;
            //     isImmobile = false;
            // }

            if (isImmobile && isCheckingImmobile) {
                // LANCE LE CONTEUR ET SI IL EST AU DESSUS DE 3 SECONDES, ON VALIDE LA REPONSE
                timerImmobile += Time.deltaTime;
                // Debug.Log("isImmobile " + timerImmobile);
                
                if (onImmobileAudio.isPlaying == false && canPlayImmobileAudio) {
                    // onImmobileAudio.Play();
                    onImmobileAudio.volume = 1f;
                    onImmobileAudio.PlayDelayed(1f);
                    // rightSound.DOFade(0.05f, 4f);
                    // leftSound.DOFade(0.05f, 4f);
                }
                
                
                if (timerImmobile > 9f) {
                    VoixOffGameObject.SetActive(false);
                    // ON VALIDE LA REPONSE ET ON REMET LE TIMER A ZERO ET ON BLOQUE LE VISUEL : ORIGIN NE CHANGE PLUS
                    StartCoroutine(LiberteSecuriteController.ValidLiberteSecurite());
                    timerImmobile = 0;
                    isCheckingPosition = false;
                    isCheckingImmobile = false;
                    canPlayImmobileAudio = false;
                    // if (mainBodyPosition.x < 0f) {
                    //     Debug.Log("right sound chosen");
                    //     rightSound.DOFade(.5f, 1f);
                    //     leftSound.DOFade(0f, 1f);
                    //     StartCoroutine(StopAmbiance(rightSound));
                    // } else if (mainBodyPosition.x > 0f) {
                    //     Debug.Log("left sound chosen");
                    //     leftSound.DOFade(.5f, 1f);
                    //     rightSound.DOFade(0f, 1f);
                    //     StartCoroutine(StopAmbiance(leftSound));
                    // }

                    if (playerX < 0f) {
                        Debug.Log("right sound chosen");
                        rightSound.DOFade(.5f, 1f);
                        leftSound.DOFade(0f, 1f);
                        StartCoroutine(StopAmbiance(rightSound));
                    } else if (playerX > 0f) {
                        Debug.Log("left sound chosen");
                        leftSound.DOFade(.5f, 1f);
                        rightSound.DOFade(0f, 1f);
                        StartCoroutine(StopAmbiance(leftSound));
                    }
                }
            }
        }

        oldCharacterPosition = mainBodyPosition;
        oldPlayerPosition = player.position;
        
    }

    IEnumerator StopAmbiance(AudioSource ambiance) {
        yield return new WaitForSeconds(5f);
        ambiance.DOFade(0f, 2f);

        yield return new WaitForSeconds(2f);
        ambiance.Stop();
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (isAllowedToTrigger) {
    //         Debug.Log("ON LANCE LE SCAN DES LUMIERES");
    //         isAllowedToTrigger = false;

    //         Sequence scan = DOTween.Sequence();

    //         scan.Append(BarScanGameObject.transform.DOMoveY(9.80f, 1.5f).SetEase(Ease.InOutExpo))
    //             .Append(BarScanGameObject.transform.DOMoveY(-0.2f, 1.5f).SetEase(Ease.InOutExpo));
    //             // .Append(voixOff.Play(0));

    //             // yield return new WaitForSeconds(2f);
    //             // introBruit.Play(0);

    //             // yield return new WaitForSeconds(1.5f);
    //             // voixOff.Play(0);
    //             // rightSound.Play(0);
    //             // leftSound.Play(0);
    //             // .Append(RoomMaterial.DOColor(new Color(0.9528301f, 0f, 0f), "_colorA", 0.3f))
    //             // .Append(RoomMaterial.DOColor(new Color(0.1011229f, 0f, 1f), "_colorB", 0.3f));
        
    //         // spotLight.intensity = 0.0f;
    //     }
    // }
}
