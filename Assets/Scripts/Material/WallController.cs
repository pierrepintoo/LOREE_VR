using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] Material RoomMaterial;
    [SerializeField] GameObject player;
    [SerializeField] GameObject BodySourceView;

    private BodySourceView _BodySourceViewManager;
    private Vector3 mainBodyPosition;

    private float charPositionX = 0.0f;
    private Transform character = null;
    private float previousCharPosX = 0.0f;

    private bool isFixed = false;

    // private Material roomMaterial = null;

    private float originGradient = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        RoomMaterial.SetColor("_colorA", new Color(0.1f, 0.1f, 0.1f));
        RoomMaterial.SetColor("_colorB", new Color(0.1f, 0.1f, 0.1f));
        
        character = player.GetComponent<Transform>();
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        // roomMaterial = GetComponent<Material>();
        // Debug.Log("material room", roomMaterial.GetObject("_origin"));
        // Debug.Log(roomMaterial._origin);
        charPositionX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if (charPositionX > previousCharPosX - 0.01f && charPositionX < previousCharPosX + 0.01f) {
        //     isFixed = true;
        // } else {
        //     isFixed = false;
        //     previousCharPosX = charPositionX;
        // }

        // if (isFixed) {
        //     Debug.Log("FIXED");
        // } else {
        //     Debug.Log("NOT FIXEEEEEEEEEEEEEEED");
        // }
        mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
        charPositionX = -mainBodyPosition.x;
        // charPositionX = character.position.x;
        originGradient = charPositionX;
        // roomMaterial = GetComponent<Material>();
        // Debug.Log(charPositionX);
        RoomMaterial.SetFloat("_origin", -originGradient);
    }
}
