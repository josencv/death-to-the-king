using UnityEngine;
using System.Collections.Generic;
using Assets.Code.Infrastructure.Unity;
using Assets.Code.Infrastructure.Controller;

namespace Assets.Code
{
    public enum GameState { Loading = 0, Running = 1, Pause = 2 };

    public class GameManager : MonoBehaviourExtension
    {
        private IInputManager inputManager;
        private GameObject instance;
        private GameObject player1;
        private GameObject player2;
        private GameObject player3;
        private GameObject player4;

        private List<GameObject> enemies;

        void Awake()
        {
            // Does this need to be changed to use Zenject DI instead of GetComponent?
            inputManager = this.GetComponentInChildren<IInputManager>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}