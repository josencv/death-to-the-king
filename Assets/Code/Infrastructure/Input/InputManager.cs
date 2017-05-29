namespace Assets.Code.Infrastructure.Input {

    /// <summary>
    /// Input manager service that manages all input received and maps
    /// to the correct action, depending on the game context or state.
    /// </summary>
    public class InputManager : IInputManager {

        private IGameInput[] gameInputs;

        public InputManager()
        {
            gameInputs = new IGameInput[4];
            gameInputs[0] = new XboxGameInput(PlayerInputNumber.Player1, XboxCtrlrInput.XboxController.First);   // TODO: resolve with DI
            gameInputs[1] = new KeyboardGameInput(PlayerInputNumber.Player2);   // TODO: resolve with DI
        }

        public void ProcessGameInputs() 
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

        public IGameInput GetGameInput(PlayerInputNumber inputNumber)
        {
            return gameInputs[(int)inputNumber];
        }
    }

}