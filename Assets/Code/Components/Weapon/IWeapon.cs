﻿namespace Assets.Code.Components.Weapon
{
    public interface IWeapon
    {
        void Attack();
        bool IsAttacking { get; }
    }
}
