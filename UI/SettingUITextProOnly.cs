#if I18N_TMP
using TMPro;
using UniRx;
using UnityEngine;

// [ExecuteInEditMode]
public class SettingUITextProOnly : MonoBehaviour
{

	TextMeshProUGUI[] textList;
	void Start()
	{
		textList = GetComponentsInChildren<TextMeshProUGUI>(true);
		MessageBroker.Default.Receive<SystemLanguage>().Subscribe(x => SetText()).AddTo(this);
		SetText();
	}

	void SetText()
	{
		if (textList != null)
		{
			for (int i = 0; i < textList.Length; i++)
			{
				textList[i].font = I18n.font;
			}
		}
	}
}
#endif