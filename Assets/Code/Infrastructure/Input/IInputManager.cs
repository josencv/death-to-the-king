namespace Assets.Code.Infrastructure.Input
{
    public interface IInputManager
    {
        IGameInput GetGameInput(PlayerInputNumber player);
        void ProcessGameInputs();
    }
}
