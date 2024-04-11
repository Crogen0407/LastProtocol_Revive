using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;

public class ResourceManagementWindow : EditorWindow
{
    [MenuItem ("Tools/ResourceManagement")]
    public static void  ShowWindow () {
        var window = EditorWindow.GetWindow(typeof(ResourceManagementWindow));
        window.Show();
        window.minSize = new Vector2(600f, 360f);
        string path = $"Assets/Editor/ResourceManagementWindowEditor/Resources/ResourceManagementWindowSaveDataSO.asset";
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
        
        //데이터 로드
        _resourceManagementWindowSaveDataSO = 
            AssetDatabase.LoadAssetAtPath<ResourceManagementWindowSaveDataSO>(path) as ResourceManagementWindowSaveDataSO;
        
        if (_resourceManagementWindowSaveDataSO == null) //데이터를 찾지 못했다면
        {
            var newResourceManagementWindowSaveData = ScriptableObject.CreateInstance<ResourceManagementWindowSaveDataSO>();
            _resourceManagementWindowSaveDataSO = newResourceManagementWindowSaveData;
            AssetDatabase.CreateAsset(newResourceManagementWindowSaveData, uniquePath); 
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Vector2 materialListScroll = Vector2.zero;
    private static ResourceManagementWindowSaveDataSO _resourceManagementWindowSaveDataSO;
    
    private void OnGUI()
    {
        if (_resourceManagementWindowSaveDataSO.CurrentResourceListSO == null)
        {
            GUI.color = Color.red;
            GUILayout.Label("NO RESOURCELISTSO IN PATH!");
            GUI.color = Color.white;
            return;
        }
        
        #region Init

        Rect settingSaveAndReturnAreaRect = new Rect(0, 0, position.size.x, 90f);
        Rect mainSettingAreaRect = new Rect(
            0, 
            settingSaveAndReturnAreaRect.height, 
              settingSaveAndReturnAreaRect.width, 
        position.height - settingSaveAndReturnAreaRect.height);


        #endregion
        
        GUILayout.BeginArea(settingSaveAndReturnAreaRect, new GUIStyle("helpBox"));
        {
            //window 저장 SO가 없으면 아예 그리지 말것!
            if (_resourceManagementWindowSaveDataSO == null) return;

            //ResourceListSO에 할당하기
            _resourceManagementWindowSaveDataSO.CurrentResourceListSO = 
                EditorGUILayout.ObjectField(
                    _resourceManagementWindowSaveDataSO.CurrentResourceListSO, 
                    typeof(ResourceListSO)) as ResourceListSO;

            if (_resourceManagementWindowSaveDataSO.CurrentResourceListSO != null)
            {
                GUILayout.Space(10);
                GUI.color = Color.green;
                if (GUILayout.Button("Save Setting"))
                {
                
                }
                GUILayout.Space(10);
                GUI.color = Color.red;
                if (GUILayout.Button("Return Setting"))
                {
                
                }
                GUI.color = Color.white;
            }
        }
        GUILayout.EndArea();
        
        GUILayout.BeginArea(mainSettingAreaRect, new GUIStyle("helpBox"));
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginScrollView(materialListScroll, false, true);
                {
                    
                }
                GUILayout.EndScrollView();    
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
}
