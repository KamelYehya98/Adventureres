using Assets.Scripts.Player;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Inventory
{
    public class CraftingUI : MonoBehaviour
    {
        public CraftingSystem craftingSystem;
        public GameObject recipeSlotPrefab;
        public Transform recipeSlotContainer;

        public CraftingRecipeData[] recipes;

        private void Start()
        {
            RefreshCraftingUI();
        }

        public void RefreshCraftingUI()
        {
            foreach (Transform child in recipeSlotContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var recipe in recipes)
            {
                GameObject recipeSlot = Instantiate(recipeSlotPrefab, recipeSlotContainer);
                //recipeSlot.GetComponentInChildren<Text>().text = recipe.result.itemName;
                recipeSlot.GetComponentInChildren<Button>().onClick.AddListener(() => Craft(recipe));
            }
        }

        public void Craft(CraftingRecipeData recipe)
        {
            if (craftingSystem.CraftItem(recipe))
            {
                RefreshCraftingUI();
            }
        }
    }
}