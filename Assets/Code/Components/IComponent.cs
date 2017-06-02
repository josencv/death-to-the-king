using UnityEngine;

namespace Assets.Code.Components
{
    public interface IComponent
    {
        string tag { get; }
        Transform transform { get; }

        T GetComponent<T>();
    }
}
