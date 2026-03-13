using System;
using System.Collections;
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
            animator.SetTrigger("Death");
            Destroy(gameObject, 5f); //temp for now, will switch to the dead sprite later.
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
        if (stunned)
        {
            transform.position = gameObject.transform.position;
            StartCoroutine(StunCR());
        }
        if (Vector2.Distance(transform.position, player.transform.position) > 3f && !stunned)
        {
            //walk to player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        if(Vector2.Distance(transform.position, player.transform.position) <= 3f &&!stunned)
        {
            StartCoroutine(WaitToPunchCR());

            OnAttack();
        }
    }

    protected override void FixedUpdate()
    {
        movement.x = 1.0f * speed;
        animator.SetFloat("Speed", Mathf.Abs(movement.x));
        if (Mathf.Abs(movement.x) > 0.1f) facing = (movement.x > 0) ? eFace.Left : eFace.Right;
            base.FixedUpdate();
    }


    public void OnAttack()
    {
        if (stunned) return;
        animator.SetTrigger("Punch");
        OnAttackHitBox();
        StartCoroutine(PunchCooldownCR());
    }

    IEnumerator PunchCooldownCR()
    {
        yield return new WaitForSeconds(3.0f);
        animator.ResetTrigger("Punch");

    }

    IEnumerator StunCR()
    {
        yield return new WaitForSeconds(3f);
        animator.ResetTrigger("Hit");



    }
    IEnumerator WaitToPunchCR()
    {
        yield return new WaitForSeconds(1.5f);
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
