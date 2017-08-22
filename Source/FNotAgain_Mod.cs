using System;
using HugsLib;
using System.Collections.Generic;
using Verse;
using HugsLib.Settings;
using HugsLib.Source.Settings;
using System.Xml.Serialization;
using Harmony;
using RimWorld;
using RimWorld.Planet;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;

namespace FNotAgain_Mod
{

    public class FNotAgain_Mod : HugsLib.ModBase
    {
        private FNotAgain_Mod()
        {
            instance = this;
        }

        public override void Initialize()
        {
            Crashland.settings = this.Settings;
            SaveSurvivors.settings = this.Settings;
            base.Initialize();
        }

        private static FNotAgain_Mod instance = null;
        public static FNotAgain_Mod Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FNotAgain_Mod();
                }
                return instance;
            }
        }

        public const string Identifier = "FNotAgain_Mod";
        public const string HarmonyInstanceId = "FNotAgain_Mod";

        private SettingHandle<bool> isCrashlanding;

        private List<Pawn> savedPawns = new List<Pawn>();

        public List<Pawn> SavedPawns
        {
            get => savedPawns;
            set => savedPawns = value;
        }

        public bool IsCrashlanding
        {
            get => isCrashlanding.Value;
            set => isCrashlanding.Value = value;
        }

        public override string ModIdentifier
        {
            get { return FNotAgain_Mod.Identifier; }
        }

        public override void DefsLoaded()
        {
            isCrashlanding = Settings.GetHandle<bool>("isCrashlanding", "toggleSetting_label".Translate(), "toggleSetting_desc".Translate(), true);
        }
        
        [HarmonyPatch(typeof(Page_ConfigureStartingPawns), "PreOpen")]
        class Crashland
        {
            public static ModSettingsPack settings;

            [HarmonyPrefix]
            public static void Crashland_Main()
            {
                FNotAgain_Mod.Instance.Logger.Message("getting saved pawns");
                var pawns = FNotAgain_Mod.Instance.SavedPawns;
                FNotAgain_Mod.Instance.Logger.Message("checking for crash");
                if (settings.GetHandle<bool>("isCrashlanding") && pawns.Count != 0)
                {
                    foreach(Pawn p in pawns)
                    {
                        FNotAgain_Mod.Instance.Logger.Message("Clearing relations");
                        p.relations.ClearAllRelations();
                    }
                    FNotAgain_Mod.Instance.Logger.Message("Loaded " + pawns.Count + " pawns");
                    Current.Game.InitData.startingPawns = pawns;
                }
            }
        }

        [HarmonyPatch(typeof(ShipCountdown), "InitiateCountdown")]
        class SaveSurvivors
        {
            public static ModSettingsPack settings;

            [HarmonyPrefix]
            public static void InitiateCountdown_Patch(ref Building launchingShipRoot, ref int journeyDestinationTile)
            {
                List<Pawn> pawnsToSave = new List<Pawn>();
                FNotAgain_Mod.Instance.Logger.Message("else");
                List<Building> list = ShipUtility.ShipBuildingsAttachedTo(launchingShipRoot).ToList<Building>();
                foreach (Building current in list)
                {
                    FNotAgain_Mod.Instance.Logger.Message("foreach");
                    Building_CryptosleepCasket building_CryptosleepCasket = current as Building_CryptosleepCasket;
                    if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents && building_CryptosleepCasket.ContainedThing.GetType() == typeof(Pawn))
                    {
                        FNotAgain_Mod.Instance.Logger.Message("woo");
                        pawnsToSave.Add((Pawn)building_CryptosleepCasket.ContainedThing);
                    }
                }
                FNotAgain_Mod.Instance.SavedPawns = pawnsToSave;
                FNotAgain_Mod.Instance.Logger.Message("Saved " + FNotAgain_Mod.Instance.SavedPawns.Count + " pawns");
            }
        }
        
        [Serializable]
        class SerializablePawn : Pawn
        {}
    }
}