using Assets.Scripts.Scriptable_Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class CraftingSystem : MonoBehaviour
    {
        public Inventory inventory;

        public bool CraftItem(CraftingRecipeData recipe)
        {
            // Check if inventory has all ingredients
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                if (!inventory.HasItem(recipe.ingredients[i], recipe.ingredientCounts[i]))
                {
                    Debug.Log("Not enough ingredients");
                    return false;
                }
            }

            // Remove ingredients from inventory
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                inventory.RemoveItem(recipe.ingredients[i], recipe.ingredientCounts[i]);
            }

            // Add crafted item to inventory
            inventory.AddItem(recipe.result);
            return true;
        }
    }
}