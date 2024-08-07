using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class InventoryManager : MonoBehaviour
    {
        private int _maxStackCount = 4;
        public InventorySlot[] inventorySlots;
        public GameObject inventoryItemPrefab;

        int selectedSlot = -1;

        public void ChangeSelectedSlot(int newValue)
        {
            if(selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].DeSelect();
            }

            inventorySlots[newValue].Select();
            selectedSlot = newValue;
        }

        private void Update()
        {
            if (Input.inputString != null)
            {
                bool isNumber = int.TryParse(Input.inputString, out int number);
                if (isNumber && number > 0 && number < 8)
                {
                    ChangeSelectedSlot(number - 1);
                }
            }
        }

        public bool AddItem(ItemData item)
        {
            // Stack the item if available and stackable and lower than max count
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < _maxStackCount && itemInSlot.item.stackable)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();

                    return true;
                }
            }

            // Add a new item in an empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }

            return false;
        }

        public void SpawnNewItem(ItemData item, InventorySlot slot)
        {
            GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
            inventoryItem.InitializeItem(item);
        }

        public ItemData GetSelectedItem(bool use)
        {
            InventorySlot slot = inventorySlots[selectedSlot];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if(itemInSlot != null)
            {
                ItemData item = itemInSlot.item;
                if(use)
                {
                    itemInSlot.count--;
                    if(itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                }
                
            }

            return null;
        }


    }
}
