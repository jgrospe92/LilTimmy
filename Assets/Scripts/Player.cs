using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    public PostProcessVolume volume;
    private ColorGrading colorGrading;


    private float timerCountDown = 6f;
    public static bool isNight;

    private int orbCounter;

    private void Awake()
    {
        volume = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out colorGrading);

        orbCounter = 0;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        Flip();

        if (timerCountDown > 0)
        {
            timerCountDown -= Time.deltaTime;
        }
        else
        {
            timerCountDown = 0;
        }

        nightCycle();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void nightCycle()
    {
     
        if (timerCountDown == 0)
        {
            isNight = true;
            colorGrading.enabled.value = true;
            colorGrading.saturation.value = -100f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Orb"))
        {
            orbCounter++;
            print("collected " + orbCounter);
            Destroy(collision.gameObject);
        }
    }

}
