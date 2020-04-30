using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if ARABIC
using ArabicSupport;
#endif
#if I18N_TMP
using TMPro;
#endif
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "I18n", menuName = "Data/Language/I18n", order = 4)]
public class I18n : ScriptableObject
{
    public string jsonFolder
    {
        get
        {
            string jsonPath = Path.Combine(Application.dataPath, @"I18n/ExcelData/Json");
            string fullPath = Path.GetFullPath(jsonPath);
            return fullPath;
        }
    }
    public StringDic[] languageArray;
    public StringDic defaultStringDic;
#if I18N_TMP
    public static TMP_FontAsset font { get; set; }
	public TMP_FontAsset defaultFont;
#endif
    public LanguageReactiveProperty currentLanguage = new LanguageReactiveProperty();
    Dictionary<string, string> config = new Dictionary<string, string>();

    public void Init()
    {
        LoadJsonOnEditor();
        currentLanguage.Value = GetInitLocale();
        currentLanguage.Subscribe(l => SetLanguage(l));
    }

    public void LoadJsonOnEditor()
    {
#if UNITY_EDITOR
        foreach (var item in languageArray)
        {
            item.LoadFromPersistentJson(jsonFolder);
            UnityEditor.EditorUtility.SetDirty(item);
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
    }

    SystemLanguage GetInitLocale()
    {
        var result = Application.systemLanguage;
        if (PlayerPrefs.HasKey("SaveLanguage"))
        {
            string saveValue = PlayerPrefs.GetString("SaveLanguage");
            result = (SystemLanguage)System.Enum.Parse(typeof(SystemLanguage), saveValue);
        }
        return result;
    }

    void SetLanguage(SystemLanguage locale)
    {
        InitConfig(locale);
        PlayerPrefs.SetString("SaveLanguage", locale.ToString());
        MessageBroker.Default.Publish<SystemLanguage>(locale);
        Debug.Log("SetLanguage " + locale);
    }

    private static I18n _instance;

    public static I18n Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<I18n>().FirstOrDefault();
#if UNITY_EDITOR
            if (!_instance)
                InitializeFromDefault(UnityEditor.AssetDatabase.LoadAssetAtPath<I18n>("Assets/I18n/Cfg/I18n.asset"));
#endif
            return _instance;
        }
    }

    public static void InitializeFromDefault(I18n settings)
    {
        if (_instance) DestroyImmediate(_instance);
        _instance = Instantiate(settings);
        _instance.hideFlags = HideFlags.HideAndDontSave;
    }

    public void InitConfig(SystemLanguage locale)
    {
        var strDic = GetStringDic(locale);
        ParseStringDic(strDic);
    }

    public StringDic GetStringDic(SystemLanguage locale)
    {
        foreach (var item in languageArray)
        {
            if (item.name == LocalToName(locale))
            {
                return item;
            }
        }
        return defaultStringDic;
    }

    string LocalToName(SystemLanguage local)
    {
        string result = "en";
        switch (local)
        {
            case SystemLanguage.Chinese:
                result = "zh";
                break;
            case SystemLanguage.English:
                result = "en";
                break;
            case SystemLanguage.French:
                result = "fr";
                break;
            case SystemLanguage.German:
                result = "de";
                break;
            case SystemLanguage.Japanese:
                result = "jp";
                break;
            case SystemLanguage.Korean:
                result = "ko";
                break;
            case SystemLanguage.Russian:
                result = "ru";
                break;
            case SystemLanguage.ChineseSimplified:
                result = "zh";
                break;
            case SystemLanguage.ChineseTraditional:
                result = "zh-tr";
                break;
            case SystemLanguage.Arabic:
                result = "ar";
                break;
            case SystemLanguage.Spanish:
            case SystemLanguage.Catalan:
                result = "es";
                break;
            case SystemLanguage.Portuguese:
                result = "pt";
                break;
            case SystemLanguage.Italian:
                result = "it";
                break;
            case SystemLanguage.Dutch:
                result = "du";
                break;
            case SystemLanguage.Thai:
                result = "th";
                break;
            case SystemLanguage.Unknown:
                result = "en";
                break;
        }
        return result;
    }

    void ParseStringDic(StringDic strDic)
    {
        config.Clear();
        foreach (var item in strDic.words)
        {
            config[item.key] = item.word.Trim();
        }

#if I18N_TMP
        SetTMPFont(strDic);
#endif
    }

#if I18N_TMP
    void SetTMPFont(StringDic strDic)
    {
        if (strDic.font != null)
        {
            font = strDic.font;
        }
        else
        {
            font = defaultFont;
        }
    }
#endif

    public string LanguageName(StringDic strDic)
    {
        if (strDic.words[0].key == "languageName")
        {
            return strDic.words[0].word;
        }
        foreach (var item in strDic.words)
        {
            if (item.key == "languageName")
            {
                return item.word;
            }
        }
        return "English";
    }

    public static bool ContainKey(string key)
    {
        return Instance.ContainKeyOf(key);
    }

    bool ContainKeyOf(string key)
    {
        if (config == null || config.Count == 0)
        {
            return false;
        }
        else
        {
            return config.ContainsKey(key);
        }
    }

    public static string __(string key, params object[] args)
    {
        return Instance.___(key, args);
    }

    string ___(string key, params object[] args)
    {
        if (config == null || config.Count == 0)
        {
            Debug.Log("__ config null or 0");
            return key;
        }
        string translation = key;
        if (config.ContainsKey(key))
        {

            translation = config[key];

            // check if we have embeddable data
            if (args.Length > 0)
            {
                translation = string.Format(translation, args);
            }
        }
        // else
        // {
        // 	Debug.Log("Missing translation for:" + key);
        // }
#if ARABIC
		if (Application.systemLanguage == SystemLanguage.Arabic)
		{
			translation = ArabicFixer.Fix(translation);
		}
#endif
        return translation;
    }
}