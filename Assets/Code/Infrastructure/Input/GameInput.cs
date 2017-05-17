using System.Collections.Generic;

namespace Assets.Code.Infrastructure.Input
{
    /// <summary>
    /// This class represents the player game input. It is an abstraction from the actual hardware input used.
    /// </summary>
    public abstract class GameInput : IGameInput
    {
        protected InputType type;                                                       // Keyboard or Xbox
        protected PlayerInputNumber playerInputNumber;                                  // Player input number to differenciate from other player inputs
        protected GameInputContext currentContext;                                      // The current context in which the input is in
        protected Dictionary<GameInputButton, GameInputButtonState> currentButtonState; // Saves the current state of the GameInput buttons
        protected Dictionary<GameInputButton, float> holdTime;                          // Stores the time elapsed since every button was pressed
        protected Dictionary<GameInputStick, float[]> currentStickState;                // Stores the state of the GameInput sticks (composed axis)

        // List of all game events that can be triggered by the game input
        public event ButtonSignal PlayerAttackSignal;
        public event StickSignal PlayerMoveSignal;

        /// <summary>
        /// GameInput constructor
        /// </summary>
        /// <param name="type">Type of the hardaware input source</param>
        /// <param name="playerInputNumber">The number of the player to be assigned to the input</param>
        public GameInput(InputType type, PlayerInputNumber playerInputNumber)
        {
            this.type = type;
            this.playerInputNumber = playerInputNumber;
            this.currentContext = GameInputContext.InGame;  // To be changed to MainMenu or something like that
            currentButtonState = new Dictionary<GameInputButton, GameInputButtonState>();
            currentStickState = new Dictionary<GameInputStick, float[]>();
            holdTime = new Dictionary<GameInputButton, float>();

            InitializeControllerState();
        }

        /// <summary>
        /// Initializes the state for first time use
        /// </summary>
        private void InitializeControllerState()
        {
            currentButtonState.Add(GameInputButton.A, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.B, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.Y, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.X, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.L1, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.L2, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.R1, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.R2, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.Start, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.Back, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.LeftStick, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.RightStick, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.DPadUp, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.DPadLeft, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.DPadDown, GameInputButtonState.None);
            currentButtonState.Add(GameInputButton.DPadRight, GameInputButtonState.None);
            currentStickState.Add(GameInputStick.Left, new float[2]);
            currentStickState.Add(GameInputStick.Right, new float[2]);
        }

        /// <summary>
        /// Updates the GameInput button and axis states. To be implemented by each child class.
        /// </summary>
        public abstract void UpdateState();

        /// <summary>
        /// Gets the state of the button passed. Posible states are defined in the
        /// GameInputButton enum.
        /// </summary>
        /// <param name="button">GameInput button type to be checked</param>
        /// <returns></returns>
        public GameInputButtonState GetButtonState(GameInputButton button)
        {
            return currentButtonState[button];
        }

        public void SendButtonDownSignals()
        {
            if (currentContext == GameInputContext.InGame)
            {
                if (currentButtonState[GameInputButton.A] == GameInputButtonState.Down)
                {
                    TriggerEvent(PlayerAttackSignal);
                }
            }
        }

        // TODO: Consider moving the game logic to a game controller or similar.
        /// <summary>
        /// Sends the specified stick signal (X and Y axis values) to the subscribers, depending on the passed GameInputContext.
        /// </summary>
        /// <param name="stick">Type of stick to check</param>
        public void SendStickEvent(GameInputStick stick)
        {
            if (currentContext == GameInputContext.InGame)
            {
                if (stick == GameInputStick.Left)
                {
                    TriggerEvent(PlayerMoveSignal, currentStickState[GameInputStick.Left][0], currentStickState[GameInputStick.Left][1]);
                }

                if (stick == GameInputStick.Right)
                {
                    // Send some signal
                }
            }
        }

        /// <summary>
        /// Sends all signals to the different game components, depending on the current GameInput context
        /// </summary>
        public void SendAllSignals()
        {
            SendButtonDownSignals();
            SendStickEvent(GameInputStick.Left);
            SendStickEvent(GameInputStick.Right);
        }

        #region Private methods

        /// <summary>
        /// Triggers a button signal event
        /// </summary>
        /// <param name="signal"></param>
        private void TriggerEvent(ButtonSignal signal)
        {
            if (signal != null)
            {
                signal.Invoke();
            }
        }

        private void TriggerEvent(StickSignal signal, float x, float y)
        {
            if (signal != null)
            {
                signal.Invoke(x, y);
            }
        }

        #endregion

        // Properties ==========================================================

        public PlayerInputNumber PlayerInputNumber { get { return playerInputNumber; } }

    }

}