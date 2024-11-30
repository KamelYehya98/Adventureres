using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class EnemyStatsController : MonoBehaviour
    {
        [field:SerializeField]
        public EnemyData enemyData;
        
        public void InitializeData(EnemyData data)
        {
            enemyData = data;
        }
    }
}
