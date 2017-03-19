using Assets.Code.Infrastructure.Input;

namespace Assets.Code.Components.Controller
{
    public interface IEntityController
    {
        void Initialize(IGameInput gameInput);
    }
}
