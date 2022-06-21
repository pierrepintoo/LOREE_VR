using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
using UnityEditor.VFX;

public class FinalController : MonoBehaviour
{

    [SerializeField] GameObject BodySourceView;
    [SerializeField] GameObject FinalGameObject;
    [SerializeField] GameObject QRCode;
    [SerializeField] AudioSource finalVoixOff;
    [SerializeField] UnityEngine.VFX.VisualEffect finalVFX;

    private BodySourceView _BodySourceViewManager;

    private Vector3 mainBodyPosition = new Vector3(0f, 0f, 0f);
    private Vector3 leftHandPosition = new Vector3(0f, 0f, 0f);
    private Vector3 rightHandPosition = new Vector3(0f, 0f, 0f);
    private Vector3 oldCharacterPosition = new Vector3(0f, 0f, 0f);
    private Vector3 averageHandsPosition = new Vector3(0f, 0f, 0f);
    private Vector3 oldAverageHandsPosition = new Vector3(0f, 0f, 0f);
    private Vector3 handsDirection = new Vector3(0f, 0f, 0f);

    private AudioSource FinalAmbiance;

    private float timerImmobile = 0f;
    private float timerInMove = 0f;
    private float timerHandsInMove = 0f;
    private float oldTimerInMove = 0f;
    private float oldTimerHandsInMove = 0f;
    private float countImmobile = 0f;
    private float countHandsImmobile = 0f;

    private bool isImmobile = false;
    private bool isHandsImmobile = false;
    private bool isCheckingImmobile = false;
    private bool isFadingToZero = false;
    // Start is called before the first frame update
    void Start()
    {
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        FinalAmbiance = GetComponent<AudioSource>();

        Run();
    }

    // Update is called once per frame
    void Update()
    {
        mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
        leftHandPosition = _BodySourceViewManager.leftHandPosition;
        rightHandPosition = _BodySourceViewManager.rightHandPosition;

        averageHandsPosition = (leftHandPosition + rightHandPosition) / 2f;
        
        if (Mathf.Abs(mainBodyPosition.x - oldCharacterPosition.x) > 0.125f ||
            Mathf.Abs(mainBodyPosition.y - oldCharacterPosition.y) > 0.125f ||
            Mathf.Abs(mainBodyPosition.z - oldCharacterPosition.z) > 0.125f ) 
            {
            timerImmobile = 0;
            countImmobile = 0f;
        } else {

            countImmobile += 1f;
        }

        if (countImmobile > 300f) {
            isImmobile = true;
            if (isCheckingImmobile == true) {
                timerImmobile += Time.deltaTime;
            }
        } else {
            isImmobile = false;
        }

        if (countImmobile > 15f && isFadingToZero == false) {
            isFadingToZero = true;
            timerInMove = 0f;
        } else {
            isFadingToZero = false;
            timerInMove += mainBodyPosition.x - oldCharacterPosition.x;
        }

        if (timerInMove != oldTimerInMove) {
            // finalVFX.SetFloat("direction", -timerInMove);
        }

        oldCharacterPosition = mainBodyPosition;
        oldTimerInMove = timerInMove;

        if (
            Mathf.Abs(averageHandsPosition.x - oldAverageHandsPosition.x) > 0.1f ||
            Mathf.Abs(averageHandsPosition.y - oldAverageHandsPosition.y) > 0.1f ||
            Mathf.Abs(averageHandsPosition.z - oldAverageHandsPosition.z) > 0.1f 
        ) {
            countHandsImmobile = 0f;
        } else {
            countHandsImmobile += 1f;
        }


        if (countHandsImmobile > 15f) {
            isHandsImmobile = true;
            timerHandsInMove = 0f;
            handsDirection = new Vector3(0f, 0f, 0f);
        } else {
            isHandsImmobile = false;
            Vector3 deltaHandsPosition = averageHandsPosition - oldAverageHandsPosition;
            handsDirection += deltaHandsPosition;

            timerHandsInMove += Mathf.Abs(((deltaHandsPosition.x + deltaHandsPosition.y + deltaHandsPosition.z) / 3f));
        }

        if (timerHandsInMove != oldTimerHandsInMove) {
            finalVFX.SetVector3("directionHands", -handsDirection * 0.005f);
            finalVFX.SetFloat("intensity", timerHandsInMove * 0.03f);
        }

        oldAverageHandsPosition = averageHandsPosition;
        oldTimerHandsInMove = timerHandsInMove;

    }

    public IEnumerator Run() {
        finalVFX.SetFloat("Count", 100000f);
        FinalAmbiance.Play();
        FinalAmbiance.DOFade(1.5f, .5f);
        finalVoixOff.Play();

        yield return new WaitForSeconds(43.5f);
        QRCode.SetActive(true);
    }
}
