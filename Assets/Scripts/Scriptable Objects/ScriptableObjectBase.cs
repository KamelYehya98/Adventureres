﻿using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects
{

    public class ScriptableObjectIdAttribute : PropertyAttribute { }

    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
    public class ScriptableObjectIdDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = Guid.NewGuid().ToString();
            }
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    #endif

    public class ScriptableObjectBase : ScriptableObject
    {
        [ScriptableObjectId]
        public string Id;
    }
}
