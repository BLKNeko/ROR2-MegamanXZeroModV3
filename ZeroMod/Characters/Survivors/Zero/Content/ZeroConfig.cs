using BepInEx.Configuration;
using ZeroMod.Modules;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroConfig
    {
        public static ConfigEntry<bool> someConfigBool;
        public static ConfigEntry<float> someConfigFloat;
        public static ConfigEntry<float> someConfigFloatWithCustomRange;

        public static ConfigEntry<bool> ChungLeePoseBool;

        public static void Init()
        {
            string section = "Zero";

            ChungLeePoseBool = Config.BindAndOptions(
                section,
                "ChungLeePoseBool",
                true,
                "Enable ChungLee Idle pose with BFan");

            someConfigFloat = Config.BindAndOptions(
                section,
                "someConfigfloat",
                5f);//blank description will default to just the name

            someConfigFloatWithCustomRange = Config.BindAndOptions(
                section,
                "someConfigfloat2",
                5f,
                0,
                50,
                "if a custom range is not passed in, a float will default to a slider with range 0-20. risk of options only has sliders");
        }
    }
}
