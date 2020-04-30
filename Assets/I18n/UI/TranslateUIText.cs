using System.Collections.Generic;
#if I18N_TMP
using TMPro;
#endif
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TranslateUIText : MonoBehaviour
{
    public Text[] textParts;
#if I18N_TMP
	public TextMeshProUGUI[] textProParts;
#endif
    List<string> keys = new List<string>();
    List<string> prokeys = new List<string>();

    void Start()
    {
        foreach (var item in textParts)
        {
            keys.Add(item.text.Trim());
        }
#if I18N_TMP
		foreach (var item in textProParts)
		{
			prokeys.Add(item.text.Trim());
		}
#endif
        MessageBroker.Default.Receive<SystemLanguage>().Subscribe(x => SetText()).AddTo(this);
        SetText();
    }

    void SetText()
    {
        for (int i = 0; i < textParts.Length && i < keys.Count; i++)
        {
            textParts[i].text = I18n.__(keys[i]);
        }
#if I18N_TMP
        for (int i = 0; i < textProParts.Length && i < prokeys.Count; i++)
        {
            textProParts[i].font = I18n.font;
            textProParts[i].text = I18n.__(prokeys[i]);
        }
#endif
    }
}