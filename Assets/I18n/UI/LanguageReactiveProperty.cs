
using UnityEngine;
using UniRx;
using System;

[Serializable]
public class LanguageReactiveProperty : ReactiveProperty<SystemLanguage>
{
    public LanguageReactiveProperty()
    {
    }

    public LanguageReactiveProperty(SystemLanguage initialValue)
        :base(initialValue)
    {
    }
}