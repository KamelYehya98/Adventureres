using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class InventoryDemoScript : MonoBehaviour
    {
        public InventoryManager inventoryManager;
        public ItemData[] itemsToPickup;

        public void PickupItem(int id)
        {
            inventoryManager.AddItem(itemsToPickup[id]);
        }
    }
}
