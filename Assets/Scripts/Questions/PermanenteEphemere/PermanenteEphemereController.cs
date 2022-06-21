using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public static class ExtensionMethods {
    
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
}

public class PermanenteEphemereController : MonoBehaviour
{

    [SerializeField] GameObject BodySourceView;
    [SerializeField] Transform player;
    [SerializeField] AudioSource contexte;
    [SerializeField] AudioSource onImmobileAudio;
    [SerializeField] AudioSource PermanenteContexte;
    [SerializeField] AudioSource EphemereContexte;
    [SerializeField] AudioSource EphemereAmbiance;
    [SerializeField] AudioSource PermanenteAmbiance;
    [SerializeField] AudioSource EphemereTexture;
    [SerializeField] AudioSource PermanenteTexture;
    [SerializeField] AudioSource Shutdown;
    [SerializeField] AudioSource VoixOffAfterShutdown;
    [SerializeField] AudioSource PermanenteBruitage;
    [SerializeField] AudioSource EphemereBruitage;

    [SerializeField] UnityEngine.VFX.VisualEffect voixOffPermanenteVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect voixOffEphemereVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect voixOffAfterShutdownVFX;

    [SerializeField] AppartenanceIndependanceController AppartenanceIndependanceController;

    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;
    private float oldPlayerPositionX;
    private VisualEffect vfx;
    private float playerX;
    private bool isBlue = false;
    private bool isRose = false;
    private bool isImmobile = false;
    public bool canPlayImmobileAudioPEAA = false; // SET TO FALSE FOR PROD
    public bool isCheckingImmobilePEAA = false; // SET TO FALSE FOR PROD

    private float charPositionX = 0.0f;

    private float deltaPosition = 1f;

    private float oldCharacterPositionX = 0f;

    private float timerImmobile = 0f;

    public bool isCheckingPositionPE = false;

    // Start is called before the first frame update
    void Start()
    {
        
        // StartCoroutine(test());
        // test();
        // playAmbiances();
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        vfx = GetComponent<VisualEffect>();
    }

    private void test() {
            // voixOffPermanenteVFX.SetFloat("Count", 100000);
        // yield return new WaitForSeconds(8f);
        // Shutdown.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(canPlayImmobileAudioPE);
        if (isCheckingPositionPE == true) {
        // KINECT VERSION
            mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
            // deltaPosition += Mathf.Abs(charPositionX - oldCharacterPositionX);
            charPositionX = -mainBodyPosition.x;
            charPositionX -= 1f;
            // Debug.Log(Mathf.Abs(charPositionX - oldCharacterPositionX));
            // if (Mathf.Abs(playerX - oldCharacterPositionX) <= 0.005) {
            //     deltaPosition = 1f;
            //     Debug.Log("Reset");
            // } else {
            //     Debug.Log(deltaPosition);
            // }
            // TO DO : CHECK IF IT WORKS WITH KINECT
            // vfx.SetFloat("origin", charPositionX);



            playerX = player.position.x;

            if (Mathf.Abs(charPositionX - oldCharacterPositionX) > 0.1f) {
                isImmobile = false;
                timerImmobile = 0;
                onImmobileAudio.Stop();
                deltaPosition += Mathf.Abs(playerX - oldCharacterPositionX) / 10;
            } else {
                deltaPosition = 1f;
                isImmobile = true;
            }

            vfx.SetFloat("Intensity", deltaPosition);
            
            if (isImmobile && isCheckingImmobilePEAA) {
                // LANCE LE CONTEUR ET SI IL EST AU DESSUS DE 3 SECONDES, ON VALIDE LA REPONSE
                timerImmobile += Time.deltaTime;
                // Debug.Log("isImmobile " + timerImmobile);
                if (onImmobileAudio.isPlaying == false && canPlayImmobileAudioPEAA == true && timerImmobile >= 2f) {
                    onImmobileAudio.volume = 1f;
                    onImmobileAudio.Play();
                }
                
                
                if (timerImmobile > 5f) {
                    canPlayImmobileAudioPEAA = false;
                    isCheckingImmobilePEAA = false;
                    if (charPositionX.Remap(-8, 8, -10, 10) >= 0) {
                        Debug.Log("t'es à gauche");
                        isRose = true;
                        PermanenteAmbiance.DOFade(.5f, .5f);
                        EphemereAmbiance.DOFade(0f, 2f);
                    } else if (charPositionX.Remap(-8, 8, -10, 10) <= 0) {
                        Debug.Log("t'es à droite");
                        isBlue = true;
                        EphemereAmbiance.DOFade(.5f, 2f);
                        PermanenteAmbiance.DOFade(0f, .5f);
                    }

                    StartCoroutine(ValidPermanenteEphemere());
                }
            }

            // if (mainBodyPosition.x > oldCharacterPosition.x - 0.1f && mainBodyPosition.x < oldCharacterPosition.x + 0.1f) {
            //     isImmobile = true;
            // } else {
            //     timerImmobile = 0;
            //     isImmobile = false;
            // }


            if (!isBlue && !isRose) {
                PermanenteAmbiance.volume = charPositionX.Remap(-5f, 5f, 0f, .3f);
                EphemereAmbiance.volume = charPositionX.Remap(-5f, 5f, .3f, 0f);

                vfx.SetFloat("origin", -charPositionX.Remap(-8, 8, -8, 8));
            } else if (isRose) {
                vfx.SetFloat("origin", -10);
            } else if (isBlue) {
                vfx.SetFloat("origin", 10);
            }
        }
        oldCharacterPositionX = charPositionX;
        oldPlayerPositionX = player.position.x;
    }

