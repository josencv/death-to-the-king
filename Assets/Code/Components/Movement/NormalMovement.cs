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
        private Rigidbody2D body;

        [Inject]
        private void Inject(Rigidbody2D body)
        {
            this.body = body;
        }

        public void Awake()
        {
            speed = 3;
        }

        public void Move(float x, float y)
        {
            // TODO: set a max vector length
            Vector2 direction = new Vector2(x, y);
            body.velocity = direction * speed;
        }
    }
}
