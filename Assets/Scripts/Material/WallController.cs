using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] Material RoomMaterial;
    [SerializeField] GameObject player;

    private float charPositionX = 0.0f;
    private Transform character = null;

    // private Material roomMaterial = null;

    private float originGradient = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        character = player.GetComponent<Transform>();
        // roomMaterial = GetComponent<Material>();
        // Debug.Log("material room", roomMaterial.GetObject("_origin"));
        // Debug.Log(roomMaterial._origin);
        charPositionX = character.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        charPositionX = character.position.x;
        originGradient = charPositionX;
        // roomMaterial = GetComponent<Material>();
        RoomMaterial.SetFloat("_origin", -originGradient);
    }
}
