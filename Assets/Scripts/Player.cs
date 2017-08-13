using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;

    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        if (Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }
	}

    private void FixedUpdate()
    {
        if(!isDead)
        {
            float h = Input.GetAxis("Horizontal") * currentSpeed; //left/right
            float z = Input.GetAxis("Vertical") * currentSpeed; //up/down
            float y = rb.velocity.y; //jump

            if (!onGround)
            {
                z = 0;
            }

            rb.velocity = new Vector3(h, y, z);

            if (onGround)
            {
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
            }

            if ((h < 0 && facingRight) || (h > 0 && !facingRight))
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth, maxWidth), rb.position.y, Mathf.Clamp(rb.position.z, minHeight + 1, maxHeight - 1));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }
}
