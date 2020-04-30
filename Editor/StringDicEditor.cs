using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

[CustomEditor(typeof(StringDic))]
public class StringDicEditor : Editor
{
    private StringDic dic;
    void Awake()
    {
        dic = (StringDic)target;
    }
    public override void OnInspectorGUI()
    {
        // if (GUILayout.Button("SaveToJson"))
        // {
        //     dic.SaveToJSON(dic.name);
        // }
        if (GUILayout.Button("Load form Json"))
        {
            Debug.Log(I18n.Instance.jsonFolder);
            dic.LoadFromPersistentJson(I18n.Instance.jsonFolder);
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
        if (dic.name == "en" && GUILayout.Button("Scan select UI"))
        {
            PrintText();
        }

        DrawDefaultInspector();
    }

    void PrintText()
    {
        var texts = Selection.gameObjects[0].GetComponentsInChildren<Text>(true);
        StringBuilder sb = new StringBuilder();
        foreach (var item in texts)
        {
            sb.Append(item.text);
            sb.Append("\n");
        }
        Debug.Log(sb.ToString());
    }
}