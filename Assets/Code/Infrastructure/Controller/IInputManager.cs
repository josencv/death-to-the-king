namespace Assets.Code.Infrastructure.Controller
{
    public interface IInputManager
    {
        IGameInput GetGameInput(PlayerInputNumber player);
    }
}
