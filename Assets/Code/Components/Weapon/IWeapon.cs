namespace Assets.Code.Components.Weapon
{
    public interface IWeapon
    {
        void Attack();
        void Stop();
        bool IsAttacking { get; }
    }
}
