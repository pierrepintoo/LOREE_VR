using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LiberteSecuriteController : MonoBehaviour
{
    [SerializeField] CenterDetectionZoneController CenterDetectionZoneController; 

    [SerializeField] UnityEngine.VFX.VisualEffect liberteVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect securiteVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect voixOffVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect PermanenteEphemereVFX;

    [SerializeField] AudioSource voixOffContexteSujetAudio;
    [SerializeField] AudioSource introBruit;
    [SerializeField] AudioSource rightSoundAmbiance;
    [SerializeField] AudioSource leftSoundAmbiance;

    [SerializeField] Transform voixOff;

    [SerializeField] PermanenteEphemereController PermanenteEphemereController;

    [SerializeField] AudioSource EphemereAmbianceBefore;
    [SerializeField] AudioSource PermanenteAmbianceBefore;

    private BodySourceView _BodySourceViewManager;
    private bool isCheckingPosition;
    private bool isGreen = false;
    private bool isOrange = false;
    private float playerX = 0f;
    private float charPositionX = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CenterDetectionZoneController.isCheckingPosition) {
            // UNCOMMENT THAT TO ENABLE KINECT
            // position = CenterDetectionZoneController.mainBodyPosition;
            
            playerX = CenterDetectionZoneController.playerX;
            charPositionX = CenterDetectionZoneController.charPositionX;
            liberteVFX.SetFloat("Arc", playerX);
            securiteVFX.SetFloat("Arc", -playerX);
        }

        if (!isGreen && !isOrange) {

            leftSoundAmbiance.volume = charPositionX.Remap(-5f, 5f, 0f, .3f);
            rightSoundAmbiance.volume = charPositionX.Remap(-5f, 5f, .3f, 0f);

            // leftSoundAmbiance.volume = playerX.Remap(-8f, 8f, 0f, .2f);
            // rightSoundAmbiance.volume = playerX.Remap(-8f, 8f, .2f, 0f);

            // rightSoundAmbiance.volume = 0.5f + (charPositionX * 0.1f);
            // leftSoundAmbiance.volume = 0.5f - (charPositionX * 0.1f);

            // vfx.SetFloat("Arc", playerX.Remap(-8, 8, -10, 10));
            liberteVFX.SetFloat("Arc", charPositionX.Remap(-8, 8, -10, 10));
            securiteVFX.SetFloat("Arc", -charPositionX.Remap(-8, 8, -10, 10));

            if (charPositionX.Remap(-8, 8, -10, 10) == 10f) {
                securiteVFX.SetFloat("Count", 0f);

            } else if (-charPositionX.Remap(-8, 8, -10, 10) == 10f) {
                liberteVFX.SetFloat("Count", 0f);
            }

            // liberteVFX.SetFloat("Arc", playerX.Remap(-8, 8, -10, 10));
            // securiteVFX.SetFloat("Arc", -playerX.Remap(-8, 8, -10, 10));
        } else if (isOrange) {
            securiteVFX.SetFloat("Arc", 10f);
            liberteVFX.SetFloat("Arc", -10f);
        } else if (isGreen) {
            liberteVFX.SetFloat("Arc", 10f);
            securiteVFX.SetFloat("Arc", -10f);
        }
        
    }

    public IEnumerator ValidLiberteSecurite() {
        if (charPositionX.Remap(-8, 8, -10, 10) < 0f) {
            isOrange = true;
            liberteVFX.SetFloat("Count", 0f);
        } else if (charPositionX.Remap(-8, 8, -10, 10) > 0f) {
            securiteVFX.SetFloat("Count", 0f);
            isGreen = true;
        }
        
        yield return new WaitForSeconds(5f);
        liberteVFX.SetFloat("Count", 0f);
        securiteVFX.SetFloat("Count", 0f);

        yield return new WaitForSeconds(3f);
        introBruit.DOFade(.2f, 5f);
        voixOffVFX.SetFloat("Count", 100000);
        // voixOff.DOScale(1f, .3f);
        voixOffContexteSujetAudio.Play();
    
        yield return new WaitForSeconds(25f);
        rightSoundAmbiance.DOFade(0f, 2f);
        leftSoundAmbiance.DOFade(0f, 2f);
        introBruit.DOFade(0f, 5f);
        voixOffVFX.SetFloat("Count", 0);
        PermanenteEphemereVFX.SetFloat("Count", 25000);
        PermanenteEphemereController.isCheckingPositionPE = true;

        PermanenteAmbianceBefore.Play();
        EphemereAmbianceBefore.Play();
        // PermanenteEphemereController.PermanenteAmbiance.DOFade(.5f, .3f);
        // PermanenteEphemereController.EphemereAmbiance.DOFade(.5f, .3f);

        yield return new WaitForSeconds(3f);
        PermanenteEphemereController.canPlayImmobileAudioPEAA = true;
        PermanenteEphemereController.isCheckingImmobilePEAA = true;
    }
}
