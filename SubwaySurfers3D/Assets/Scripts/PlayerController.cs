using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform render;
    public Animator animator;

    public float forwardSpeed = 10f;
    public float verticalSpeed = 20f;
    public float laneSwapSpeed = 5f;
    public float laneDistance = 4f;

    public float jumpHeight = 1f;
    public float gravity = -9.81f;

    public float slideHeight = 0.5f;
    public float slideTime = 1f;

    public float hitDistance = 0.1f;
    public LayerMask collisionLayerMask;
    public float speedIncremental = 0.01f;

    // Lane change
    [HideInInspector] public int currentLane = 1;

    private bool _isSliding = false;
    private bool _isAlive = true;
    private float _currentGravity = 0f;
    private Vector3 targetPosition;
    private CharacterController _charCtr;

    private float timeIncrement = 0f;

    // ================= SPEED BOOST =================
    [Header("Speed Boost")]
    public float speedBoostMultiplier = 1.5f;
    private bool speedBoostActive = false;
    // ===============================================

    void Start()
    {
        _charCtr = GetComponent<CharacterController>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MoveLane(-1);

        if (Input.GetKeyDown(KeyCode.D))
            MoveLane(1);

        if (Input.GetKeyDown(KeyCode.Space) && _charCtr.isGrounded)
        {
            _currentGravity = jumpHeight;
            animator.SetBool("Jump", true);
        }

        if (Input.GetKeyDown(KeyCode.S) && !_isSliding)
            StartCoroutine(Slide());

        CheckHealth();
    }

    void FixedUpdate()
    {
        ComputeGravity();

        timeIncrement += Time.fixedDeltaTime * speedIncremental;

        float finalSpeed = (forwardSpeed + timeIncrement) *
                           (speedBoostActive ? speedBoostMultiplier : 1f);

        Vector3 forwardMove = Vector3.forward * finalSpeed * Time.fixedDeltaTime;
        Vector3 verticalMove = Vector3.up * _currentGravity;
        Vector3 horizontalMove = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            laneSwapSpeed * Time.fixedDeltaTime
        );
        horizontalMove = new Vector3(horizontalMove.x - transform.position.x, 0, 0);

        _charCtr.Move(forwardMove + horizontalMove + verticalMove);
    }

    void MoveLane(int direction)
    {
        int newLane = Mathf.Clamp(currentLane + direction, 0, 2);

        if (newLane != currentLane)
        {
            currentLane = newLane;
            float newX = (currentLane - 1) * laneDistance;
            targetPosition = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    void ComputeGravity()
    {
        if (_charCtr.isGrounded && _currentGravity < 0)
        {
            _currentGravity = -0.5f;
            animator.SetBool("Jump", false);
        }

        _currentGravity += gravity * Time.fixedDeltaTime;
    }

    IEnumerator Slide()
    {
        _isSliding = true;

        Vector3 oldCenter = _charCtr.center;
        float oldHeight = _charCtr.height;

        _charCtr.center = Vector3.up * slideHeight;
        _charCtr.height = oldHeight * slideHeight;

        render.localScale = new Vector3(0.7f, 0.4f, 0.7f);
        render.localPosition = Vector3.up * 0.4f;

        yield return new WaitForSeconds(slideTime);

        _charCtr.center = oldCenter;
        _charCtr.height = oldHeight;

        render.localScale = Vector3.one;
        render.localPosition = Vector3.up;

        _isSliding = false;
    }

    void CheckHealth()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position;
        Vector3 p2 = p1 + Vector3.up * _charCtr.height;

        if (Physics.CapsuleCast(p1, p2, _charCtr.radius, transform.forward,
            out hit, hitDistance, collisionLayerMask))
        {
            if (_isAlive)
            {
                _isAlive = false;
                SceneManager.LoadScene("UIGameOverMenu");
            }
        }
    }

    // ================= SPEED BOOST =================

    public void ActivateSpeedBoost(float duration)
    {
        if (!speedBoostActive)
            StartCoroutine(SpeedBoostCoroutine(duration));
    }

    IEnumerator SpeedBoostCoroutine(float duration)
    {
        speedBoostActive = true;
        yield return new WaitForSeconds(duration);
        speedBoostActive = false;
    }

    // ===============================================
}