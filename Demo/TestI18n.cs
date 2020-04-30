using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestI18n : MonoBehaviour
{
    public I18n i18n;
    public Text formatText;

    void Awake()
    {
        i18n.Init();
    }

    void Start()
    {
        formatText.text = I18n.__("accuracyTask", 1 , 2, 0.3);
    }



}
