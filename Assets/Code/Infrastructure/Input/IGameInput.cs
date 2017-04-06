namespace Assets.Code.Infrastructure.Input
{
    public interface IGameInput
    {
        event ButtonSignal PlayerAttackSignal;
        event ButtonSignal PlayerToggleWeaponSignal;
        event StickSignal PlayerMoveSignal;

        GameInputButtonState GetButtonState(GameInputButton button);
        void UpdateState();
        void SendButtonDownSignals();
        void SendStickEvent(GameInputStick stick);
        void SendAllSignals();
    }
}
