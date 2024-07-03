using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayerUI : UIBase
{
    private bool playing = false;
    private SettingUI _settingButton;
    private void Start()
    {
        _settingButton = GetComponentInChildren<SettingUI>();
        _settingButton.Open();
    }
}
