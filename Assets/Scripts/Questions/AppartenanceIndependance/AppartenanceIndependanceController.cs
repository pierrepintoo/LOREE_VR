using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AppartenanceIndependanceController : MonoBehaviour
{
    [SerializeField] UnityEngine.VFX.VisualEffect appartenanceVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect independanceVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect appartenanceContextVFX;
    [SerializeField] UnityEngine.VFX.VisualEffect independanceContextVFX;

    [SerializeField] AudioSource rightSoundAmbiance;
    [SerializeField] AudioSource leftSoundAmbiance;
    [SerializeField] AudioSource leftSoundTexture;
    [SerializeField] AudioSource rightSoundTexture;
    [SerializeField] AudioSource appartenanceContext;
    [SerializeField] AudioSource independanceContext;
    [SerializeField] AudioSource appartenanceBruitage;
    [SerializeField] AudioSource independanceBruitage;

    [SerializeField] AudioSource ticTic;

    [SerializeField] FinalController FinalController;

    [SerializeField] GameObject BodySourceView;

    private BodySourceView _BodySourceViewManager;

    private bool isCheckingPosition = false;
    private bool isViolet = false;
    private bool isBlue = false;
    private bool isImmobile = false;
    private bool canPlayTicTic = false; 
    private float charPositionX = 0f;
    private float countImmobile = 0f;
    private float timerImmobile = 0f;

    private Vector3 mainBodyPosition = new Vector3(0f, 0f, 0f);
    private Vector3 oldCharacterPosition = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        // StartCoroutine(Run());
    }

    // Update is called once per frame
    void Update()
    {
        if (isCheckingPosition) {
            mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
            charPositionX = mainBodyPosition.x;
        }

        if (Mathf.Abs(mainBodyPosition.x - oldCharacterPosition.x) > 0.125f ||
            Mathf.Abs(mainBodyPosition.y - oldCharacterPosition.y) > 0.125f ||
            Mathf.Abs(mainBodyPosition.z - oldCharacterPosition.z) > 0.125f ) 
        {
            countImmobile = 0f;
            isImmobile = false;
            ticTic.Stop();
            timerImmobile = 0f;
        } else {
            countImmobile += 1f;
        }
        
        if (countImmobile > 300f && isImmobile == false && canPlayTicTic == true)  {
            isImmobile = true;
            ticTic.Play();
        }

        if (isImmobile) {
            timerImmobile += Time.deltaTime;
        }

        if (isImmobile && timerImmobile >= 3f && canPlayTicTic == true) {
            StartCoroutine(ValidAI());
            canPlayTicTic = false;
        }

        if (!isViolet && !isBlue) {

            rightSoundAmbiance.volume = 0.5f + (charPositionX * 0.1f);
            leftSoundAmbiance.volume = 0.5f - (charPositionX * 0.1f);

            independanceVFX.SetFloat("Arc", charPositionX.Remap(-8, 8, 0f, 6.29f));
            appartenanceVFX.SetFloat("Arc", charPositionX.Remap(8, -8, 0f, 6.29f));

        } else if (isBlue) {
            appartenanceVFX.SetFloat("Arc", 6.29f);
            independanceBruitage.DOFade(0f, .5f);

        } else if (isViolet) {
            independanceVFX.SetFloat("Arc", 6.29f);
            appartenanceBruitage.DOFade(0f, 0.5f);
        }
        
        oldCharacterPosition = mainBodyPosition;
    }

    private IEnumerator ValidAI() {
        if (charPositionX <= 0f) {

            isBlue = true;
            independanceVFX.SetFloat("Count", 0f);
            
            leftSoundAmbiance.DOFade(0f, 0.5f);
            rightSoundAmbiance.DOFade(0.2f, 0.5f);
            rightSoundTexture.DOFade(0.2f, 0.5f);

            yield return new WaitForSeconds(4f);
            appartenanceVFX.SetFloat("Count", 0f);
            yield return new WaitForSeconds(2.5f);
            appartenanceContext.Play();
            appartenanceContextVFX.SetFloat("Count", 100000f);

        } else if (charPositionX >= 0f) {
            appartenanceVFX.SetFloat("Count", 0f);

            rightSoundAmbiance.DOFade(0f, 0.5f);
            leftSoundAmbiance.DOFade(0.2f, 0.5f);
            leftSoundTexture.DOFade(0.2f, 0.5f);

            isViolet = true;
            yield return new WaitForSeconds(3f);
            independanceVFX.SetFloat("Count", 0f);
            yield return new WaitForSeconds(1.5f);
            independanceContext.Play();
            independanceContextVFX.SetFloat("Count", 100000f);
        }

        yield return new WaitForSeconds(26.5f);
        appartenanceContextVFX.SetFloat("Count", 0f);
        independanceContextVFX.SetFloat("Count", 0f);

        rightSoundAmbiance.DOFade(0f, 0.3f);
        leftSoundAmbiance.DOFade(0f, 0.3f);

        rightSoundTexture.DOFade(0f, .3f);
        leftSoundTexture.DOFade(0f, .3f);

        yield return new WaitForSeconds(.3f);
        rightSoundAmbiance.Stop();
        leftSoundAmbiance.Stop();

        rightSoundTexture.Stop();
        leftSoundTexture.Stop();

        StartCoroutine(FinalController.Run());

        yield return null;

    }

    public IEnumerator Run() {
        isCheckingPosition = true;
        appartenanceVFX.SetFloat("Count", 80);
        appartenanceBruitage.Play();
        appartenanceBruitage.DOFade(0.5f, 1f);
        
        yield return new WaitForSeconds(3f);
        independanceVFX.SetFloat("Count", 20);
        independanceBruitage.Play();
        independanceBruitage.DOFade(0.5f, 1f);
            
        leftSoundAmbiance.DOFade(0.5f, 0.5f);
        rightSoundAmbiance.DOFade(0.5f, 0.5f);
            
        leftSoundAmbiance.Play();
        rightSoundAmbiance.Play();
        yield return new WaitForSeconds(3f);
        canPlayTicTic = true;
    }
}
