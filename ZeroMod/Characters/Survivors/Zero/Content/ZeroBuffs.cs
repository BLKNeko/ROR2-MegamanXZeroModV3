using RoR2;
using UnityEngine;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static BuffDef TBreakerBuff;
        public static BuffDef BFanBuff;
        public static BuffDef KKnuckleBuff;
        public static BuffDef SigmaBladeBuff;

        public static BuffDef GokumonkenBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            TBreakerBuff = Modules.Content.CreateAndAddBuff("TBreakerBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            BFanBuff = Modules.Content.CreateAndAddBuff("BFanBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            KKnuckleBuff = Modules.Content.CreateAndAddBuff("KKnuckleBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            SigmaBladeBuff = Modules.Content.CreateAndAddBuff("SigmaBladeBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            GokumonkenBuff = Modules.Content.CreateAndAddBuff("GokumonkenBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

        }
    }
}
