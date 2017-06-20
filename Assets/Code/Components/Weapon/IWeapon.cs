namespace Assets.Code.Components.Weapon
{
    public delegate void AttackFinishedHandler();

    public interface IWeapon : IComponent
    {
        void Attack();
        void Stop();
        bool IsAttacking { get; }
        float AttackRange { get; }
        float Damage { get; }

        event AttackFinishedHandler AttackFinished;
    }
}
