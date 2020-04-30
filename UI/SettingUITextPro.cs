#if I18N_TMP
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

// [ExecuteInEditMode]
public class SettingUITextPro : MonoBehaviour
{
    List<string> keys = new List<string>();
    TextMeshProUGUI[] textList;
    void Start()
    {
        textList = GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var item in textList)
        {
            keys.Add(item.text.Trim());
        }
        MessageBroker.Default.Receive<SystemLanguage>().Subscribe(x => SetText()).AddTo(this);
        SetText();
    }

    void SetText()
    {
        for (int i = 0; i < textList.Length && i < keys.Count; i++)
        {
            if (!string.IsNullOrEmpty(keys[i]) && I18n.ContainKey(keys[i]))
            {
                // textList[i].font = I18n.font;
                textList[i].text = I18n.__(keys[i]);
            }
        }
    }
}
#endif