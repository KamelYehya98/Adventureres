using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "CraftingRecipeData", menuName = "ScriptableObjects/CraftingRecipeData")]
    public class CraftingRecipeData : ScriptableObject
    {
        public ItemData result;
        public ItemData[] ingredients;
        public int[] ingredientCounts;
    }
}
