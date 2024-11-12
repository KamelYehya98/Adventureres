namespace Assets.Scripts.Enemies.GreenSlime
{
    public class GreenSlimeData : EnemyData
    {
        public GreenSlimeData() 
        {
            enemyName = "Green Slime";
            health = 50;
            attackPower = 5;
            attackRange = 7;
            attackCooldown = 4;
            lastAttackTime = 0;
            moveSpeed = 2;
            stoppingDistance = 0.5f;
        }
    }
}