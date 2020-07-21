#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class KeyboardKeyWindow : EditorWindow
{
	List<KeyboardKey> connections = new List<KeyboardKey>();

	GameObject key = null;

    private void OnGUI() {
		if (key == null) {
			key = StaticKeyReference.CurrentKey.gameObject;
			foreach (KeyboardKey adjKey in StaticKeyReference.CurrentKey.adjacentKeys) {
				connections.Add(adjKey);
			}
		}
		GUILayout.Label("Current Key", EditorStyles.boldLabel);
		EditorGUILayout.ObjectField(key, typeof(GameObject), true);

		if (GUILayout.Button("Add Connection")) {
			connections.Add(new KeyboardKey());
		}
		
		GUILayout.Label("Connected Keys", EditorStyles.boldLabel);
		for (int i = 0; i < connections.Count; i++) {
			if (connections[i] != null && (connections[i]).GetComponent<KeyboardKey>() == null) {
				Debug.LogError("Invalid GameObject at index " + i);
				connections[i] = null;
			}
			connections[i] = EditorGUILayout.ObjectField(connections[i], typeof(KeyboardKey), true) as KeyboardKey;
		}


		if (GUILayout.Button("CONFIRM NEW CONNECTIONS")) {

			List<KeyboardKey> connectedKeys = new List<KeyboardKey>();

			for (int i = 0; i < connections.Count; i++) {
				if (connections[i] == null) {
					continue;
				}

				KeyboardKey keyToBeAdded = connections[i];
				if (!connectedKeys.Contains(keyToBeAdded)) {
					connectedKeys.Add(keyToBeAdded);
				}
			}

			StaticKeyReference.CurrentKey.adjacentKeys = new List<KeyboardKey>();

			foreach (KeyboardKey key in connectedKeys) {
				key.AddIfNotExisting(StaticKeyReference.CurrentKey);
				StaticKeyReference.CurrentKey.adjacentKeys.Add(key);
			}

			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

			this.Close();
		}
	}
}
#endif