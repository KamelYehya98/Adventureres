using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class ItemsManager : MonoBehaviour
    {
        [SerializeField]
        public List<ItemData> Items;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public ItemData GetItem(string id)
        {
            return Items.First(x  => x.Id == id);
        }
    }
}
