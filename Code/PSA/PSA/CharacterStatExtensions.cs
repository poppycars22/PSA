﻿using System;
using System.Runtime.CompilerServices;
using HarmonyLib;

namespace PSA.Extensions
{
    // ADD FIELDS TO GUN
    [Serializable]
    public class CharacterStatModifiersAdditionalData
    {
        public bool healDmg;
        public float healDmgPercent;
        public float healReduction;
        public float damageReduction;
        public float thorns;
        public bool selfThorns;
        public float selfThornsPercent;
        public bool noHeal;
        public CharacterStatModifiersAdditionalData()
        {
            healDmg = false;
            healDmgPercent = 1f;
            healReduction = 1f;
            damageReduction = 1f;
            thorns = 0f;
            selfThorns = false;
            selfThornsPercent = 1f;
            noHeal = false;
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
            __instance.GetAdditionalData().healReduction = 1f;
            __instance.GetAdditionalData().healDmgPercent = 1f;
            __instance.GetAdditionalData().damageReduction = 1f;
            __instance.GetAdditionalData().thorns = 0f;
            __instance.GetAdditionalData().selfThorns = false;
            __instance.GetAdditionalData().selfThornsPercent = 1f;
            __instance.GetAdditionalData().noHeal = false;
        }
    }
}