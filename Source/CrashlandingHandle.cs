using System;
using Verse;

[Serializable]
public class CrashlandingHandle : SettingHandleConvertible
{
    [XmlElement] public List<Pawn> nums = new List<Pawn>();

    public override void FromString(string settingValue)
    {
        SettingHandleConvertibleUtility.DeserializeValuesFromString(settingValue, this);
        Log.Message(nums.Join(","));
    }

    public override string ToString()
    {
        return SettingHandleConvertibleUtility.SerializeValuesToString(this);
    }
}