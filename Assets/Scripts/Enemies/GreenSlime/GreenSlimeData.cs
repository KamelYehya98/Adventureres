namespace Assets.Scripts.Enemies.GreenSlime
{
    public class GreenSlimeData : EnemyData
    {
        public GreenSlimeData() 
        {
            enemyName = "Green Slime";
            health = 100;
            attackPower = 5;
            attackRange = 7;
            attackCooldown = 4;
            lastAttackTime = 0;
            moveSpeed = 1.5f;
            stoppingDistance = 4f;
        }
    }
}