using System;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace PSA.Extensions
{
    // ADD FIELDS TO GUN
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public bool healDmg;
        public bool noHeal;
        public float damageReduction;
        public float damageMult;
        public float damageMultMax;
        public float thorns;
        public bool selfThorns;
        public CharacterStatModifiersAdditionalData()
        {
            healDmg = false;
            noHeal = false;
            damageReduction = 1f;
            damageMult = 1f;
            damageMultMax = 1f;
            thorns = 0f;
            selfThorns = false;
        }
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData> data = new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersAdditionalData>();

        public static CharacterStatModifiersAdditionalData GetAdditionalData(this CharacterStatModifiers statModifiers)
        {
            return data.GetOrCreateValue(statModifiers);
        }

        public static void AddData(this CharacterStatModifiers statModifiers, CharacterStatModifiersAdditionalData value)
        {
            try
            {
                data.Add(statModifiers, value);
            }
            catch (Exception) { }
        }
    }
    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAdditionalData().healDmg = false;
            __instance.GetAdditionalData().noHeal = false;
            __instance.GetAdditionalData().damageReduction = 1f;
            __instance.GetAdditionalData().thorns = 0f;
            __instance.GetAdditionalData().selfThorns = false;
        }
    }
}