using Assets.Scripts.Classes;
using Assets.Scripts.Data;
using Assets.Scripts.Player;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.UI.Inventory;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class InventoryManager : MonoBehaviour, IDataPersistence
    {
        [SerializeField]
        public ItemsManager itemManager;

        private int _maxStackCount = 4;
        public InventorySlot[] inventorySlots;
        public GameObject inventoryItemPrefab;
        public CharacterStateManager stateManager;
        private PlayerController PlayerController;

        int selectedSlot = -1;

        private void Awake()
        {
            PlayerController = GetComponent<PlayerController>();
            stateManager = GetComponent<CharacterStateManager>();
        }

        public void ChangeSelectedSlot(int newValue)
        {
            if(selectedSlot >= 0)
            {
                inventorySlots[selectedSlot].DeSelect();
            }

            inventorySlots[newValue].Select();
            selectedSlot = newValue;

            InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item != null)
            {
                Debug.Log("Item in slot type: " + itemInSlot.item.type.ToString());

                if (itemInSlot.item.type == ItemType.Sword)
                {
                    Debug.Log("Equiped item is sword");

                    PlayerController.weaponComponent.SetWeapon(itemInSlot.item);
                    stateManager.meleeStateMachine.SetNextStateToMain();
                }
            }


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

        public ItemType GetCurrentItemType()
        {
            return selectedSlot == -1 || inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() is null
                ? ItemType.None
                : inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item.type;
        }

        public void LoadData(GameData gameData)
        {
            Debug.Log("Called Load for inventoryyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");

            if(gameData.PlayerData.Inventory != null)
            {
                Debug.Log("Inventory not nulllllllllllllllllllllllllllll");

                foreach (var  item in gameData.PlayerData.Inventory)
                {
                    ItemData itemData = itemManager.GetItem(item.Key.Id);

                    InventorySlot slot = inventorySlots[item.Key.Index];

                    SpawnNewItem(itemData, slot);

                    InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

                    if(inventoryItem != null)
                    {
                        inventoryItem.count = item.Value;
                    }
                }
            }
        }

        public void SaveData(GameData gameData)
        {
            gameData.PlayerData.Inventory = new();

            if(inventorySlots != null )
            {
                for(int i = 0; i<inventorySlots.Length; i++)
                {
                    InventorySlot slot = inventorySlots[i];
                    InventoryItem inventoryItem = slot.GetComponentInChildren<InventoryItem>();

                    if(inventoryItem != null && inventoryItem.item != null)
                    {
                        InventoryItemData inventoryItemData = new(inventoryItem.item.Id, i);
                        Debug.Log("Item Id: " + inventoryItemData.Id + " --- Item Index: " + inventoryItemData.Index);
                        gameData.PlayerData.Inventory.Add(inventoryItemData, inventoryItem.count);
                    }
                }
            }
        }
    }
}
