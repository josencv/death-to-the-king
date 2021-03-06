﻿using Assets.Code.Infrastructure.Unity;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Body
{
    public class Body : MonoBehaviourExtension, IBody
    {
        private const float knockbackBaseSpeed = 12.0f;
        private const float knockbackDuration = 0.09f;
        private readonly Color hitColor = new Color(1, 0.3f, 0.3f, 1);

        private Animator animator;
        private Rigidbody body;

        [SerializeField]
        private int maxHealth = 100;
        [SerializeField]
        private float mass = 1;

        private bool isDead;
        private bool isHit;
        private float currentHealth;
        private float currentKnockbackTime;


        [Inject]
        private void Inject(Animator animator, Rigidbody body)
        {
            this.animator = animator;
            this.body = body;
        }

        private void Awake()
        {
            isDead = false;
            isHit = false;
            currentHealth = maxHealth;
            currentKnockbackTime = 0;

            if (mass <= 0)
            {
                throw new ArgumentException("The value must be positive", "mass");
            }
        }
         
        public void Hit(float amount, Vector3 direction)
        {
            if (!isHit && !isDead)
            {
                isHit = true;
                TakeDamage(amount);
                GetComponentInChildren<Renderer>().material.color = hitColor;
                actions["Kockback"] = () => ApplyKnockback(direction);
            }
        }

        private void TakeDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void ApplyKnockback(Vector3 direction)
        {
            currentKnockbackTime = 0;
            body.velocity = direction.normalized * knockbackBaseSpeed / mass;
        }

        private void CheckKnockback(float deltaTime)
        {
            if (isHit)  // For now "isHit" will be equivalent of "inKnockback"
            {
                currentKnockbackTime += deltaTime;
                if (currentKnockbackTime >= knockbackDuration)
                {
                    isHit = false;
                    GetComponentInChildren<Renderer>().material.color = Color.white;
                    body.velocity = Vector3.zero;
                }
            }
        }

        private void Die()
        {
            isDead = true;
            Destroy(gameObject);
        }

        private void Update()
        {
        }

        private void FixedUpdate()
        {
            ExecuteActions();
            CheckKnockback(Time.deltaTime);
        }

        public bool IsDead { get { return isDead; } }
    }
}
