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
            //damage = new Vector2(Math.Abs(damage.x) + Math.Abs(damage.y), 0);

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
            damage *= player.data.stats.GetAdditionalData().damageMult;
            if (damagingPlayer != null)
                damage -= damage * (Mathf.Clamp(player.data.stats.GetAdditionalData().damageReductionFlat, 0, 0.9f) - (Mathf.Clamp(player.data.stats.GetAdditionalData().damageReductionFlat, 0, 0.9f) * damagingPlayer.data.stats.GetAdditionalData().reductionPierce));
            else
                damage -= damage * Mathf.Clamp(player.data.stats.GetAdditionalData().damageReductionFlat, 0, 0.9f);
            if (player.data.stats.GetAdditionalData().damageReduction > 1 && damagingPlayer!=null)
                temp = player.data.stats.GetAdditionalData().damageReduction - (player.data.stats.GetAdditionalData().damageReduction * damagingPlayer.data.stats.GetAdditionalData().reductionPierce);
            else
                temp = player.data.stats.GetAdditionalData().damageReduction;
            if (temp > 0)
                damage /= temp;
            Vector2 dmg2becauseunityisstupid = damage;
            Main.instance.ExecuteAfterFrames(5, ()=> { 
            if (damagingPlayer != null && player.data.stats.GetAdditionalData().thorns > 0 && player.playerID != damagingPlayer.playerID)
                damagingPlayer.data.healthHandler.TakeDamage(dmg2becauseunityisstupid * player.data.stats.GetAdditionalData().thorns*damagingPlayer.data.stats.GetAdditionalData().thornsPercent, Vector2.down, Color.green, null, null, player.data.stats.GetAdditionalData().lethalThorns, true);
            if (damagingPlayer != null && damagingPlayer.data.stats.GetAdditionalData().thorns > 0 && damagingPlayer.data.stats.GetAdditionalData().selfThorns && player.playerID!=damagingPlayer.playerID)
                damagingPlayer.data.healthHandler.TakeDamage(dmg2becauseunityisstupid * (damagingPlayer.data.stats.GetAdditionalData().thorns*damagingPlayer.data.stats.GetAdditionalData().selfThornsPercent), Vector2.down, Color.green, null, null, damagingPlayer.data.stats.GetAdditionalData().lethalSelfThorns, true);
            });
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