namespace Assets.Code.Infrastructure.Input
{
    public interface IGameInput
    {
        event ButtonSignal PlayerAttackSignal;
        event StickSignal PlayerMoveSignal;

        GameInputButtonState GetButtonState(GameInputButton button);
        void UpdateState();
        void SendButtonDownSignals();
        void SendStickEvent(GameInputStick stick);
        void SendAllSignals();
    }
}
