using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(I18n))]
public class I18nEditor : Editor
{
    private I18n i18n;

    void Awake()
    {
        i18n = (I18n)target;
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Load all Language"))
        {
            var path = AssetDatabase.GetAssetPath(target);
            string absolutePath = Application.dataPath.Replace("Assets", path);
            FileInfo fileInfo = new FileInfo(absolutePath);
			var absoluteDir = fileInfo.Directory.FullName;
			var dir = absoluteDir.Replace(Application.dataPath, "Assets");

            string[] searchFolder = { dir };
            string[] assetPaths = AssetDatabase.FindAssets("t:ScriptableObject", searchFolder);
            List<StringDic> tempList = new List<StringDic>();
            for (int i = 0; i < assetPaths.Length; i++)
            {
                string assetPath = assetPaths[i];
                StringDic preb = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetPath), typeof(StringDic)) as StringDic;
                if (preb != null)
                {
                    tempList.Add(preb);
                }
            }

            i18n.languageArray = tempList.ToArray();
			i18n.LoadJsonOnEditor();
            EditorUtility.SetDirty(target);
        }

        DrawDefaultInspector();
    }
}
