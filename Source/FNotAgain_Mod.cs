using System.Collections.Generic;
using Verse;
using HugsLib.Settings;
using Harmony;
using RimWorld;
using System.Linq;

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
            isCrashlanding = Settings.GetHandle<bool>("isCrashlanding", "Crashlanding?", "Auto-load saved colonists in new colony creation.", true);
        }
        
        [HarmonyPatch(typeof(Page_ConfigureStartingPawns), "PreOpen")]
        class Crashland
        {
            public static ModSettingsPack settings;

            [HarmonyPrefix]
            public static void Crashland_Main()
            {
                var pawns = FNotAgain_Mod.Instance.SavedPawns;
                if (settings.GetHandle<bool>("isCrashlanding") && pawns.Count != 0)
                {
                    foreach(Pawn p in pawns)
                    {
                        p.relations.ClearAllRelations();
                    }
                    Current.Game.InitData.startingPawns = pawns;
                }
            }
        }

        [HarmonyPatch(typeof(ShipCountdown), "InitiateCountdown")]
        class SaveSurvivors
        {
            [HarmonyPrefix]
            public static void InitiateCountdown_Patch(ref Building launchingShipRoot, ref int journeyDestinationTile)
            {
                List<Pawn> pawnsToSave = new List<Pawn>();
                List<Building> list = ShipUtility.ShipBuildingsAttachedTo(launchingShipRoot).ToList<Building>();
                foreach (Building current in list)
                {
                    Building_CryptosleepCasket building_CryptosleepCasket = current as Building_CryptosleepCasket;
                    if (building_CryptosleepCasket != null && building_CryptosleepCasket.HasAnyContents && (building_CryptosleepCasket.ContainedThing.GetType() == typeof(Pawn) || building_CryptosleepCasket.ContainedThing.GetType() == typeof(Psychology.PsychologyPawn)))
                    {
                        pawnsToSave.Add((Pawn)building_CryptosleepCasket.ContainedThing);
                    }
                }
                FNotAgain_Mod.Instance.SavedPawns = pawnsToSave;
                FNotAgain_Mod.Instance.Logger.Message("Saved " + FNotAgain_Mod.Instance.SavedPawns.Count + " pawns");
            }
        }
    }
}
