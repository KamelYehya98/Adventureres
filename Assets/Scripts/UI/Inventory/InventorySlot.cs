using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class InventorySlot : MonoBehaviour, IDropHandler
    {
        public Image image;
        public Color selectedColor, deselectedColor;

        public void Awake()
        {
            DeSelect();
        }

        public void Select()
        {
            image.color = selectedColor;
        }

        public void DeSelect()
        {
            image.color = deselectedColor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
                inventoryItem.parentAfterDrag = transform;
            }
        }
    }
}
