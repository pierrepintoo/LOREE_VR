using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftSoundController : MonoBehaviour
{
    [SerializeField] GameObject player;
    AudioSource audioSource;
    private float charPositionX = 0.0f;
    private Transform character = null;

    [SerializeField] AudioSource RightAmbianceSound;
    void Start()
    {
        character = player.GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.L)) {
            audioSource.volume = 1;
            Debug.Log("L");
            StartCoroutine(Fade());
        }

        // charPositionX = character.position.x;
        // Debug.Log(charPositionX);
        // audioSource.volume = -(charPositionX * 0.1f);
    }

    private IEnumerator Fade() {
            GameObject.Find("RightSoundAmbiance").GetComponent<AudioSource>().volume = 0;
            audioSource.panStereo = 0;

            yield return new WaitForSeconds(5);

            GameObject.Find("RightSoundAmbiance").GetComponent<AudioSource>().volume = 0.5f;
            audioSource.panStereo = -1;
            yield return null;
    }
}