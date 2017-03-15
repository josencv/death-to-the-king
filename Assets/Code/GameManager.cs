using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Infrastructure.Unity;
using Assets.Code.Infrastructure.Controller;
using Assets.Code.Infrastructure.Sound;
using Zenject;

namespace Assets.Code
{
    public enum GameState { Loading = 0, Running = 1, Pause = 2 };

    public class GameManager : MonoBehaviourExtension
    {
        private IInputManager inputManager;
        private ISoundManager soundManager;
        private GameObject instance;
        private GameObject player1;
        private GameObject player2;
        private GameObject player3;
        private GameObject player4;

        private List<GameObject> enemies;

        [Inject]
        private void Inject(IInputManager inputManager, ISoundManager soundManager)
        {
            this.inputManager = inputManager;
            this.soundManager = soundManager;
        }

        void Awake()
        {
           
        }

        void Start()
        {
            soundManager.PlayLoopingSound("Sound/Music/seiken-powell", "background", 1);
        }

        void Update()
        {

        }
    }
}