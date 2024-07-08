using Assets.Scripts.Classes;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ClassManager : MonoBehaviour
    {
        public PlayerClassData mageData;
        public PlayerClassData warriorData;

        public static ClassManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            mageData.classType = typeof(MageClass);
            warriorData.classType = typeof(WarriorClass);
        }
    }
}