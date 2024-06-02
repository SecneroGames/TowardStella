
using UnityEngine;

[DisallowMultipleComponent]
public class SpaceShipController : MonoBehaviour
{
    public PlayerDataSO PlayerShipStatSO;
    public ShipStats PlayerShipStats;

    public float fireRate = 1;

    [Header("Ship Turn Visuals Setup")]     
    public float rollSpeed = 2.0f;  // Speed of roll rotation
    public float rollTurnSpeed = 2.0f; // Speed at which the ship turns when rolling
    public float maxRollAngle = 15f; // Maximum roll angle in degrees
    public float yawSpeed = 6.0f; // Speed of yaw rotation

    private Rigidbody rb; // Reference to the Rigidbody component
    private float targetRollAngle = 0f; // Target roll angle
    private float currentRollAngle = 0f; // Current roll angle

    private float tempAcceleration = 0;
    private float currentAcceleration = 0;
    private float currentMaxSpeed = 0;
    private float buffTimer = 0;
    private float blasterTimer = 0;

    private float rollInput;
    private float steeringValue = 0;

    private bool isFlying = false;
    private bool canShoot = false;

    [Header("blaster")]
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float bulletSpeed = 20f;

    //move this to a different thing
    public SpaceLightsController ctrl;

    public float BlasterTimer { get => blasterTimer; set => blasterTimer = value; }

    [SerializeField] private GameObject rockBoundary;
    [SerializeField] private GameObject rockBoundary2;

    private void Start()
    {
        PlayerShipStatSO.ResetToDefaultValues();// edited by me// always default the values
        PlayerShipStats.CopyFromSO(PlayerShipStatSO);

        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        currentAcceleration = PlayerShipStats.acceleration;
        tempAcceleration = PlayerShipStats.acceleration;
        buffTimer = 0;
        currentMaxSpeed = PlayerShipStats.maxSpeed;
        isFlying = true;
     
        if (PlayerShipStats.hasBlaster) blasterTimer = fireRate;
    }

    private void Update()
    {
        if (blasterTimer > 0)
        {
            blasterTimer -= Time.deltaTime;
            if (blasterTimer <= 0)
            {
                canShoot = true;
            }
            else
            {
                canShoot = false;
            }
        }

        //reset speed boost / speed debuff;
        if (buffTimer > 0)
        {            
            buffTimer -= Time.deltaTime;
            if (buffTimer <= 0)
            {
                currentAcceleration = tempAcceleration;
                buffTimer = 0;
                currentMaxSpeed = PlayerShipStats.maxSpeed;
                ctrl.ToggleFTL(false);
            }
        }

        if (!isFlying) return;

        if (steeringValue == 0)
        {
            rollInput = Input.GetAxis("Horizontal");
        }
        else
        {
            rollInput = steeringValue;
        }
    }

    private void FixedUpdate()
    {
        // Calculate the target roll angle based on input, inverse the roll
        if (rollInput != 0)
        {
            targetRollAngle = (rollInput * -1) * maxRollAngle;
        }
        else
        {
            targetRollAngle = 0f;
        }

        // Apply roll rotation towards the target angle
        currentRollAngle = Mathf.Lerp(currentRollAngle, targetRollAngle, Time.fixedDeltaTime * rollSpeed);

        // Clamp the roll angle to the maximum roll angle
        currentRollAngle = Mathf.Clamp(currentRollAngle, -maxRollAngle, maxRollAngle);

        // Calculate the roll rotation
        Quaternion rollRotationDelta = Quaternion.Euler(0f, 0f, currentRollAngle);

        // Calculate yaw rotation based on input
        float yawInput = rollInput;
        Quaternion yawRotationDelta = Quaternion.Euler(0f, yawInput * yawSpeed, 0f);

        // Apply the yaw rotation separately
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, rb.rotation * yawRotationDelta, Time.fixedDeltaTime * yawSpeed));

        // Apply the roll rotation relative to the current rotation
        Quaternion newRotation = rb.rotation * rollRotationDelta;

        // Modify the new rotation to keep rotation around the x-axis at 0
        newRotation = Quaternion.Euler(0f, newRotation.eulerAngles.y, newRotation.eulerAngles.z);

        rb.MoveRotation(Quaternion.Lerp(rb.rotation, newRotation, Time.fixedDeltaTime * rollSpeed));

        // Calculate side-to-side movement based on roll angle
        Vector3 lateralMovement = Vector3.right * rollInput * PlayerShipStats.steering;

        rb.AddRelativeForce(lateralMovement, ForceMode.Force);

        if (isFlying)
        {
            // Apply constant forward force
            rb.AddRelativeForce(Vector3.forward * currentAcceleration, ForceMode.Force);
        }

        // Clamp the velocity to the maximum speed
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentMaxSpeed);
        ctrl.AdjustSpeed(rb.velocity.z * .2f);

        // Return z rotation (roll) to zero when no input
        if (rollInput == 0)
        {
            Quaternion targetRotation = Quaternion.Euler(rb.rotation.eulerAngles.x, rb.rotation.eulerAngles.y, 0f);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rollSpeed));
        }
    }

    private void OnTriggerEnter(Collider otherobject)
    {
        if (otherobject.CompareTag("Obstacle"))
        {
            //play audio
            AudioManager.instance.PlaySFX("HitSFX");
            //stop ship movement
            rb.velocity = Vector3.zero;
            //reset speed
            currentMaxSpeed = PlayerShipStats.maxSpeed;
            //slow down acceleration
            currentAcceleration = PlayerShipStats.acceleration / 3;  
            //
            buffTimer = PlayerShipStats.collisionRecovery;
            Debug.Log("obstacle collided");
        }

        if (otherobject.CompareTag("Speedboost"))
        {
            AudioManager.instance.PlaySFX("BoostSFX");
            ctrl.ToggleFTL(true);
            currentAcceleration *= 2.5f; 
            currentMaxSpeed *= 1.5f; // 1.5 max speed boost
            buffTimer = PlayerShipStats.boostDuration;
            Debug.Log("woosh!");
        }

        if (otherobject.gameObject.CompareTag("Goal"))
        {
            if(RaceTrackManager.Instance.hasLaps)
            {
                RaceTrackManager.Instance.laps++;
            }
            else
            {

            }
        }

        if(otherobject.gameObject.CompareTag("Bound"))
        {
            rockBoundary.SetActive(false);
            rockBoundary2.SetActive(true);
        }
        
        if(otherobject.gameObject.CompareTag("Bound2"))
        {
            rockBoundary.SetActive(true);
            rockBoundary2.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            return;
        }
       
        rb.velocity = Vector3.zero;
        Debug.Log("Barrier hit");
    }

    //blaster
    void Shoot()
    {
        GameObject projectile = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        projectile.SetActive(true);
    }

    public void ButtonShoot()
    {
        if (canShoot)
        {
            Shoot();
            blasterTimer = fireRate;
            canShoot = false;
        }
    }

    //On screen controls
    public void Steer(float val)
    {
        steeringValue = val;
    }

    public void StopShip()
    {
        rb.velocity = Vector3.zero;
        isFlying = false;
    }
}
