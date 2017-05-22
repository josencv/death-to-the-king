using UnityEngine;

namespace Assets.Code.Components.Health
{
    public interface IBody : IComponent
    {
        void Hit(float amount, Vector3 direction);
    }
}
