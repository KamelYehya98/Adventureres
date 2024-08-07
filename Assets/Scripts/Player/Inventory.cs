using Assets.Scripts.Scriptable_Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class Inventory : MonoBehaviour
    {
        public List<ItemData> items = new List<ItemData>();
        public Dictionary<ItemData, int> itemCounts = new Dictionary<ItemData, int>();
        public delegate void OnInventoryChanged();
        public OnInventoryChanged onInventoryChangedCallback;

        public void AddItem(ItemData item, int count = 1)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item] += count;
            }
            else
            {
                items.Add(item);
                itemCounts[item] = count;
            }
            onInventoryChangedCallback?.Invoke();
        }

        public bool RemoveItem(ItemData item, int count = 1)
        {
            if (itemCounts.ContainsKey(item) && itemCounts[item] >= count)
            {
                itemCounts[item] -= count;
                if (itemCounts[item] == 0)
                {
                    items.Remove(item);
                    itemCounts.Remove(item);
                }
                onInventoryChangedCallback?.Invoke();
                return true;
            }
            return false;
        }

        public bool HasItem(ItemData item, int count = 1)
        {
            return itemCounts.ContainsKey(item) && itemCounts[item] >= count;
        }
    }

}