using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntitySettings : EditorWindow
{
    public static bool showMovement = true;
    string status = "Select a GameObject";

    [MenuItem("MyExample/Test")]
    static void Init()
    {
        EntitySettings window = (EntitySettings)GetWindow(typeof(EntitySettings));
        window.Show();
    }

    public void OnGUI()
    {
        showMovement = EditorGUILayout.Foldout(showMovement, status);
        if (showMovement)
        {
            if (Selection.activeTransform)
            {
                Selection.activeTransform.position = EditorGUILayout.Vector3Field("Position", Selection.activeTransform.position);
                status = Selection.activeTransform.name;
            }

            if (!Selection.activeTransform)
            {
                status = "Select a GameObject";
                showMovement = false;
            }
        }
    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }
}