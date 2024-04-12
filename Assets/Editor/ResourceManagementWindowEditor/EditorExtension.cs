using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EditorExtension : MonoBehaviour
    {
        public static T CreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
        
            T newResourceManagementWindowSaveData = ScriptableObject.CreateInstance<T>();
            T newSO = newResourceManagementWindowSaveData;
            AssetDatabase.CreateAsset(newResourceManagementWindowSaveData, uniquePath); 
        
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newSO;
        }
    }
}