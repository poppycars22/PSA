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

            if (player.data.stats.GetAdditionalData().damageReduction > 1 && ignoreBlock)
                damage /= player.data.stats.GetAdditionalData().damageReduction * 0.5f;
            else if (player.data.stats.GetAdditionalData().damageReduction > 0)
                damage /= player.data.stats.GetAdditionalData().damageReduction;

            if (damagingPlayer != null && player.data.stats.GetAdditionalData().thorns > 0)
                damagingPlayer.data.healthHandler.DoDamage(damage * player.data.stats.GetAdditionalData().thorns, Vector2.down, Color.green, null, null, false, false, true);
            if (damagingPlayer != null && damagingPlayer.data.stats.GetAdditionalData().thorns > 0 && damagingPlayer.data.stats.GetAdditionalData().selfThorns)
                damagingPlayer.data.healthHandler.DoDamage(damage * damagingPlayer.data.stats.GetAdditionalData().thorns, Vector2.down, Color.green, null, null, false, false, true);
        }
    }
}