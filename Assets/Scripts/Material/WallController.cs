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

    // private Material roomMaterial = null;

    private float originGradient = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        RoomMaterial.SetColor("_colorA", new Color(0.1f, 0.1f, 0.1f));
        RoomMaterial.SetColor("_colorB", new Color(0.1f, 0.1f, 0.1f));
        
        character = player.GetComponent<Transform>();
        // roomMaterial = GetComponent<Material>();
        // Debug.Log("material room", roomMaterial.GetObject("_origin"));
        // Debug.Log(roomMaterial._origin);
        charPositionX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _BodySourceViewManager = BodySourceView.GetComponent<BodySourceView>();
        mainBodyPosition = _BodySourceViewManager.mainBodyPosition;
        // charPositionX = mainBodyPosition.x;
        charPositionX = character.position.x;
        originGradient = charPositionX;
        // roomMaterial = GetComponent<Material>();
        // Debug.Log(charPositionX);
        RoomMaterial.SetFloat("_origin", -originGradient);
    }
}
