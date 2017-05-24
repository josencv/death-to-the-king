using UnityEngine;
using Zenject;

namespace Assets.Code.Configuration
{
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        [SerializeField]
        private CoreInstaller.Settings GameInstaller;

        public override void InstallBindings()
        {
            Container.BindInstance(GameInstaller);
        }
    }
}
