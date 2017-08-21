using System;
using HugsLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.Pawn;
using System.Reflection;
using HugsLib.Settings;
using UnityEngine;

namespace FNotAgainMod
{

    public class FNotAgainMod : ModBase
    {
        private FNotAgainMod()
        {
            instance = this;
        }

        private static FNotAgainMod instance = null;
        public static FNotAgainMod Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FNotAgainMod();
                }
                return instance;
            }
        }

        public const string Identifier = "com.github.fyarn.FNotAgainMod";

        private static List<Pawn> pawns = new List<Pawn>();

        private SettingHandle<bool> crashlandLaunchedPawns;

        private SettingHandle<CrashlandingHandle> savedPawns;

        public List<Pawn> SavedPawns
        {
            get
            {
                return savedPawns.Value;
            }
            set
            {
                savedPawns.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        public override string ModIdentifier
        {
            get { return FNotAgainMod.Identifier; }
        }

        public override void DefsLoaded()
        {
            isCrashlanding = Settings.GetHandle<bool>("isCrashlanding", "toggleSetting_label".Translate(), "toggleSetting_desc".Translate(), true);
            savedPawns = Settings.GetHandle<CrashlandingHandle>("savedPawns", "saveDataHiddenTitle", "saveDataHiddenLabel", "");
            savedPawns.NeverVisible = true;
        }
    }
}