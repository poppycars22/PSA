using TabInfo.Utils;
using PSA.Extensions;

namespace PSA
{
    public class TabinfoInterface
    {
        public static void Setup()
        {
            var ExtraStats = TabInfoManager.RegisterCategory("PSA Stats", 20);
            TabInfoManager.RegisterStat(ExtraStats, "Damage Reduction (Damage from enemies is divided by this number)", (p) => p.data.stats.GetAdditionalData().damageReduction > 1f, (p) => string.Format("{0:F0}%", (p.data.stats.GetAdditionalData().damageReduction - 1f) * 100));
            TabInfoManager.RegisterStat(ExtraStats, "Damage Reduction Flat (Damage is reduced by this much)", (p) => p.data.stats.GetAdditionalData().damageReductionFlat > 0f, (p) => string.Format("{0:F0}%", (p.data.stats.GetAdditionalData().damageReductionFlat) * 100));
            TabInfoManager.RegisterStat(ExtraStats, "Damage Multiplier", (p) => p.data.stats.GetAdditionalData().damageMult >1f, (p) => string.Format("{0:F0}%", (p.data.stats.GetAdditionalData().damageMult-1) * 100));
            TabInfoManager.RegisterStat(ExtraStats, "Thorns", (p) => p.data.stats.GetAdditionalData().thorns != 0f, (p) => string.Format("{0:F0}%", (p.data.stats.GetAdditionalData().thorns) * 100));
            TabInfoManager.RegisterStat(ExtraStats, "Slow Thorns", (p) => p.data.stats.GetAdditionalData().slowThorns != 0f, (p) => string.Format("{0:F0}%", (p.data.stats.GetAdditionalData().slowThorns) * 100));
            TabInfoManager.RegisterStat(ExtraStats, "Self Thorns: ", (p) => p.data.stats.GetAdditionalData().selfThorns != false, (p) => "True");
        }
    }
}