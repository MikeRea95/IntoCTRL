public interface IEnemyCombat
{
    string Name();
    bool PlayerAttacks();
    void EnemyAttacks();
    void Die();
    void DieWithoutPoints();
    void AddKeyReference(KeyboardKey key);
    void Hide();
    void Show();
}