using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField] float gravity = -13.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    float velocityY = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    private Transform player = null;

    private Vector3 playerPosition = Vector3.zero;
    private Vector3 oldPlayerPosition = Vector3.zero;

    private float immobileTimer = 0.0f;

    private bool[] sections = {false, false, false, false};

    private int sectionCounter = 0;

    private bool canCheckIfImmobile = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if(lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        player = GetComponent<Transform>();

        playerPosition = player.position;
    }

    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();

        if (canCheckIfImmobile) {
            checkIfImmobileForSection(sectionCounter);
        }

    }

    void checkIfImmobileForSection(int section = 0) {
        Debug.Log("section" + section);
        playerPosition = player.position;
        if (playerPosition == oldPlayerPosition) {
            Debug.Log("immobile");
            immobileTimer += Time.deltaTime;

            if(immobileTimer > 4 && section < 4 && !sections[section]) {
                immobileTimer = 0.0f;
                sectionCounter += 1;
                sections[section] = true;
            }
        } else {
            immobileTimer = 0.0f;
        }

        oldPlayerPosition = player.position;
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if(controller.isGrounded)
            velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
		
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

    }
}