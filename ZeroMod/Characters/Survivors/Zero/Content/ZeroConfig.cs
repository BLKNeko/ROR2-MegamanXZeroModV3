using BepInEx.Configuration;
using ZeroMod.Modules;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroConfig
    {

        public static ConfigEntry<bool> ChungLeePoseBool;
        public static ConfigEntry<bool> enableVoiceBool;

        public static ConfigEntry<int> enableZFootstep;
        public static ConfigEntry<int> ZeroFirstUpgradeInt;
        public static ConfigEntry<int> ZeroSecondUpgradeInt;
        public static ConfigEntry<int> ZeroThirdUpgradeInt;
        public static ConfigEntry<int> ZeroFourthUpgradeInt;

        public static void Init()
        {
            string section = "Zero";

            enableVoiceBool = Config.BindAndOptions(
                section,
                "EnableVoice",
                true,
                "At certain moments or when using a skill, Zero may talk or scream. If you prefer to disable this feature, you can turn it off here.");

            enableZFootstep = Config.BindAndOptions(
                section,
                "Enable Zero Footstep",
                1,
                0,
                2,
                "Megaman X footstep SFX. \n\n 0 = OFF \n\n 1 = Comand Mission SFX \n\n 2 = MegamanX8 SFX");

            ChungLeePoseBool = Config.BindAndOptions(
                section,
                "ChungLeePoseBool",
                true,
                "Enable ChungLee Idle pose with BFan");

            ZeroFirstUpgradeInt = Config.BindAndOptions(
                section,
                "FistUpgrade",
                3,
                2,
                5,
                "Lvl required to unlock the first upgrades");

            ZeroSecondUpgradeInt = Config.BindAndOptions(
                section,
                "SecondUpgrade",
                5,
                3,
                7,
                "Lvl required to unlock the second upgrades");

            ZeroThirdUpgradeInt = Config.BindAndOptions(
                section,
                "ThirdUpgrade",
                7,
                4,
                9,
                "Lvl required to unlock the third upgrades");

            ZeroFourthUpgradeInt = Config.BindAndOptions(
                section,
                "FourthUpgrade",
                10,
                5,
                12,
                "Lvl required to unlock the fourth upgrades");
        }
    }
}
