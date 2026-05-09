namespace TheAdventure.Interfaces;

// All damageable objects have a life span and a die function
public interface IDamageable
{
    int HP { get; set; }

    void Die();
}