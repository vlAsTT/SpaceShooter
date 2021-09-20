namespace Core.Interfaces
{
    public interface ISpaceshipCombat
    {
        float Damage { get; set; }

        void OnHit(float damage);
    }
}