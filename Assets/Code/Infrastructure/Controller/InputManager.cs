using Assets.Code.Infrastructure.Unity;
using Zenject;

namespace Assets.Code.Infrastructure.Controller {

    /// <summary>
    /// Input manager service that manages all input received and maps
    /// to the correct action, depending on the game context or state.
    /// </summary>
    public class InputManager : MonoBehaviourExtension, IInputManager {

        private IGameInput[] gameInputs;
        private MonoInstaller coreInstaller;    // DI Container

        private void InitializeGameInputs() 
        {
            gameInputs = new IGameInput[4];

            IGameInput gameInput = new KeyboardGameInput(PlayerInputNumber.Player1);
            gameInputs[0] = gameInput;
        }

        private void ProcessGameInputs() 
        {
            foreach (GameInput gameInput in gameInputs)
            {
                if (gameInput != null)
                {
                    gameInput.UpdateState();
                    gameInput.SendAllSignals();
                }
            }
        }

        void Awake() {
            InitializeGameInputs();
        }

	    protected void Start () {
            coreInstaller = GetComponent<MonoInstaller>();
        }

        protected void Update () {
            ProcessGameInputs();
	    }

        public IGameInput GetGameInput(PlayerInputNumber inputNumber)
        {
            return gameInputs[(int)inputNumber];
        }
    }

}