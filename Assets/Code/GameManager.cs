using UnityEngine;
using Assets.Code.Infrastructure.Unity;
using Assets.Code.Infrastructure.Input;
using Assets.Code.Infrastructure.Sound;
using Zenject;
using Assets.Code.Components.Controller;
using Assets.Code.Components.Containers;
using Assets.Code.Configuration;

namespace Assets.Code
{
    public enum GameState { Loading = 0, Running = 1, Pause = 2 };

    public class GameManager : MonoBehaviourExtension
    {
        private IInputManager inputManager;
        private ISoundManager soundManager;
        private Factory<Character> characterFactory;

        [SerializeField]
        private CoreInstaller.Settings GameSettings;

        [Inject]
        private void Inject(IInputManager inputManager, ISoundManager soundManager, Factory<Character> characterFactory)
        {
            this.inputManager = inputManager;
            this.soundManager = soundManager;
            this.characterFactory = characterFactory;
        }

        void Awake()
        {
            Character character = characterFactory.Create();
            character.transform.position = new Vector3(0, 0, 0);    // TODO: add a Position property in the Character class
            character.transform.rotation = Quaternion.identity;

            IGameInput input = inputManager.GetGameInput(PlayerInputNumber.Player1);
            character.GetComponent<IEntityController>().Initialize(input);
        }

        void Start()
        {
            soundManager.PlayLoopingSound("Sound/Music/seiken-powell", "background", 1, 0.5f);
        }

        void Update()
        {
            inputManager.ProcessGameInputs();
        }
    }
}