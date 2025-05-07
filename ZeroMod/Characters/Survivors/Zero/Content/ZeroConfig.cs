using BepInEx.Configuration;
using ZeroMod.Modules;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroConfig
    {

        public static ConfigEntry<bool> ChungLeePoseBool;
        public static ConfigEntry<bool> enableVoiceBool;
        public static ConfigEntry<bool> enableToolTipBool;
        public static ConfigEntry<bool> x4VoicesBool;

        public static ConfigEntry<int> enableZFootstep;
        public static ConfigEntry<int> ZeroFirstUpgradeInt;
        public static ConfigEntry<int> ZeroSecondUpgradeInt;
        public static ConfigEntry<int> ZeroThirdUpgradeInt;
        public static ConfigEntry<int> ZeroFourthUpgradeInt;

        public static void Init()
        {
            string section = "Zero";

            enableToolTipBool = Config.BindAndOptions(
                section,
                "Enable ToolTip",
                true,
                "Show a visual tooltip for certain skills that require holding mouse buttons. Disable to hide this indicator.");

            enableVoiceBool = Config.BindAndOptions(
                section,
                "Enable Voice",
                true,
                "At certain moments or when using a skill, Zero may talk or scream. If you prefer to disable this feature, you can turn it off here.");

            x4VoicesBool = Config.BindAndOptions(
                section,
                "X4 Voices",
                false,
                "Zero saber combo will have the Megaman X4 VFX. \n\n Zero Death SFX will also change to Megaman X4 SFX \n\n The voices NEED TO BE ENABLE or this will not work.");         

            enableZFootstep = Config.BindAndOptions(
                section,
                "Enable Zero Footstep",
                1,
                0,
                2,
                "Megaman X footstep SFX. \n\n 0 = OFF \n\n 1 = Comand Mission SFX \n\n 2 = MegamanX8 SFX");

            ChungLeePoseBool = Config.BindAndOptions(
                section,
                "ChungLee Pose",
                true,
                "Enable ChungLee Idle pose with BFan");

            ZeroFirstUpgradeInt = Config.BindAndOptions(
                section,
                "First Upgrade",
                3,
                2,
                5,
                "Lvl required to unlock the first upgrades");

            ZeroSecondUpgradeInt = Config.BindAndOptions(
                section,
                "Second Upgrade",
                5,
                3,
                7,
                "Lvl required to unlock the second upgrades");

            ZeroThirdUpgradeInt = Config.BindAndOptions(
                section,
                "Third Upgrade",
                7,
                4,
                9,
                "Lvl required to unlock the third upgrades");

            ZeroFourthUpgradeInt = Config.BindAndOptions(
                section,
                "Fourth Upgrade",
                10,
                5,
                12,
                "Lvl required to unlock the fourth upgrades");
        }
    }
}
