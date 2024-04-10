using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceSettingWindow : EditorWindow
{
    [MenuItem("Tools/Utility")]
    private static void OpenWindow()
    {
        ResourceSettingWindow window = GetWindow<ResourceSettingWindow>("Game Utility");
        window.minSize = new Vector2(700, 500);
        window.Show();
    }
}
