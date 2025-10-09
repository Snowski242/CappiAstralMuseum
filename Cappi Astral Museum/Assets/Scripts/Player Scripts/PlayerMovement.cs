using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [Header("Controller Additions")]
    public CharacterController characterController;
    public Transform cam;


    public Animator animator;
    Rigidbody rb;

    public string state = "idle";

    //[HideInInspector]
    public bool canMove = true;
    public bool canAttack = true;
    [HideInInspector]
    public bool isAttacking;
    [HideInInspector]
    public bool hanging = false;
    [SerializeField] float decel = 0.04f;
    [SerializeField] float accel = 0.3f;

    [SerializeField] bool canDash = true;

    [SerializeField] bool canDoubleJump = false;

    [SerializeField]
    private float hangOffset = 2.1f;

    public float speed = 6f;
    public float maxSpeed = 9f;
    public float jump = 12f;
    public float rollSpeed = 8f;
    private bool canDodgeRoll = true;
    public float dashTime;

    [Header("Flags for movement")]
    public float tensionGaugeMax = 120f;
    public float tensionGauge;
    private bool canAirBoost = true;
    public float airBoostTime = 50f;
    private float airBoostTimeMax = 50f;
    public float revTimeMax = 180;
    public float revTime;
    public float dashPadTimer;
    public float ringBoostTimer;
    public float homingTime = 0;

    [Header("Ground Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f; //checks if there is a ground
    public LayerMask groundMask;
    //[HideInInspector]
    public bool isGrounded;
    public Transform ledgeCheck;

    [Header("Simulated Gravity Settings")]
    public Vector3 transformVelocity; //this applies the gravity and causes the player to fall
    public float gravity = -15.81f; //how heavy gravity gets
    [HideInInspector]
    public float turnSmoothTime = 0.1f; //allows for smoother turning of the model
    float turnSmoothVelocity;
    public Homing homing1;

    [Header("Sound and VFX")]
    public GameObject groundHitFX;
    public AudioClip groundHitSound;
    public GameObject airDashFX;
    public AudioClip airDashSound;
    [SerializeField] ParticleSystem cloudVFX;
    [SerializeField] ParticleSystem runCloudVFX;
    public ParticleSystem railFX;

    [Header("Animations")]
    private string currentState;
    public const string PLAYER_IDLE = "CappiStance";
    const string PLAYER_BATTLE_IDLE = "CappiBMStance";
    const string PLAYER_RUN = "CappiWalk";
    const string PLAYER_JUMP = "CappiJumpStart";
    const string PLAYER_FALLING = "CappiJump";
    const string PLAYER_ROLL = "CappiRoll";
    const string PLAYER_AIR_ATTACK = "Player_air_attack";

    [Header("Hitboxes")]
    public GameObject homingHB;

    

    private float runTimer = 0;

    void Start()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        tensionGauge = tensionGaugeMax;
        homingHB.SetActive(false);
    }



    void Update()
    {


        States();
        Animations();
        LedgeGrab();



    }

    private void States()
    {

        if (state == "idle")
        {
            //ground check
            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if(isGrounded)
                {
                    canDoubleJump = true;
                }
            }

            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = 0f;
            }

            if (!isGrounded && !isAttacking && transformVelocity.y < 0)
            {
                state = "fall";
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("crouch");
                state = "crouch";
            }

            if (Input.GetButtonDown("Fire3"))
            {
                runCloudVFX.Play();
                Debug.Log("rev");
                state = "rev";
            }


            speed -= decel;
            if(speed < 0f)
            {
                speed = 0;
            }

            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if(speed > 0)
                {
                    Vector3 moveDir = transform.rotation * Vector3.forward;
                    characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    if (horizontal != 0f || vertical != 0f)
                    {
                        runCloudVFX.Play();
                        state = "walk";
                    }


                }

                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    hanging = false;
                    cloudVFX.Play();
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "jump")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                if (speed < maxSpeed)
                {
                    speed += (accel/2);
                }
            }
            else
            {
                speed -= 0.011f;
                if (speed < 0f)
                {
                    speed = 0;
                }
            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {

                float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * speed);
            }
            else
            {
                Vector3 moveDir = transform.rotation * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * speed);
            }

            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y += gravity * Time.deltaTime;

            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("stomp");
                state = "stomp";
            }

            if (Input.GetButtonDown("Fire3") && canAirBoost && tensionGauge > 20f)
            {
                tensionGauge -= 15f;
                Instantiate(airDashFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(airDashSound, transform.position);
                Debug.Log("boost (jump)");
                airBoostTime = airBoostTimeMax;
                transformVelocity.y = Mathf.Sqrt(jump * -0.75f * gravity);
                canAirBoost = false;
                cloudVFX.Play();
                state = "airboost";
            }

            if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump)
            {
                Debug.Log("DJ");
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                canDoubleJump = false;
                cloudVFX.Play();
                state = "jump";
            }

            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = 0f;
            }

            if (!isGrounded && !isAttacking && transformVelocity.y <= 0)
            {
                state = "fall";
            }

            if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack && transformVelocity.y <= 0)
            {
                cloudVFX.Play();
                state = "idle";

            }
            if (horizontal != 0 && isGrounded && transformVelocity.y <= 0 || vertical != 0 && isGrounded && transformVelocity.y <= 0)
            {
                cloudVFX.Play();
                runCloudVFX.Play();
                state = "walk";

            }

            if (!isGrounded && !isAttacking && transformVelocity.y <= 0)
            {
                state = "fall";
            }
        }
        else if (state == "fall")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if(horizontal != 0 || vertical != 0)
            {
                if (speed < maxSpeed)
                {
                    speed += (accel / 2);
                }
            }
            else
            {
                speed -= 0.011f;
                if (speed < 0f)
                {
                    speed = 0;
                }
            }

            if (!isGrounded && !isAttacking && transformVelocity.y < -20f)
            {
                transformVelocity.y = -20f;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("stomp");
                state = "stomp";
            }


            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (direction.magnitude >= 0.1f)
            {

                float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * speed);
            }
            else
            {
                Vector3 moveDir = transform.rotation * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * speed);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y += gravity * Time.deltaTime;

            if (Input.GetButtonDown("Fire3") && canAirBoost && tensionGauge > 20f)
            {
                tensionGauge -= 15f;
                Instantiate(airDashFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(airDashSound, transform.position);
                Debug.Log("boost (jump)");
                airBoostTime = airBoostTimeMax;
                transformVelocity.y = Mathf.Sqrt(jump * -0.75f * gravity);
                canAirBoost = false;
                cloudVFX.Play();
                state = "airboost";
            }

            if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump)
            {
                Debug.Log("DJ");
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                canDoubleJump = false;
                cloudVFX.Play();
                state = "jump";
            }

            if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack)
            {
                cloudVFX.Play();
                state = "idle";

            }
            if (horizontal != 0 && isGrounded || vertical != 0 && isGrounded)
            {
                cloudVFX.Play();
                runCloudVFX.Play();
                state = "walk";

            }


        }
        else if (state == "walk")
        {
            //ground check
            

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                transformVelocity.y = -5f;
            }

            if (!isGrounded && !isAttacking && transformVelocity.y < 0)
            {
                runCloudVFX.Stop();
                state = "fall";
            }



            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (horizontal != 0 || vertical != 0)
                {
                    if (speed < maxSpeed)
                    {
                        speed += accel;
                    }
                }
                else
                {
                    speed -= decel;
                    if (speed < 0f)
                    {
                        speed = 0;
                    }
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }

                }

                if (Input.GetButtonDown("Fire2"))
                {
                    Debug.Log("slide");
                    speed += 2;
                    state = "slide";
                }

                if (horizontal == 0f && vertical == 0f)
                {
                    runCloudVFX.Stop();
                    state = "idle";
                }

                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded && canDodgeRoll)
                {
                    runCloudVFX.Stop();
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    runCloudVFX.Stop();
                    hanging = false;
                    cloudVFX.Play();
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "hanging")
        {
            speed = 0;

            if (Input.GetButtonDown("Jump"))
            {
                hanging = false;
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                isGrounded = false;
                state = "jump";
            }
        }
        else if (state == "airboost")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            characterController.height = 1.15f;
            characterController.center = new Vector3(0f, 0.55f, 0f);

            speed = 12;

            float airDashSpeed = 24f;

            airBoostTime -= 0.68f;
            if ((airBoostTime < 0))
            {
                airBoostTime = 0;
            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (airBoostTime <= 0)
            {

                state = "fall";
            }
            else
            {
                //float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = transform.rotation * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * airDashSpeed);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y += gravity * Time.deltaTime;

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("stomp");
                state = "stomp";
            }

            if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump)
            {
                Debug.Log("DJ");
                canAirBoost = true;
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                canDoubleJump = false;
                cloudVFX.Play();
                state = "jump";
            }

            if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack && airBoostTime < 3)
            {
                cloudVFX.Play();
                state = "idle";

            }
            if (horizontal != 0 && isGrounded && airBoostTime < 3  || vertical != 0 && isGrounded && airBoostTime < 3)
            {
                cloudVFX.Play();
                state = "walk";

            }


        }
        else if (state == "stomp")
        {
            speed = 0;

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            transformVelocity.x = 0f;
            transformVelocity.z = 0f;
            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y -= 3f;

            if (isGrounded)
            {
                StartCoroutine(WaitAfterStomp());
                AudioSource.PlayClipAtPoint(groundHitSound, transform.position);
                Instantiate(groundHitFX, groundCheck.position, Quaternion.identity);
                state = "stompL";
            }

            if (Input.GetButtonDown("Fire3") && canAirBoost && tensionGauge > 20f)
            {
                tensionGauge -= 15f;
                Instantiate(airDashFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(airDashSound, transform.position);
                Debug.Log("boost (jump)");
                airBoostTime = airBoostTimeMax;
                transformVelocity.y = Mathf.Sqrt(jump * -0.75f * gravity);
                canAirBoost = false;
                cloudVFX.Play();
                state = "airboost";
            }
        }
        else if (state == "stompL")
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("DJ");
                isGrounded = false;
                transformVelocity.y = Mathf.Sqrt(jump * -4f * gravity);
                canDoubleJump = false;
                cloudVFX.Play();
                state = "jump";
            }

            if (Input.GetButtonDown("Fire3") && canAirBoost && tensionGauge > 20f)
            {
                tensionGauge -= 15f;
                Instantiate(airDashFX, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(airDashSound, transform.position);
                Debug.Log("boost (jump)");
                airBoostTime = airBoostTimeMax;
                transformVelocity.y = Mathf.Sqrt(jump * -0.75f * gravity);
                canAirBoost = false;
                cloudVFX.Play();
                state = "airboost";
            }
        }
        else if (state == "slide")
        {
            //ground check
            runCloudVFX.Play();

            runTimer -= 1;
            if (runTimer < 0)
            {
                runCloudVFX.Play();
                runTimer = 10;
            }

            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = 0f;
            }



            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");



                characterController.height = 1.15f;
                characterController.center = new Vector3(0f, 0.5f, 0f);

                RaycastHit downHit;

                Vector3 p1 = transform.position + characterController.center;

                //shoots raycast forward to see if theres a raycast hit
                if (Physics.SphereCast(p1, characterController.height, transform.up, out downHit, 1.15f, LayerMask.GetMask("Ground")))
                {
                    Debug.Log("cant get out of slide, keep going");
                }
                else 
                {
                    speed -= 0.025f;
                }

                
                if (speed < 0f)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);

                    

                    if (horizontal != 0 || vertical != 0)
                    {
                        state = "walk";
                    }
                    else
                    {
                        state = "idle";
                    }
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }
                    else
                    {
                        Vector3 moveDir = transform.rotation * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }

                }

                if (Input.GetButtonDown("Fire2"))
                {
                    Debug.Log("crouch");
                    state = "crouch";
                }


                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded && canDodgeRoll)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
            }
        }
        else if (state == "crouch")
        {
            characterController.height = 1.15f;
            characterController.center = new Vector3(0f, 0.5f, 0f);

            //ground check
            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded)
                {
                    canDoubleJump = true;
                }
            }

            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = 0f;
            }

            if (Input.GetButtonUp("Fire2"))
            {
                characterController.height = 1.15f;
                characterController.center = new Vector3(0f, 0.55f, 0f);

                state = "idle";
            }

            if (Input.GetButtonDown("Fire3"))
            {
                Debug.Log("rev");
                revTime = 60;
                state = "rev";
            }

            speed -= decel;
            if (speed < 0f)
            {
                speed = 0;
            }

            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (speed > 0)
                {
                    Vector3 moveDir = transform.rotation * Vector3.forward;
                    characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    


                }

                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);

                    transformVelocity.y = Mathf.Sqrt(jump * -4f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);

                    hanging = false;
                    cloudVFX.Play();
                    transformVelocity.y = Mathf.Sqrt(jump * -4f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "rev")
        {

            revTime += 1f;
            tensionGauge -= decel;

            if(revTime > revTimeMax)
            {
                revTime = revTimeMax;
            }
            //ground check
            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
                if (isGrounded)
                {
                    canDoubleJump = true;
                }
            }

            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = 0f;
            }

            if (Input.GetButtonUp("Fire3") && revTime > 20f || revTime > 20f && tensionGauge <= 0)
            {
                characterController.height = 1.15f;
                characterController.center = new Vector3(0f, 0.55f, 0f);

                if(revTime >= 20f && revTime < 40f)
                {
                    speed = 12;
                }

                if (revTime >= 40f && revTime < 80f)
                {
                    speed = 16;
                }

                if (revTime >= 80f && revTime < 120f)
                {
                    speed = 20;
                }

                if (revTime >= 120f)
                {
                    speed = 24;
                }

                state = "rev run";
            }


            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);

                    transformVelocity.y = Mathf.Sqrt(jump * -4f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    characterController.height = 1.15f;
                    characterController.center = new Vector3(0f, 0.55f, 0f);

                    hanging = false;
                    cloudVFX.Play();
                    transformVelocity.y = Mathf.Sqrt(jump * -4f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "rev run")
        {
            //ground check
            revTime -= 1.15f;


            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = -5f;
            }



            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if(revTime <= 0)
                {
                    if (horizontal != 0 || vertical != 0)
                    {
                        state = "walk";
                    }
                    else
                    {
                        runCloudVFX.Stop();
                        state = "idle";
                    }
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }
                    else
                    {
                        Vector3 moveDir = transform.rotation * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }

                }

                if (Input.GetButtonDown("Fire2"))
                {
                    runCloudVFX.Play();
                    Debug.Log("slide");
                    speed += 2;
                    state = "slide";
                }

                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded && canDodgeRoll)
                {
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    runCloudVFX.Stop();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    hanging = false;
                    cloudVFX.Play();
                    runCloudVFX.Stop();
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "boostring")
        {
            speed = 12;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float airDashSpeed = 24f;

            ringBoostTimer -= 0.68f;
            if ((ringBoostTimer< 0))
            {
                ringBoostTimer = 0;
            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (ringBoostTimer <= 0)
            {

                state = "fall";
            }
            else
            {
                //float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = transform.rotation * Vector3.forward;
                characterController.Move(moveDir.normalized * Time.deltaTime * airDashSpeed);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y += gravity * Time.deltaTime;


        }
        else if (state == "dashpad")
        {
            //ground check
            dashPadTimer -= 1.15f;

            runTimer -= 1;
            if (runTimer < 0)
            {
                runCloudVFX.Play();
                runTimer = 10;
            }

            if (transformVelocity.y < 0)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            if (isGrounded && transformVelocity.y < 0)
            {
                transformVelocity.y = -5f;
            }



            //movement (hanging prevents movement)
            if (!hanging && canDodgeRoll)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                if (dashPadTimer <= 0)
                {
                    if (horizontal != 0 || vertical != 0)
                    {
                        state = "walk";
                    }
                    else
                    {
                        state = "idle";
                    }
                }

                //handles turning
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
                if (canMove)
                {
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }
                    else
                    {
                        Vector3 moveDir = transform.rotation * Vector3.forward;
                        characterController.Move(moveDir.normalized * Time.deltaTime * speed);
                    }

                }

                if (Input.GetButtonDown("Fire2"))
                {
                    runCloudVFX.Play();
                    Debug.Log("slide");
                    speed += 2;
                    state = "slide";
                }

                if (!hanging)
                {
                    transformVelocity.y += gravity * Time.deltaTime;
                }

                if (canDodgeRoll && canMove)
                {
                    characterController.Move(transformVelocity * Time.deltaTime);
                }


                if (Input.GetButtonDown("Jump") && isGrounded && canDodgeRoll)
                {
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    isGrounded = false;
                    cloudVFX.Play();
                    state = "jump";
                }
                else if (Input.GetButtonDown("Jump") && hanging && canDodgeRoll)
                {
                    hanging = false;
                    cloudVFX.Play();
                    transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                    state = "jump";
                }
            }
        }
        else if (state == "hurt")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            speed = 0;

            

            airBoostTime -= 0.68f;
            if ((airBoostTime < 0))
            {
                airBoostTime = 0;
            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 moveDir = transform.rotation * Vector3.forward;
            characterController.Move(-moveDir.normalized * Time.deltaTime * 4);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            characterController.Move(transformVelocity * Time.deltaTime);
            transformVelocity.y += gravity * Time.deltaTime;

            if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack && airBoostTime < 3)
            {
                cloudVFX.Play();
                state = "idle";

            }
            if (horizontal != 0 && isGrounded && airBoostTime < 3 || vertical != 0 && isGrounded && airBoostTime < 3)
            {
                cloudVFX.Play();
                runCloudVFX.Play();
                state = "walk";

            }


        }
        else if (state == "homing")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            speed = 0;
            homingHB.SetActive(true);
            float homingSpeed = 1500f *Time.deltaTime;

            homingTime -= 1f;
            if ((homingTime < 0))
            {
                homingTime = 0;
            }

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            if (homingTime <= 0)
            {
                homing1.canHome = true;
                homingHB.SetActive(false);
                if (horizontal == 0 && vertical == 0 && isGrounded && !isAttacking && canAttack && homingTime < 3)
                {
                    cloudVFX.Play();
                    state = "idle";

                }
                else if (horizontal != 0 && isGrounded && airBoostTime < 3 || vertical != 0 && isGrounded && homingTime < 3)
                {
                    cloudVFX.Play();
                    runCloudVFX.Play();
                    state = "walk";

                }

                else
                {

                    state = "fall";

                }

                
            }
            else
            {
                float dist = Vector3.Distance(homing1.homingTarget.transform.position, transform.position);
                if (homing1.homingTarget != null && dist > 0.3f)
                {
                    Debug.Log("move nigga");

                    print("Distance to other: " + dist);
                    transform.position += (homing1.homingTarget.transform.position - transform.position).normalized * homingSpeed * Time.fixedDeltaTime;
                }
                else
                {
                    Debug.Log("or not");
                    homingTime = 0;
                }
                //float targetAngle = Mathf.Atan2(direction.x, vertical) * Mathf.Rad2Deg + cam.eulerAngles.y;
                //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                //transform.rotation = Quaternion.Euler(0f, angle, 0f);
                //Vector3 moveDir = transform.rotation * Vector3.forward;
                //characterController.Move(moveDir.normalized * Time.deltaTime * airDashSpeed);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            //characterController.Move(transformVelocity * Time.deltaTime);
            //transformVelocity.y += gravity * Time.deltaTime;

            if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("stomp");
                state = "stomp";
            }

            

            


        }
        else if (state == "rail")
        {
            if (Input.GetButtonDown("Jump"))
            {
                PlayerGrind grind = FindAnyObjectByType<PlayerGrind>();
                railFX.Stop();
                grind.railDelay = 10;
                grind.onRail = false;
                grind.currentRailScript = null;
                transformVelocity.y = Mathf.Sqrt(jump * -2f * gravity);
                isGrounded = false;
                cloudVFX.Play();
                runCloudVFX.Stop();
                state = "jump";
            }
        }
        else if(state == "win")
        {
            speed = 0;

            CinemachineFreeLook cine = FindAnyObjectByType<CinemachineFreeLook>();
            cine.m_Lens.FieldOfView = Mathf.Lerp(cine.m_Lens.FieldOfView, 16.64f, 0.2f);

            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
            cam.transform.rotation * Vector3.up);
        }

        tensionGauge = Mathf.MoveTowards(tensionGauge, tensionGaugeMax, 0.035f);

        if(state != "walk" && state != "rev" && state != "rev run" && state != "slide")
        {
            runCloudVFX.Stop();
            runTimer = 10;
        }
        else
        {
            
        }

        if (state != "rev" && state != "rev run")
        {
            revTime = 0;
        }

        if(state != "boostring")
        {
            ringBoostTimer = 0;
        }

        if (state != "dashpad")
        {
            dashPadTimer = 0;
        }

        if(state != "homing")
        {
            homingHB.SetActive(false);
        }

        if (!hanging && canDodgeRoll)
        {




            if (state == "walk" && Input.GetMouseButtonDown(1) && isGrounded && canDash)
            {
                canDash = false;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                characterController.SimpleMove(forward * rollSpeed);
                StartCoroutine(Rolling());
            }
        }

        if(isGrounded)
        {
            canAirBoost = true;
            canDoubleJump = true;
        }

        

        //if (!isGrounded && !isAttacking && transformVelocity.y > 0)
        //{
        //    state = "jump";
        //}



        

        if (isGrounded && isAttacking && canDodgeRoll)
        {
            state = "attack ground";
        }
    }

    void LedgeGrab()
    {
        if (transformVelocity.y < 10 && !hanging && !isGrounded)
        {
            RaycastHit downHit;
            Vector3 lineDownStart = ledgeCheck.position + Vector3.up * 2.5f + ledgeCheck.forward * hangOffset;
            Vector3 lineDownEnd = ledgeCheck.position + Vector3.up * 1.7f + ledgeCheck.forward * hangOffset;
            Physics.Linecast(lineDownStart, lineDownEnd, out downHit, LayerMask.GetMask("Ground"));
            Debug.DrawLine(lineDownStart, lineDownEnd);

            //shoots raycast forward to see if theres a raycast hit
            if (downHit.collider != null)
            {
                RaycastHit fwdHit;
                Vector3 lineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                Vector3 lineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + transform.forward * 4f;
                Physics.Linecast(lineFwdStart, lineFwdEnd, out fwdHit, LayerMask.GetMask("Ground"));
                Debug.DrawLine(lineFwdStart, lineFwdEnd);

                if (fwdHit.collider != null) //shoots a smaller raycast forward to see if its a ledge
                {
                    transformVelocity.y = 0;
                    transformVelocity = Vector3.zero;
                    Debug.Log("ledge hang");
                    hanging = true;
                    state = "hanging";

                    Vector3 hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * -0.1f + transform.up * -1f;
                    hangPos += offset;
                    transform.position = hangPos;
                    transform.forward = -fwdHit.normal;
                }
            }
        }
    }

    private IEnumerator Rolling()
    {
        canDash = false;
        canAttack = false;
        canMove = false;
       // animator.SetBool("Rolling", true);
        Vector3 forward = transform.TransformDirection(Vector3.forward);

        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            characterController.SimpleMove(forward * rollSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1.15f);
       // animator.SetBool("Rolling", false);
        canMove = true;
        canDash = true;
        canAttack = true;

    }

    private IEnumerator WaitAfterStomp()
    {
        yield return new WaitForSeconds(0.25f);
        if(state == "stompL")
        {
            state = "idle";
        }
    }

    private void Animations()
    {
        if (state == "idle")
        {
            ResetAnimations();
            animator.SetBool("Idle", true);
            

        }

        if (state == "walk")
        {
            ResetAnimations();
            animator.SetBool("Walk", true);
            

            if (speed < 7)
            {
                animator.speed = 0.55f;
            }
            else if (speed > 7 && speed < 11)
            {
                animator.speed = 0.75f;
            }
            else
            {
                animator.speed = 1f;
            }

        }
        else
        {
            animator.speed = 1f;
        }

        if (state == "rev run")
        {
            ResetAnimations();
            animator.SetBool("Walk", true);


            if (speed < 8)
            {
                animator.speed = 0.3f;
            }
            else if (speed > 7 && speed < 11)
            {
                animator.speed = 0.65f;
            }
            else
            {
                animator.speed = 1f;
            }

        }

        if (state == "rev")
        {
            ResetAnimations();
            animator.SetBool("Walk", true);
        }

        if (state == "dashpad")
        {
            ResetAnimations();
            animator.SetBool("Walk", true);
        }

        if (state == "attack ground")
        {


        }

        if(state == "airboost")
        {
            ResetAnimations();
            animator.SetBool("AirBoost", true);
            
        }

        if (state == "boostring")
        {
            ResetAnimations();
            animator.SetBool("AirBoost", true);

        }

        if (state == "stomp")
        {
            ResetAnimations();
            animator.SetBool("Stomp", true);
            
        }

        if (state == "stompL")
        {
            ResetAnimations();
            animator.SetBool("StompL", true);
            
        }

        if (state == "jump")
        {

            ResetAnimations();
            animator.SetBool("Jump", true);
            
        }

        if (state == "hurt")
        {

            ResetAnimations();
            animator.SetBool("Hurt", true);

        }


        if (state == "fall")
        {
            ResetAnimations();
            animator.SetBool("Falling", true);
            
        }

        if (state == "slide")
        {
            ResetAnimations();
            animator.SetBool("Slide", true);

        }

        if(state == "rail")
        {
            ResetAnimations();
            animator.SetBool("Rail", true);
        }

        if(state == "homing")
        {
            ResetAnimations();
            animator.SetBool("Homing", true);
        }

        if(state == "crouch")
        {
            ResetAnimations();
            animator.SetBool("Crouch", true);
        }

        if (state == "hanging")
        {
            ResetAnimations();
            animator.SetBool("Hanging", true);
            
        }
        else
        {
            animator.SetBool("Hanging", false);
        }

        if(state == "win")
        {
            ResetAnimations();
            animator.SetBool("GemWin", true);
        }

    }

    private void ResetAnimations()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Jump", false);
        animator.SetBool("AirBoost", false);
        animator.SetBool("Falling", false);
        animator.SetBool("Walk", false);
        animator.SetBool("StompL", false);
        animator.SetBool("Stomp", false);
        animator.SetBool("Hanging", false);
        animator.SetBool("Slide", false);
        animator.SetBool("Crouch", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Homing", false);
        animator.SetBool("Rail", false);
    }
}
