using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]

public class PlayerMovementController : MonoBehaviour
{
    public static PlayerMovementController instance;
    public GameObject PlayerModelParent; // Reference to the player model
    [SerializeField] private Animator modelAnimator;

    [Header("Setup")]
    // Movement speed variables
    [SerializeField] float movementSpeed = 5;  

 
    public Vector3 MovementVector; // Vector to store movement input
    private bool canMove = true; // Flag to control player movement
    float defaultSpeed = 0f;
    CharacterController characterController;


    [Header("For rotation")]
    public Transform targetHolder;
    public Transform lookTransform;
    public float lookatSpeed;

    Vector3 relativePos = Vector3.zero;
    Vector3 relativeVector = Vector3.zero;

    // getters and setters
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
    public Animator _Animator { get => modelAnimator; set => modelAnimator = value; }
    public bool CanMove { get => canMove; set => canMove = value; }

    //my code
    [SerializeField] private Canvas hpPopup;
    private Camera cam;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component
        lookTransform.position = transform.position; // Set initial look position to player's position
        defaultSpeed = movementSpeed; // Store default movement speed
        cam = Camera.main;
    }

    Vector2 InputVector = Vector2.zero;

    void Update()
    {
        // Get input from Horizontal and Vertical axes
        if(JoystickController.instance.InputDirection!=Vector2.zero)
        {
            InputVector = JoystickController.instance.InputDirection;
        }
        else
        {
           InputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        MovementVector.x = InputVector.x;
        MovementVector.z = InputVector.y;

        SetAnimatorParameters(); // Set animator parameters based on movement input
        ApplyGravity(); // Apply gravity to the player

        // Move the player if movement is allowed
        if (canMove)
        {
            characterController.Move(MovementVector * movementSpeed * Time.unscaledDeltaTime);
        }

        UpdateLookAtTransform(); // Update look direction to face the camera

        // If there is movement input, calculate the rotation to face the direction
        if (InputVector.x != 0f || InputVector.y != 0f)
        {
            CalculateLookAtRotation(); // Calculate rotation to face movement direction

            if (!canMove) return; // If movement is not allowed, exit the function
        }

        ResetLookAtTarget(); // Reset the look transform position to the target holder
    }

    // function to change the movement speed
    public void ChangeSpeed(float speed)
    {
        movementSpeed = speed;
    }

    // function to reset the movement speed to default
    public void ResetSpeed()
    {
        movementSpeed = defaultSpeed;
    }
      

    // function to set animator parameters based on movement input
    void SetAnimatorParameters()
    {
        if (MovementVector.x != 0f)
        {
            modelAnimator.SetFloat("MovementFloat", Mathf.Abs(MovementVector.x));
        }
        else if (MovementVector.z != 0f)
        {
            modelAnimator.SetFloat("MovementFloat", Mathf.Abs(MovementVector.z));
        }
        else
        {
            modelAnimator.SetFloat("MovementFloat", -1);
        }
    }

    #region PlayerRotation
    // function to calculate the rotation to face the movement direction
    void CalculateLookAtRotation()
    {
        relativeVector.x = InputVector.x;
        relativeVector.z = InputVector.y;
        lookTransform.Translate(relativeVector * Time.unscaledDeltaTime * lookatSpeed);
        relativePos.x = lookTransform.position.x - PlayerModelParent.transform.position.x;
        relativePos.z = lookTransform.position.z - PlayerModelParent.transform.position.z;

        Quaternion rotation = Quaternion.LookRotation(relativePos);
        PlayerModelParent.transform.rotation = rotation;
    }

    // function to apply gravity to the player
    void ApplyGravity()
    {
        MovementVector.y -= 2f * Time.unscaledDeltaTime;
        if (MovementVector.y < -2f)
            MovementVector.y = -2f;
    }

    // function to update the look direction to face the camera
    void UpdateLookAtTransform()
    {
        lookTransform.transform.rotation = Camera.main.transform.rotation;
        lookTransform.transform.eulerAngles = new Vector3(0, lookTransform.transform.eulerAngles.y, 0);
    }

    // function to reset the look transform position to the target holder 
    void ResetLookAtTarget()
    {
        lookTransform.position = targetHolder.transform.position;
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle"))
        {
            ChangeSpeed(movementSpeed * .5f);
            movementSpeed = Mathf.Clamp(movementSpeed, 2, defaultSpeed);
        }
        if(other.gameObject.tag == "Boundary")
        {
            Debug.Log("DED");
            RunningPhaseManager.instance.GameOver();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(movementSpeed<defaultSpeed)
         ResetSpeed();
    }

    public void spawnPopUP(string info)
    {
        Vector3 offset = new Vector3(1f, 2f, 0.1f);
        Canvas hpPop = Instantiate(hpPopup, transform.position + offset, Quaternion.identity);
        TMP_Text hpPopText = hpPop.GetComponentInChildren<TMP_Text>();
        hpPopText.text = info;

        hpPop.transform.LookAt(hpPop.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

        Vector3 sideOffset = new Vector3(.5f, 0f, 0f);
        Rigidbody hpPopRb = hpPop.gameObject.GetComponent<Rigidbody>();
        hpPopRb.AddForce((transform.up + sideOffset) * 150f);
        
        Destroy(hpPop, 0.5f);
    }
}