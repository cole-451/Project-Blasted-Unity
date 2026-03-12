using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Character2D, IDamagable
{
    [SerializeField] float maxHealth = 100.0f;
    [SerializeField] Slider healthSlider;

    [SerializeField] GameObject player;

    [SerializeField] GameObject leftDamager;
    [SerializeField] GameObject rightDamager;

    private bool stunned = false;


    public void OnDamage(float damage)
    {

        health -= damage;
        Mathf.Clamp(health, 0f, maxHealth);
        if (health <= 0f)
        {
            //Die
            Destroy(gameObject); //temp for now, will switch to the dead sprite later.
        }
        stunned = true;
        Debug.Log($"ow! i am at {health} health!");
    }

    protected override void Awake()
    {
        health = maxHealth;


    }

    private void OnEnable()
    {
        leftDamager.SetActive(false);
        rightDamager.SetActive(false);
    }

    private void Update()
    {
        healthSlider.value = health / maxHealth;
    }

    protected override void FixedUpdate()
    {
        var distance = transform.position.x - player.transform.position.x; // maybe something i can do with this?
        movement.x = 1.0f * speed;
        animator.SetFloat("Speed", Mathf.Abs(movement.x));
        if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Right : eFace.Left;
        if(distance < 0)
        {
            // move left torwards player
        }
        else
        {
            //move right torwards player
        }

            base.FixedUpdate();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void OnAttack()
    {

    }

    public void OnAttackHitBox()
    {
        if (facing == eFace.Left)
        {
            leftDamager.SetActive(true);
        }
        if (facing == eFace.Right)
        {
            rightDamager.SetActive(true);
        }
    }



}
