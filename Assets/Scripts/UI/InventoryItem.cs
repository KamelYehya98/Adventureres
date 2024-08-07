using Assets.Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("UI")]
        public Image image;
        public Text countText;

        [HideInInspector] public int count = 1;
        [HideInInspector] public ItemData item;
        [HideInInspector] public Transform parentAfterDrag;

        public void InitializeItem(ItemData newItem)
        {
            image.sprite = newItem.image;
            item = newItem;
            RefreshCount();
        }

        public void Start()
        {
            InitializeItem(item);
        }

        public void RefreshCount()
        {
            bool textActive = count > 1;
            countText.gameObject.SetActive(textActive);
            countText.text = count.ToString();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
    }
}
