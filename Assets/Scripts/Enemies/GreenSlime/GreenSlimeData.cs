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
            attackCooldown = 3;
            lastAttackTime = 0;
            moveSpeed = 2f;
            stoppingDistance = 4f;
            detectionRange = 7f;
            stunDuration = 1.5f;
        }
    }
}