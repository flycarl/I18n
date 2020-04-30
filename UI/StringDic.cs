using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if I18N_TMP
using TMPro;
#endif
using System.IO;

[System.Serializable]
public class StringPair
{
    public string key;
    public string word;
}

[CreateAssetMenu(fileName = "StringDic", menuName = "Data/Language/StringDic", order = 4)]
public class StringDic : ScriptableObject
{
#if I18N_TMP
    public TMP_FontAsset font;
#endif
    public StringPair[] words;

    public void LoadFromPersistentJson(string folderPath)
    {
        string path = Path.Combine(folderPath, this.name + ".json");
        if (System.IO.File.Exists(path))
        {
            LoadFromJSON(path);
        }
        else
        {
            Debug.Log("can't find " + path);
        }
    }

    void LoadFromJSON(string path)
    {
        string textData = System.IO.File.ReadAllText(path);
        string textDataClosure = "{\"words\" : " + textData + " }";
        JsonUtility.FromJsonOverwrite(textDataClosure, this);
    }

    public void SaveToJSON(string folderPath, string name)
    {
        string path = Path.Combine(folderPath, this.name + ".json");
        Debug.LogFormat("Saving config to {0}", path);
        System.IO.FileInfo file = new System.IO.FileInfo(path);
        file.Directory.Create();
        System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
    }

    public string Get(string key)
    {
        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word.key == key)
            {
                return word.word;
            }
        }
        return "";
    }
}
