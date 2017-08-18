using System;
using HugsLib;

public class FNotAgainSettings : HugsLib.ModBase
{ 
    private SettingHandle<bool> isCrashlanding;

    public override string ModIdentifier
    {
        get { return "FNotAgainSetting"; }
    }

    public override void DefsLoaded()
    {
        isCrashlanding = Settings.GetHandle<bool>("isCrashlanding", "FNotAgain.toggleSetting_title".Translate(), "FNotAgain.toggleSetting_desc".Translate(), true);
    }
}