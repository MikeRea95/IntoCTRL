#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KeyboardKey))]
public class KeyboardKeyEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Connections")) {
            StaticKeyReference.CurrentKey = target as KeyboardKey;
            EditorWindow.GetWindow(typeof(KeyboardKeyWindow));
        }
    }
}
#endif