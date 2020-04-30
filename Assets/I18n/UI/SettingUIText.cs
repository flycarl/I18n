using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class SettingUIText : MonoBehaviour {
	List<string> keys = new List<string>();
	Text[] textList;
	void Start()
	{
		textList = GetComponentsInChildren<Text>(true);
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
				textList[i].text = I18n.__(keys[i]);	
			}
		}
	}
}