    IEnumerator ValidPermanenteEphemere() {
        yield return new WaitForSeconds(5f);
        vfx.SetFloat("Count", 0f);
        
        if (isBlue) {
            PermanenteAmbiance.DOFade(.3f, 2f);
            PermanenteTexture.DOFade(.3f, 2f);
            EphemereAmbiance.DOFade(0f, 2f);
            EphemereBruitage.DOFade(0f, 2f);
            
            yield return new WaitForSeconds(2f);
            EphemereBruitage.Stop();
            voixOffEphemereVFX.SetFloat("Count", 100000);
            EphemereContexte.Play();
            Shutdown.Play();
            yield return new WaitForSeconds(6f);
            PermanenteAmbiance.DOFade(1f, 8f);

        } else if (isRose) {
            EphemereAmbiance.DOFade(.3f, 2f);
            PermanenteAmbiance.DOFade(0f, 2f);
            PermanenteBruitage.DOFade(0f, 2f);
            EphemereTexture.DOFade(.3f, 2f);
            
            yield return new WaitForSeconds(2f);
            PermanenteBruitage.Stop();
            voixOffPermanenteVFX.SetFloat("Count", 100000);
            PermanenteContexte.Play();
            Shutdown.Play();

            yield return new WaitForSeconds(6f);
            EphemereAmbiance.DOFade(1f, 8f);
        }

        yield return new WaitForSeconds(4f);
        PermanenteAmbiance.Stop();
        PermanenteTexture.Stop();
        EphemereAmbiance.Stop();
        EphemereTexture.Stop();
        voixOffPermanenteVFX.SetFloat("Count", 0);
        voixOffEphemereVFX.SetFloat("Count", 0);


        yield return new WaitForSeconds(3f);
        
        VoixOffAfterShutdown.Play();
        voixOffAfterShutdownVFX.SetFloat("Count", 100000f);

        yield return new WaitForSeconds(18.5f);

        voixOffAfterShutdownVFX.SetFloat("Count", 0f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(AppartenanceIndependanceController.Run());
    }

    public void playAmbiances() {
        PermanenteAmbiance.DOFade(.5f, 0.5f);
        EphemereAmbiance.DOFade(.5f, 0.5f);
        PermanenteAmbiance.Play();
        PermanenteBruitage.Play();
        EphemereAmbiance.Play();
        EphemereBruitage.Play();
    }

}
