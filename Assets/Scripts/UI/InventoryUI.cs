using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public Inventory inventory;
        public GameObject itemSlotPrefab;
        public Transform itemSlotContainer;

        private void Start()
        {
            if (inventory == null)
            {
                inventory = GetComponentInParent<Inventory>();
            }

            inventory.onInventoryChangedCallback += RefreshInventoryUI;
            RefreshInventoryUI();
        }

        private void OnDestroy()
        {
            if (inventory != null)
            {
                inventory.onInventoryChangedCallback -= RefreshInventoryUI;
            }
        }

        public void RefreshInventoryUI()
        {
            // Clear existing slots
            foreach (Transform child in itemSlotContainer)
            {
                Destroy(child.gameObject);
            }

            // Instantiate new slots
            foreach (var item in inventory.items)
            {
                GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotContainer);
                //itemSlot.GetComponentInChildren<Text>().text = item.itemName + " x" + inventory.itemCounts[item];
                //itemSlot.GetComponentInChildren<Image>().sprite = item.itemIcon;
            }
        }
    }


}
