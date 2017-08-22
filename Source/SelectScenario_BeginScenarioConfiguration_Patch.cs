using System.Collections.Generic;
using RimWorld;
using Verse;
using Harmony;
using HugsLib.Settings;


namespace FNotAgain_Mod {

    [StaticConstructorOnStartup]
    public static class SelectScenario_BeginScenarioConfiguration_Patch
    {
        static SelectScenario_BeginScenarioConfiguration_Patch()
        {
            //var harmony = HarmonyInstance.Create(FNotAgainMod.HarmonyInstanceId);
            //harmony.PatchAll(typeof(FNotAgainMod).Assembly);
            //FNotAgainMod.Logger.Message("see me?");
        }
    }

    [HarmonyPatch(typeof(Page_SelectScenario), "BeginScenarioConfiguration")]
    class Crashland
    {
        [HarmonyPostfix]
        public static void Crashland_Main(Page_SelectScenario __instance)
        {
            var pawns = FNotAgain_Mod.Instance.SavedPawns;
            if ( Settings.GetHandle<bool>("isCrashlanding") && pawns != null)
            {
                Current.Game.InitData.startingPawns = pawns;
            }
        }
    }

    [HarmonyPatch(typeof(ShipCountdown), "CountdownEnded")]
    class SaveSurvivors {
        [HarmonyPrepare]
        public static void SaveSurvivors_Main()
        {
            List<Pawn> pawnsToSave;
            var savedPawns = FNotAgain_Mod.Settings.GetHandle<CrashlandingHandle>("savedPawns");
            
            if (__instance.journeyDestinationTile >= 0)
            {
                CaravanJourneyDestinationUtility.PlayerCaravansAt(__instance.journeyDestinationTile, __instance.caravans);
                foreach (List<Pawn> p in __instance.caravans)
                {
                    pawnsToSave.addRange(p);
                }
            }
            else
            {
                List<Building> list = ShipUtility.ShipBuildingsAttachedTo(__instance.shipRoot).ToList<Building>();
                foreach (Building current in list)
                {
                    Building_CryptosleepCasket building_CryptosleepCasket = current as Building_CryptosleepCasket;
                    if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents && building_CryptosleepCasket.ContainedThing == typeof(Pawn))
                    {
                        pawnsToSave.add(building_CryptosleepCasket.ContainedThing);
                    }
                }
            }
        }
    }

}