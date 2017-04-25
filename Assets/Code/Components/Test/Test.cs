using UnityEngine;

namespace Assets.Code.Components.Test
{
    class Test : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}
