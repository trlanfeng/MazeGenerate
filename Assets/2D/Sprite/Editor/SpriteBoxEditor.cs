using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SpriteBox))]
public class SpriteBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SpriteBox sbox = (SpriteBox)target;
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("ResizeToSet"))
        {
            sbox.resizeToSet();
        }
        if (GUILayout.Button("ResizeToOriginal"))
        {
            sbox.resizeToOriginal();
        }
        EditorGUILayout.EndHorizontal();
    }
}
