using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PlayerClassManager : MonoBehaviour
    {
        public PlayerClassData activeClass;

        private PlayerClass currentClassComponent;
        private List<PlayerClassData> availableClasses;
        private Dictionary<PlayerClassData, ClassState> classStates = new();

        private void Start()
        {
            availableClasses = new List<PlayerClassData>()
            {
                GameManager.Instance.classManager.warriorData,
                GameManager.Instance.classManager.mageData
            };

            activeClass = availableClasses.Count > 0 ? availableClasses[0] : null;

            InstantiatePlayer(activeClass);
        }

        private void InstantiatePlayer(PlayerClassData classData)
        {
            Debug.Log("Entered InstantiatePlayer at playerClassmanager");

            // Initialize Player specific class script

            if (classData.classType == typeof(MageClass))
            {
                currentClassComponent = gameObject.AddComponent<MageClass>();
                currentClassComponent.className = GameConstants.ClassNames.Mage;
                if (gameObject.TryGetComponent<WarriorClass>(out var warriorClass))
                {
                    Destroy(warriorClass);
                }

            }
            else if (classData.classType == typeof(WarriorClass))
            {
                currentClassComponent = gameObject.AddComponent<WarriorClass>();
                currentClassComponent.className = GameConstants.ClassNames.Warrior;
                if (gameObject.TryGetComponent<MageClass>(out var mageClass))
                {
                    Destroy(mageClass);
                }
            }
        }

        public void SwitchClass(int direction)
        {
            SaveCurrentClassState();

            int classCount = availableClasses.Count;
            int currentIndex = availableClasses.IndexOf(activeClass);

            if (currentIndex == -1)
            {
                Debug.LogError("Active class not found in the list of available classes.");
                return;
            }

            // For easy visuals, Imagine it's a wheel
            // Direction: 1 -> clockwise
            // Direction: -1 -> counter clockwise

            int newIndex;
            if (direction == 1)
            {
                newIndex = (currentIndex + 1) % classCount;
            }
            else if (direction == -1)
            {
                newIndex = (currentIndex - 1 + classCount) % classCount;
            }
            else
            {
                Debug.LogError("Invalid direction for class switch.");
                return;
            }

            activeClass = availableClasses[newIndex];

            ReplaceClassComponent(activeClass);
        }

        private void SaveCurrentClassState()
        {
            if (currentClassComponent != null)
            {
                classStates[activeClass] = currentClassComponent.SaveState();
            }
        }


        private void ReplaceClassComponent(PlayerClassData classData)
        {
            if (currentClassComponent != null)
            {
                Destroy(currentClassComponent);
            }

            if (gameObject.TryGetComponent(out PlayerClass playerClass))
            {
                Destroy(playerClass);
            }

            currentClassComponent = gameObject.AddComponent(classData.classType) as PlayerClass;

            if (currentClassComponent != null)
            {
                currentClassComponent.Initialize(classData);

                if (classStates.ContainsKey(classData))
                {
                    currentClassComponent.LoadState(classStates[classData]);
                }
            }
            else
            {
                Debug.LogError("Failed to add class component.");
            }
        }
    }

    [System.Serializable]
    public class ClassState
    {
        public float vitality;
        public float mana;
        public float criticalStrike;
        public float agility;
        public float attackPower;
        public float defense;
        public float regeneration;
        public string className;
        public Vector3 position;
        public Ability basicAttack;
        public Ability specialAbility;
    }
}
