using UnityEngine;
using Assets.Code.Infrastructure.Unity;
using Assets.Code.Infrastructure.Input;
using Assets.Code.Infrastructure.Sound;
using Zenject;
using Assets.Code.Components.Controller;
using Assets.Code.Components.Containers;
using Assets.Code.Configuration;
using Assets.Code.Shared;

namespace Assets.Code
{
    public enum GameState { Loading = 0, Running = 1, Pause = 2 };

    public class GameManager : MonoBehaviourExtension
    {
        private IInputManager inputManager;
        private ISoundManager soundManager;
        private Factory<Character> characterFactory;
        private WorldData worldData;

        [SerializeField]
        private CoreInstaller.Settings GameSettings;

        [Inject]
        private void Inject(IInputManager inputManager, ISoundManager soundManager, Factory<Character> characterFactory, WorldData worldData)
        {
            this.inputManager = inputManager;
            this.soundManager = soundManager;
            this.characterFactory = characterFactory;
            this.worldData = worldData;
        }

        private void Awake()
        {
            Character[] characters = new Character[4];
            characters[0] = characterFactory.Create();
            characters[0].transform.position = new Vector3(0, 0, 0);
            characters[0].transform.rotation = Quaternion.identity;
            characters[0].GetComponent<IEntityController>().Initialize(inputManager.GetGameInput(PlayerInputNumber.Player1));
            worldData.AddPlayer(characters[0]);

            characters[1] = characterFactory.Create();
            characters[1].transform.position = new Vector3(-1, 0, -1);
            characters[1].transform.rotation = Quaternion.identity;
            characters[1].GetComponent<IEntityController>().Initialize(inputManager.GetGameInput(PlayerInputNumber.Player2));

            Camera camera = Camera.main;
            GameCamera gameCamera = camera.GetComponent<GameCamera>();
            gameCamera.target = characters[1].gameObject;
            worldData.AddPlayer(characters[1]);

        }

        private void Start()
        {
            soundManager.PlayLoopingSound("Sound/Music/seiken-powell", "background", 1, 0.5f);
        }

        private void Update()
        {
            inputManager.ProcessGameInputs();
        }
    }
}