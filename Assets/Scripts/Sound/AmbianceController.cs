using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceController : MonoBehaviour
{
    [SerializeField] AudioSource rightSoundAmbiance;
    [SerializeField] AudioSource leftSoundAmbiance;
    [SerializeField] GameObject player;
    private Transform character = null;
    private float charPositionX = 0.0f;

    [SerializeField] GameObject BodySourceView;

    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;

    // Start is called before the first frame update
    void Start()
    {
        character = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
        charPositionX = mainBodyPosition.x;
        rightSoundAmbiance.volume = 1.0f + (charPositionX * 0.1f);
        leftSoundAmbiance.volume = 1.0f - (charPositionX * 0.1f);
        // Debug.Log(charPositionX);
    }
}
