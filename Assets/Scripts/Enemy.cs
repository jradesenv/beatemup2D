﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float maxSpeed;
    public float minHeight;
    public float maxHeight;
    public float minDistanceToPlayer = 1.5f;
    public float minTimeWalk = 1f;
    public float maxTimeWalk = 5f;

    public float damageTime = 0.5f;
    public int maxHealth;
    public float attackRate = 1f;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool facingRight = false;
    private bool isDead = false;
    private static float zForce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer;
    private float nextAttack;
    private float currentWalkTime;

    private Transform target;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<Player>().transform;
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        facingRight = (target.position.x < transform.position.x) ? false : true;
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (damaged && !isDead)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageTime)
            {
                damaged = false;
                damageTimer = 0;
            }
        }

        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            Vector3 targetDistance = target.position - transform.position;
            float xForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            //Debug.Log(currentWalkingTime);
            //Debug.Log(currentWalkTime + " - " + walkTimer);

            if (walkTimer >= currentWalkTime)
            {
                zForce = Random.Range(-1, 2); // -1 baixo, meio ou cima
                Debug.Log(zForce);

                walkTimer = 0;
                currentWalkTime = Random.Range(minTimeWalk, maxTimeWalk);                   
            }

            if (Mathf.Abs(targetDistance.x) < minDistanceToPlayer)
            {
                xForce = 0;
            }

            if (!damaged)
            {
                rb.velocity = new Vector3(xForce * currentSpeed, 0, zForce * currentSpeed);
            }

            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));

            if (Mathf.Abs(targetDistance.x) < minDistanceToPlayer && Mathf.Abs(targetDistance.z) < minDistanceToPlayer && Time.time > nextAttack)
            {
                anim.SetTrigger("Attack");
                nextAttack = Time.time + attackRate;
            }
        }

        rb.position = new Vector3(rb.position.x, rb.position.y, Mathf.Clamp(rb.position.z, minHeight + 1, maxHeight - 1));
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
        if(!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");

            if (currentHealth <= 0)
            {
                isDead = true;
                rb.AddRelativeForce(new Vector3(3,5,0), ForceMode.Impulse);
            }
        }
    }

    public void disableEnemy()
    {
        gameObject.SetActive(false);
    }
}