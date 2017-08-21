using System;
using Verse;
using HugsLib.Settings;
namespace FNotAgainMod
{

    [Serializable]
    public class CrashlandingHandle : SettingHandleConvertible
    {
        [XmlElement] public List<Pawn> pawns = new List<Pawn>();

        public override void FromString(string settingValue)
        {
            SettingHandleConvertibleUtility.DeserializeValuesFromString(settingValue, this);
        }

        public override string ToString()
        {
            return SettingHandleConvertibleUtility.SerializeValuesToString(this);
        }
    }

}