using UnityEngine;

namespace Assets.Code.Components.Body
{
    public interface IBody : IComponent
    {
        void Hit(float amount, Vector3 direction);
        bool IsDead { get; }
    }
}
