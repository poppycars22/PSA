using System;
using HarmonyLib;
using UnityEngine;
using PSA.Extensions;
using UnboundLib;


namespace PSA.Patches
{
    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "DoDamage")]
    class HealtHandlerPatchDoDamage
    {
        // patch for Totem and Damage Reduction
        private static void Prefix(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, ref bool lethal, bool ignoreBlock)
        {
            float temp =1;
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            Player player = data.player;
            if (!data.isPlaying)
            {
                return;
            }
            if (data.dead)
            {
                return;
            }
            if (__instance.isRespawning)
            {
                return;
            }
            if (player.data.stats.GetAdditionalData().damageReduction > 1 && damagingPlayer!=null)
                temp = player.data.stats.GetAdditionalData().damageReduction - (player.data.stats.GetAdditionalData().damageReduction * Math.Clamp(damagingPlayer.data.stats.GetAdditionalData().reductionPierce, 0, float.MaxValue));
            else
                temp = player.data.stats.GetAdditionalData().damageReduction;
            if (temp > 0)
                damage /= temp;
            if (damagingPlayer != null && player.data.stats.GetAdditionalData().thorns > 0)
                damagingPlayer.data.healthHandler.DoDamage(damage * player.data.stats.GetAdditionalData().thorns*Math.Clamp(damagingPlayer.data.stats.GetAdditionalData().thornsPercent,0,float.MaxValue), Vector2.down, Color.green, null, null, false, false, true);
            if (damagingPlayer != null && damagingPlayer.data.stats.GetAdditionalData().thorns > 0 && damagingPlayer.data.stats.GetAdditionalData().selfThorns)
                damagingPlayer.data.healthHandler.DoDamage(damage * (damagingPlayer.data.stats.GetAdditionalData().thorns*Math.Clamp(damagingPlayer.data.stats.GetAdditionalData().selfThornsPercent,0,float.MaxValue)), Vector2.down, Color.green, null, null, false, false, true);
        }
    }

    [Serializable]
    [HarmonyPatch(typeof(HealthHandler), "Heal")]
    class HealingPatch
    {
        private static void Prefix(HealthHandler __instance, ref float healAmount)
        {
            Vector2 damage;
            Player player = (Player)__instance.GetFieldValue("player");
            if (player.data.stats.GetAdditionalData().healDmg == true)
            {
                if (player.data.stats.GetAdditionalData().healDmgPercent > 0)
                    damage = Vector2.down * (healAmount * player.data.stats.GetAdditionalData().healDmgPercent);
                else
                    damage = Vector2.down * (healAmount);
                healAmount = 0;
                __instance.DoDamage(damage, Vector2.down, Color.red, null, null, false, false, true);
            }
            if (player.data.stats.GetAdditionalData().noHeal)
                healAmount = 0;
            if (player.data.stats.GetAdditionalData().healReduction > 0)
                healAmount /= player.data.stats.GetAdditionalData().healReduction;
            else
                healAmount = 0;
        }
    }
}