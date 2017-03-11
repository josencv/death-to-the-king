namespace Assets.Code.Infrastructure.Controller
{
    public interface IGameInput
    {
        GameInputButtonState GetButtonState(GameInputButton button);
        void UpdateState();
        void SendButtonDownSignals();
        void SendStickEvent(GameInputStick stick);
        void SendAllSignals();
    }
}
