using R2API;
using System;
using ZeroMod.Modules;
using ZeroMod.Survivors.Zero.Achievements;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroTokens
    {
        public static void Init()
        {
            AddZeroTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Zero.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddZeroTokens() 
        {
            string prefix = ZeroSurvivor.ZERO_X_PREFIX;

            string desc = "<color=#CCD3E0>Zero, the S-Class Maverick Hunter.</color>\n\n";
            desc += "< ! > Zero uses his Learning System to master new saber techniques, similar to X's Variable Weapon System.\n\n";
            desc += "< ! > The <color=#FF0000>Z-Buster</color> is a powerful weapon capable of unleashing devastating shots.\n\n";
            desc += "< ! > The <color=#CCD3E0>Emergency Acceleration System (DASH)</color> temporarily boosts Zero's speed, making it ideal for repositioning or evading danger.\n\n";
            desc += "< ! > Zero unlocks his full potential as he levels up, gaining access to unique techniques with diverse effects.\n\n";
            desc += "< ! > He also carries four distinct weapons, each with different stats to match your strategy.\n";


            string outro = "Mission accomplished.";
            string outroFailure = "Sorry...I...Failed....";

            Language.Add(prefix + "NAME", "Zero");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Zero, the S-Class Maverick Hunter");
            Language.Add(prefix + "LORE", "Maverick Hunter");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "DEFAULT_SKIN_NAME", "Zero");

            Language.Add(prefix + "BZ_SKIN_NAME", "Black Zero");

            Language.Add(prefix + "NZ_SKIN_NAME", "Zero Nightmare");

            Language.Add(prefix + "VZ_SKIN_NAME", "ViA");
            #endregion

            #region Weapons

            Language.Add(prefix + "WEAPON_ZSABER_NAME", "Z-Saber");
            Language.Add(prefix + "WEAPON_ZSABER_DESCRIPTION", "<color=#FF0000>ZERO</color>'s iconic saber, capable of fast and deadly combo slashes.");

            Language.Add(prefix + "WEAPON_TBREAKER_NAME", "T-Breaker");
            Language.Add(prefix + "WEAPON_TBREAKER_DESCRIPTION", "The <color=#FF0000>T-Breaker</color> is a massive, super-powerful hammer that crushes foes with devastating force.");

            Language.Add(prefix + "WEAPON_BFAN_NAME", "B-Fan");
            Language.Add(prefix + "WEAPON_BFAN_DESCRIPTION", "The <color=#FF0000>B-Fan</color> is a versatile weapon offering both high offensive and defensive capabilities. When <color=#FF0000>ZERO</color> equipped with the B-Fan, it generates a protective energy shield, enhancing his defense.");

            Language.Add(prefix + "WEAPON_KKNUCKLE_NAME", "K-Knuckle");
            Language.Add(prefix + "WEAPON_KKNUCKLE_DESCRIPTION", "The <color=#FF0000>K-Knuckle</color> is a powerful brass knuckle. Equipping it grants <color=#FF0000>ZERO</color> access to devastating special techniques inspired by the legendary <style=cIsDamage>Street Fighter</style> series.");

            Language.Add(prefix + "WEAPON_SIGMABLADE_NAME", "Σ-Blade");
            Language.Add(prefix + "WEAPON_SIGMABLADE_DESCRIPTION", "The <color=#FF0000>Σ-Blade</color> is the weapon once wielded by Sigma. It shares the same abilities as the <color=#FF0000>Z-Saber</color>, but radiates a far more ominous presence.");

            #endregion


            #region Passive
            Language.Add(prefix + "PASSIVE_LEARNINGSYSTEM_NAME", "Zero's Learning System");
            Language.Add(prefix + "PASSIVE_LEARNINGSYSTEM_DESCRIPTION", "Zero showcases the power of rapid adaptability, evolving with each level gained. As he grows, new combos and upgrades become available, turning the tide of battle.");

            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_ZSABER_COMBO_NAME", "Z-Saber");
            Language.Add(prefix + "PRIMARY_ZSABER_COMBO_DESCRIPTION", $"Zero performs swift combo slashes with his Z-Saber, dealing <style=cIsDamage>{100f * ZeroStaticValues.ZSaber1DamageCoefficient}% damage</style> with the first hit. Each consecutive strike in the combo increases damage, allowing for devastating finishers.");

            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_ZBUSTER_NAME", "Z-Buster");
            Language.Add(prefix + "SECONDARY_ZBUSTER_DESCRIPTION", $"Shoot with Z - Buster, dealing <style=cIsDamage>{100f * ZeroStaticValues.ZBusterDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ZDASH_NAME", "Dash");
            Language.Add(prefix + "UTILITY_ZDASH_DESCRIPTION", "The <color=#CCD3E0>Emergency Acceleration System (DASH)</color> temporarily boosts Zero's speed, making it ideal for repositioning or evading danger.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_SFLASHER_NAME", "C-Flasher");
            Language.Add(prefix + "SPECIAL_SFLASHER_DESCRIPTION", $"Hit the ground to scatter multiple light rounds, each light round inflicts <style=cIsDamage>{100f * ZeroStaticValues.CFlasherDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "SPECIAL_RYUUENJIN_NAME", "Ryuuenjin");
            Language.Add(prefix + "SPECIAL_RYUUENJIN_DESCRIPTION", $"Perform an uppercut with flames converted from energy, inflicting <style=cIsDamage>{100f * ZeroStaticValues.RyuuenjinDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "SPECIAL_ICEDRAGONRISE_NAME", "Hyouryuushou");
            Language.Add(prefix + "SPECIAL_ICEDRAGONRISE_DESCRIPTION", $"Zero performs a spinning, rising uppercut attack surrounded by snow, inflicting <style=cIsDamage>{100f * ZeroStaticValues.IceDragonRiseDamageCoefficient}% damage</style> and Freezes enemies on contact.");

            Language.Add(prefix + "SPECIAL_GOKUMONKEN_NAME", "Gokumonken");
            Language.Add(prefix + "SPECIAL_GOKUMONKEN_DESCRIPTION", "<style=cIsDamage>Hold the attack button</style> while standing still to enter a defensive stance. If <color=#FF0000>ZERO</color> is hit during this stance, he retaliates with a counterstrike that reflects a portion of the incoming damage, followed by a powerful cross-slash. If no attack is received, the stance ends without effect.");

            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Zero: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Zero, beat the game or obliterate on Monsoon.");
            #endregion
        }

        public static void AddHenryTokens()
        {
            string prefix = ZeroSurvivor.ZERO_X_PREFIX;

            string desc = "Zero is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            Language.Add(prefix + "NAME", "Zero");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Chosen One");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Zero passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * ZeroStaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * ZeroStaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * ZeroStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Zero: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Zero, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
