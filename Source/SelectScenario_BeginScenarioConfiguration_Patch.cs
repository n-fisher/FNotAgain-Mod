using System.Reflection;
using Hugslib;
using Verse;
using Harmony;
using UnityEngine;


namespace FNotAgainMod {

    [StaticConstructorOnStartup]
    public static class SelectScenario_BeginScenarioConfiguration_Patch
    {
        static SelectScenario_BeginScenarioConfiguration_Patch()
        {
            var harmony = HarmonyInstance.Create(FNotAgainMod.Identifier);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
    class Crashland
    {
        [HarmonyPostfix]
        public static void Crashland()
        {
            var pawns = Settings.GetHandle<CrashlandingHandle>("savedPawns");
            if (Settings.GetHandle<bool>("isCrashlanding") && pawns != null)
            {
                Current.Game.InitData.startingPawns = pawns;
            }
        }
    }

    [HarmonyPatch(typeof(ShipCountdown), "CountdownEnded")]
    class SaveSurvivors {
        [HarmonyPrefix]
        public static void SaveSurvivors()
        {
            pawns = Settings.GetHandle<CrashlandingHandle>("savedPawns");
            if (Settings.GetHandle<bool>("isCrashlanding"))
            {
                pawns.Value = PawnsFinder.AllMapsAndWorld_Alive;
            }
        }
    }

}