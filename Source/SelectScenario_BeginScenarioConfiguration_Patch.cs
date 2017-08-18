using System;
using Hugslib;

[HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
public static class SelectScenario_BeginScenarioConfiguration_Patch
{
    [HarmonyPostfix]
    public static void Crashland()
    {
        if (Settings.GetHandle<bool>("isCrashlanding"))
            Current.Game.InitData.startingPawns = 
    }
}