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
    public AudioSource EphemereAmbiance;
    public AudioSource PermanenteAmbiance;

    [SerializeField] UnityEngine.VFX.VisualEffect voixOffPermanenteVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect voixOffEphemereVFX;

    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;
    private float oldPlayerPositionX;
    private VisualEffect vfx;
    private float playerX;
    private bool isBlue = false;
    private bool isRose = false;
    private bool isImmobile = false;
    public bool canPlayImmobileAudioPE = true;
    public bool isCheckingImmobilePE = true;

    private float charPositionX = 0.0f;

    private float deltaPosition = 1f;

    private float oldPositionX = 0f;

    private float timerImmobile = 0f;

    public bool isCheckingPositionPE = true;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ValidPermanenteEphemere());

        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckingPositionPE == true) {
        // KINECT VERSION
            // mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
            // deltaPosition += Mathf.Abs(charPositionX - oldPositionX);
            // charPositionX = -mainBodyPosition.x;
            // charPositionX -= 1f;
            // Debug.Log(Mathf.Abs(charPositionX - oldPositionX));
            // if (Mathf.Abs(playerX - oldPositionX) <= 0.005) {
            //     deltaPosition = 1f;
            //     Debug.Log("Reset");
            // } else {
            //     Debug.Log(deltaPosition);
            // }
            // TO DO : CHECK IF IT WORKS WITH KINECT
            // vfx.SetFloat("origin", charPositionX);



            playerX = player.position.x;
            deltaPosition += Mathf.Abs(playerX - oldPositionX) / 50;

            if (Mathf.Abs(playerX - oldPlayerPositionX) <= 0.005) {
                deltaPosition = 1f;
                Debug.Log("Reset");
            } else {
                Debug.Log(deltaPosition);
            }

            vfx.SetFloat("Intensity", deltaPosition);

            
            // vfx.SetFloat("direction", charPositionX * 5 / 10);
            // if (charPositionX < 0) {
            //     vfx.SetFloat("origin", Mathf.Max(charPositionX, -10f));
            //     // vfx.SetFloat("direction", Mathf.Max(Mathf.Min(charPositionX, -1f), -2f));

            // } else if (charPositionX > 0) {
            //     vfx.SetFloat("origin", Mathf.Min(charPositionX, 10f));
            //     // vfx.SetFloat("direction", Mathf.Min(Mathf.Max(charPositionX, 1f), 2f));
            // }
            // vfx.SetFloat("drag", charPositionX);
            // Debug.Log(charPositionX);

            if (player.position.x == oldPlayerPositionX) {
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

            
            if (isImmobile && isCheckingImmobilePE) {
                // LANCE LE CONTEUR ET SI IL EST AU DESSUS DE 3 SECONDES, ON VALIDE LA REPONSE
                timerImmobile += Time.deltaTime;
                // Debug.Log("isImmobile " + timerImmobile);
                
                if (onImmobileAudio.isPlaying == false && canPlayImmobileAudioPE) {
                    // onImmobileAudio.Play();
                    onImmobileAudio.volume = 1f;
                    onImmobileAudio.PlayDelayed(1f);
                    // rightSound.DOFade(0.05f, 4f);
                    // leftSound.DOFade(0.05f, 4f);
                }
                
                
                if (timerImmobile > 9f) {
                    canPlayImmobileAudioPE = false;
                    if (playerX <= 0) {
                        isRose = true;
                        PermanenteAmbiance.DOFade(1f, .5f);
                        EphemereAmbiance.DOFade(0f, .5f);
                    } else if (playerX >= 0) {
                        isBlue = true;
                        EphemereAmbiance.DOFade(1f, .5f);
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
                EphemereAmbiance.volume = (0.5f + (playerX * 0.1f)).Remap(-8f, 8f, 0f, 0.2f);
                PermanenteAmbiance.volume = (0.5f - (playerX * 0.1f)).Remap(-8f, 8f, 0f, 0.2f);

                // EphemereAmbiance.volume = 0.5f + (charPositionX * 0.1f);
                // PermanenteAmbiance.volume = 0.5f - (charPositionX * 0.1f);
                vfx.SetFloat("origin", playerX.Remap(-8, 8, -10, 10));
            } else if (isRose) {
                vfx.SetFloat("origin", -10);
            } else if (isBlue) {
                vfx.SetFloat("origin", 10);
            }
        }
        oldPositionX = charPositionX;
        oldPlayerPositionX = player.position.x;
    }

    public IEnumerator ValidPermanenteEphemere() {
        yield return new WaitForSeconds(5f);
        vfx.SetFloat("Count", 0f);
        
        yield return new WaitForSeconds(2f);

        if (isBlue) {
            voixOffPermanenteVFX.SetFloat("Count", 100000);
            PermanenteAmbiance.DOFade(.1f, 1f);
            
            yield return new WaitForSeconds(1f);
            EphemereContexte.Play();
        } else if (isRose) {
            voixOffEphemereVFX.SetFloat("Count", 100000);
            EphemereAmbiance.DOFade(.1f, 1f);
            
            yield return new WaitForSeconds(1f);
            PermanenteContexte.Play();
        }
    }

}
