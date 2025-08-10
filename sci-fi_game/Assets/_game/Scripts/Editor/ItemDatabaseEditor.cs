using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Game.Item.Data;
using Game.DataManagement;

//Adapted from an SO database script from an older project
[CustomEditor(typeof(ItemDatabase))]
public class ItemDatabaseEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if(GUILayout.Button("Populate Database")) {
            ItemDatabase db = (ItemDatabase)target;

            string[] guids = AssetDatabase.FindAssets("t:ItemData");

            var guidsProperty = serializedObject.FindProperty("_guids");
            var itemsProperty = serializedObject.FindProperty("_items");

            guidsProperty.ClearArray();
            itemsProperty.ClearArray();

            for(int i = 0; i < guids.Length; i++) {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);

                guidsProperty.InsertArrayElementAtIndex(i);
                guidsProperty.GetArrayElementAtIndex(i).stringValue = guids[i];

                itemsProperty.InsertArrayElementAtIndex(i);
                itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue = item;
            }

            serializedObject.ApplyModifiedProperties();

            Debug.Log($"Item Database populated with {guids.Length} items.");
        }
    }
}