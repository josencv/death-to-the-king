namespace Assets.Code.Components.Movement
{
    public interface IMovable : IComponent
    {
        void Move(float x, float z);
        void Stop();
    }
}
