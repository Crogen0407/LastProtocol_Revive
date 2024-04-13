using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Editor;
using UnityEngine;
using UnityEditor;

public class ResourceManagementWindow : EditorWindow
{
    private static string _path = $"Assets/Editor/ResourceManagementWindowEditor/Resources";
    private static string _resourceListSOPath;
    private static string _newEnumName = "";
    private static Vector2 materialListScroll = Vector2.zero;
    private static ResourceManagementWindowSaveDataSO _resourceManagementWindowSaveDataSO;
    private ResourceListSO _resourceListSO;
    private ResourceSO _currentSelectedResourceSO;
    private Rect viewRect;
    [MenuItem ("Tools/ResourceManagement")]
    public static void  ShowWindow () {
        var window = EditorWindow.GetWindow(typeof(ResourceManagementWindow));
        window.Show();
        window.minSize = new Vector2(600f, 360f);
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(_path);
        
        //데이터 로드

        #region Path

        _resourceManagementWindowSaveDataSO = 
            AssetDatabase.LoadAssetAtPath<ResourceManagementWindowSaveDataSO>($"{_path}/ResourceManagementWindowSaveDataSO.asset");
        _resourceListSOPath = AssetDatabase.GetAssetPath(_resourceManagementWindowSaveDataSO.CurrentResourceListSO);
        _resourceListSOPath = _resourceListSOPath.Replace("/ResourceList.asset", "/ResourceSO.asset");

        #endregion
        
        if (_resourceManagementWindowSaveDataSO == null) //데이터를 찾지 못했다면
        {
            _resourceManagementWindowSaveDataSO = EditorExtension.CreateScriptableObject<ResourceManagementWindowSaveDataSO>($"{_path}/ResourceManagementWindowSaveDataSO.asset");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    private void OnGUI()
    {
        #region Init

        Rect settingSaveAndReturnAreaRect = new Rect(0, 0, position.size.x, 120f);
        
        Rect mainSettingAreaRect = new Rect(
            0, 
            settingSaveAndReturnAreaRect.height, 
              settingSaveAndReturnAreaRect.width/2, 
        position.height - settingSaveAndReturnAreaRect.height);
        
        Rect mainSettingAreaRectSecond = new Rect()
        {
            size = mainSettingAreaRect.size,
            position = new Vector2(mainSettingAreaRect.width, mainSettingAreaRect.y)
        };
        
        GUIStyle elementFont = new GUIStyle()
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            normal = new GUIStyleState()
            {
                textColor = Color.white
            }
        };
        #endregion
        
        GUILayout.BeginArea(settingSaveAndReturnAreaRect, new GUIStyle("helpBox"));
        {
            //window 저장 SO가 없으면 아예 그리지 말것!
            if (_resourceManagementWindowSaveDataSO == null)
            {
                GUILayout.EndArea();
                return;
            }

            //ResourceListSO에 할당하기
            _resourceListSO = 
                EditorGUILayout.ObjectField(
                    _resourceManagementWindowSaveDataSO.CurrentResourceListSO, 
                    typeof(ResourceListSO)) as ResourceListSO;
            
            _resourceManagementWindowSaveDataSO.CurrentResourceListSO = _resourceListSO;
            
            if (_resourceListSO != null)
            {
                GUILayout.Space(10);
                GUI.color = Color.green; //Save
                {
                    if (GUILayout.Button("SO Rename"))
                    {
                        EditorUtility.SetDirty(_resourceManagementWindowSaveDataSO.CurrentResourceListSO);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        for (int i = 0; i < _resourceListSO.resourceList.Length; ++i)
                        {
                            string path = AssetDatabase.GetAssetPath(_resourceListSO.resourceList[i].GetInstanceID());
                            EditorUtility.SetDirty(_resourceListSO.resourceList[i]);
                            AssetDatabase.RenameAsset(path, _resourceListSO.resourceList[i].resource.ToString());
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                        }
                    }    
                }
                GUILayout.Space(10);
                GUI.color = Color.blue; //Sort
                {
                    if (GUILayout.Button("Sort List"))
                    {
                        SortElementByEnum(_resourceListSO.resourceList);
                    }    
                }
                GUI.color = Color.white;
                // GUILayout.BeginHorizontal();
                // {
                //     _newEnumName = GUILayout.TextArea(_newEnumName, 30);
                //     if (GUILayout.Button("Add Enum", GUILayout.Width(100f)))
                //     {
                //
                //     }    
                // }
                // GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndArea();
        
        GUILayout.BeginArea(mainSettingAreaRect, new GUIStyle("helpBox"));
        {
            if (_resourceManagementWindowSaveDataSO.CurrentResourceListSO == null)
            {
                GUI.color = Color.red;
                {
                    GUILayout.Label("NO RESOURCELISTSO IN PATH!");
                }
                GUI.color = Color.white;
                return;
            }
            
            GUILayout.BeginHorizontal();
            {
                viewRect = new Rect(0, 0, 0, 55f * (_resourceListSO.resourceList.Length + 1));
                materialListScroll = GUI.BeginScrollView(new Rect(0,0,mainSettingAreaRect.width, mainSettingAreaRect.height), materialListScroll, viewRect, false, true);
                {
                    Rect btnRect = new Rect(0, 0, mainSettingAreaRect.width-60f, 50f);
                    for (int i = 0; i < _resourceListSO.resourceList.Length; ++i)
                    {
                        btnRect.position = new Vector2(0f, 55f * i);
                        DrawElementButton(_resourceListSO.resourceList[i], btnRect, mainSettingAreaRect, elementFont);
                    }

                    btnRect.y = 55f * _resourceListSO.resourceList.Length;
                    btnRect.width = mainSettingAreaRect.width - 25f;
                    btnRect.height = 25f;
                    GUI.color = Color.yellow;
                    if (GUI.Button(btnRect, "Add"))
                    {
                        _resourceListSO.resourceList = ArrayLastPointAddNewElement(_resourceListSO.resourceList);    
                    }

                    GUI.color = Color.white;
                }
                GUI.EndScrollView();    
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
        
        GUILayout.BeginArea(mainSettingAreaRectSecond, new GUIStyle("helpBox"));
        {
            if (_currentSelectedResourceSO != null)
            {
                SerializedObject o = new SerializedObject(_currentSelectedResourceSO);

                //Enum
                _currentSelectedResourceSO.resource = 
                    (Resource)EditorGUILayout.EnumPopup(_currentSelectedResourceSO.resource);
                
                //Sprite
                _currentSelectedResourceSO.sprite =
                    EditorGUILayout.ObjectField(_currentSelectedResourceSO.sprite, typeof(Sprite)) as Sprite;
                
                //RecipeArray
                SerializedProperty property = o.FindProperty("recipe");
                EditorGUILayout.PropertyField(property, true);
                
                //makingTime
                if (_currentSelectedResourceSO.recipe != null && _currentSelectedResourceSO.recipe .Length != 0)
                {
                    _currentSelectedResourceSO.makingTime =
                        EditorGUILayout.FloatField("Making Time", _currentSelectedResourceSO.makingTime);    
                }
                else
                {
                    _currentSelectedResourceSO.makingTime = -1f;
                }
                
                o.ApplyModifiedProperties();
                o.Update();
                AssetDatabase.SaveAssets();
                
                //DeleteBtn
                GUI.color = Color.red;
                {
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteElementInArray(_currentSelectedResourceSO);
                    }    
                }
                GUI.color = Color.white;
            }
        }
        GUILayout.EndArea();
        
        
    }

    private void DrawElementButton(ResourceSO resource, Rect btnRect, Rect areaRect, GUIStyle style)
    {
        GUILayout.BeginHorizontal();
        {
            if(_currentSelectedResourceSO == resource)
                GUI.color = Color.cyan;
            
            if (GUI.Button(btnRect, "", GUI.skin.button))
            {
                _currentSelectedResourceSO = resource;
            }

            if (resource.sprite != null)
            {
                Rect textureRect = new Rect(btnRect.x + 10f, btnRect.y + 10f, 30f, 30f);
                Texture2D texture = resource.sprite.texture;
                GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit, true);
            }

            GUI.color = Color.red;
            {
                Rect deleteBtnRect = new Rect(btnRect.x + areaRect.width - 50f, btnRect.y + 15f, 20f, 20f);
                if (GUI.Button(deleteBtnRect, EditorGUIUtility.IconContent("CrossIcon")))
                {
                    DeleteElementInArray(resource);
                }    
            }
            GUI.color = Color.white;
            
            btnRect.x += 100f;
            btnRect.width = 1000f;
            GUI.Label(btnRect, resource.resource.ToString());

            if(_currentSelectedResourceSO == resource)
                GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();
    }
    
    private ResourceSO[] ArrayLastPointAddNewElement(ResourceSO[] array)
    {
        ResourceSO[] newArray = new ResourceSO[array.Length + 1];

        for (int i = 0; i < array.Length; ++i)
        {
            newArray[i] = array[i];
        }

        newArray[newArray.Length - 1] = EditorExtension.CreateScriptableObject<ResourceSO>($"{_resourceListSOPath}");

        return newArray;
    }

    private void SortElementByEnum(ResourceSO[] array)
    {
        Array.Sort(array, (num1, num2) => num1.resource > num2.resource ? 1 : -1);  // 위의 함수와 같은 기능

        Debug.Log("정렬완료");
    }
    
    private void DeleteElementInArray(ResourceSO targetElement)
    {
        List<ResourceSO> list = _resourceListSO.resourceList.ToList();
        list.Remove(targetElement);

        _resourceListSO.resourceList = list.ToArray();
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(targetElement));
        EditorUtility.SetDirty(_resourceListSO);
        AssetDatabase.SaveAssets();
    }
}
