using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;
    public int maxHealth = 10;
    public string playerName;
    public Sprite playerImage;

    public int currentHealth;
    public float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    public bool onGround;
    public bool isDead = false;
    public bool facingRight = true;
    public bool jump = false;
    private UIManager uiManager;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
        uiManager = FindObjectOfType<UIManager>();
    }
	
	// Update is called once per frame
	void Update () {
        //onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        onGround = true;

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

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth, maxWidth), rb.position.y, Mathf.Clamp(rb.position.z, minHeight + 0.01f, maxHeight - 0.01f));

            if ((h < 0 && facingRight) || (h > 0 && !facingRight))
            {
                Flip();
            }

            if (jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }

            adjustScale();
        }
    }

    void adjustScale()
    {
        Vector3 scale = transform.localScale;
        if (rb.position.z < -7)
        {
            scale.x = 0.290f;
            scale.y = 0.290f;
        }
        else if (rb.position.z < -6.5)
        {
            scale.x = 0.285f;
            scale.y = 0.285f;
        }
        else if (rb.position.z < -6)
        {
            scale.x = 0.280f;
            scale.y = 0.280f;
        }
        else if (rb.position.z < -5.5)
        {
            scale.x = 0.275f;
            scale.y = 0.275f;
        }
        else if (rb.position.z < -5)
        {
            scale.x = 0.270f;
            scale.y = 0.270f;
        }
        else if (rb.position.z < -4.5)
        {
            scale.x = 0.265f;
            scale.y = 0.265f;
        }
        else if (rb.position.z < -4)
        {
            scale.x = 0.260f;
            scale.y = 0.260f;
        }
        else if (rb.position.z < -3.5)
        {
            scale.x = 0.255f;
            scale.y = 0.255f;
        }
        else
        {
            scale.x = 0.250f;
            scale.y = 0.250f;
        }

        if (transform.localScale.x < 0)
        {
            scale.x *= -1;
        }

        transform.localScale = scale;
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

    public void TookDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");

            uiManager.UpdateHealth(currentHealth);
        }
    }
}
