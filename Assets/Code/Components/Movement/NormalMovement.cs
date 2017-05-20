﻿using Assets.Code.Components.Controller;
using Assets.Code.Components.Health;
using Assets.Code.Components.Weapon;
using Assets.Code.Constants;
using Assets.Code.Infrastructure.Unity;
using UnityEngine;
using Zenject;

namespace Assets.Code.Components.Movement
{
    /// <summary>
    /// This component enables normal movement for an entity
    /// </summary>
    public class NormalMovement : MonoBehaviourExtension, IMovable
    {
        private float speed;

        private IWeapon weapon;
        private Rigidbody body;
        private Animator animator;
        private Vector3 movement;   // Stores the movement value to be applied in the next physics iteration

        [Inject]
        private void Inject(Rigidbody body, Animator animator, IWeapon weapon)
        {
            this.body = body;
            this.animator = animator;
            this.weapon = weapon;
        }

        public void Awake()
        {
            movement = Vector3.zero;
            speed = 3;
        }

        public void Move(float x, float z)
        {
            movement = Vector3.ClampMagnitude(new Vector3(x, 0, z), 1);
        }

        private void ApplyMovement()
        {
            if (!CanMove())
            {
                body.velocity = Vector3.zero;
                return;
            }

            body.velocity = movement * speed;
            if (movement != Vector3.zero)
            {
                animator.SetBool(AnimatorParameters.IsWalking, true);
                transform.rotation = Quaternion.LookRotation(movement);
            }
            else
            {
                animator.SetBool(AnimatorParameters.IsWalking, false);
            }
        }

        private bool CanMove()
        {
            return weapon == null || !weapon.IsAttacking;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }
    }
}
