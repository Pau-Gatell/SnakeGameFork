using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening; //DOTween
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float acceleration = 10f;
    public float jumpForce = 12f;
    public float friction = 1f;

    [Header("Agua")]
    public float waterGravityScale = 0.3f;
    public float waterDrag = 2f;
    private bool _isInWater = false;
    private float _normalGravityScale;
    private float _normalDrag;

    [Header("Raycast suelo")]
    public float rayLength = 1f;
    public LayerMask rayMask;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity = Vector2.zero;
    private float _input;
    private bool _grounded;

    private int _health = 5;
    private int _checkpoints = 0;
    private int _maxCheckpoints = 0;

    //Variables de mida
    private bool _isChangingSize = false;
    private Vector3 _originalScale;

    //Speed Boost
    private bool _speedBoostActive = false;
    private float _normalSpeed;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _normalGravityScale = _rigidbody.gravityScale;
        _normalDrag = _rigidbody.linearDamping;

        //Guardem la mida original del jugador
        _originalScale = transform.localScale;

        //Checkpoints
        Checkpoint[] checkponts = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        _maxCheckpoints = checkponts.Length;
        _checkpoints = _maxCheckpoints;
    }

    void Update()
    {
        _input = UnityEngine.Input.GetAxisRaw("Horizontal");

        _grounded = Physics2D.Raycast(transform.position, Vector2.down, rayLength, rayMask);

        if (UnityEngine.Input.GetButtonDown("Jump"))
        {
            if (_grounded || _isInWater)
            {
                float adjustedJump = _isInWater ? jumpForce * 0.6f : jumpForce;
                _rigidbody.AddForce(Vector2.up * adjustedJump, ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.linearVelocity;

        if (_input != 0)
        {
            float targetSpeed = _input * speed;
            if (_isInWater)
                targetSpeed *= 0.5f;

            _velocity.x = Mathf.MoveTowards(_velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            float currentFriction = _isInWater ? friction * 0.1f : friction;
            _velocity.x = Mathf.MoveTowards(_velocity.x, 0, currentFriction * Time.fixedDeltaTime);
        }

        _rigidbody.linearVelocity = new Vector2(_velocity.x, _rigidbody.linearVelocity.y);

        if (_isInWater)
        {
            _rigidbody.gravityScale = waterGravityScale;
            _rigidbody.linearDamping = waterDrag;

            if (_rigidbody.linearVelocity.y < 0)
                _rigidbody.AddForce(Vector2.up * 1.5f, ForceMode2D.Force);
        }
        else
        {
            _rigidbody.gravityScale = _normalGravityScale;
            _rigidbody.linearDamping = _normalDrag;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            EnterWater();
        }

        //Collectable que fa petit
        if (collision.CompareTag("CollectableSmall") && !_isChangingSize)
        {
            StartCoroutine(ChangeSizeTemporarily(0.5f));
        }

        //Collectable que fa gran
        if (collision.CompareTag("CollectableBig") && !_isChangingSize)
        {
            StartCoroutine(ChangeSizeTemporarily(1.5f));
        }

        //Speed Boost
        if (collision.CompareTag("SpeedUp"))
        {
            StartCoroutine(SpeedBoost());
        }
    }

    // Corrutina Speed Boost
    private IEnumerator SpeedBoost()
    {
        if (_speedBoostActive)
            yield break;

        _speedBoostActive = true;

        _normalSpeed = speed;
        speed = speed * 1.8f;

       
        //Espera 5 segons
        yield return new WaitForSeconds(5f);

        speed = _normalSpeed;
        _speedBoostActive = false;

      
    }

    private IEnumerator ChangeSizeTemporarily(float scaleMultiplier)
    {
        _isChangingSize = true;

        Vector3 targetScale = _originalScale * scaleMultiplier;

        transform.DOScale(targetScale, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(6f);

        transform.DOScale(_originalScale, 0.5f).SetEase(Ease.OutBack);

        _isChangingSize = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            ExitWater();
        }
    }

    private void EnterWater()
    {
        if (_isInWater) return;

        _isInWater = true;
        _rigidbody.gravityScale = waterGravityScale;
        _rigidbody.linearDamping = waterDrag;
    }

    private void ExitWater()
    {
        if (!_isInWater) return;

        _isInWater = false;
        _rigidbody.gravityScale = _normalGravityScale;
        _rigidbody.linearDamping = _normalDrag;
    }

    public void Respawn()
    {
        if (Checkpoint.current != null)
        {
            transform.position = Checkpoint.current.transform.position;
            Time.timeScale = 1f;
            _rigidbody.linearVelocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("GameOverMenu");
        }
    }
}

