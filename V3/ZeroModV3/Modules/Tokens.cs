using R2API;
using System;

namespace ZeroModV3.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Zero
            string prefix = ZeroModV3Plugin.DEVELOPER_PREFIX + "_ZERO_V3_BODY_";

            string desc = "Zero, S class Hunter<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Z-Buster is a chargeable cannon. It can fire regular shots as well as Charge attacks." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Emergency Acceleration System(DASH) is a move that temporarily speeds up the character zero also cover his sword with ice that freeze the enemies upon contact" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Zero's Ryuenjin make him perform a powerful flaming uppercut, on the air he strikes down with Enkoujin that causes burn to the enemies and explodes the surrounding area.</color>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Zero's Learning System make him even stronger on level up" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 3 Zero receive one Hopoo Feather" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 4 Zero can use <style=cIsDamage>RASETSUSEN</style> by holding down the shift key during <style=cIsDamage>RYUENJIN</style>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 4 Zero can use <style=cIsDamage>ENKOUJIN</style> by holding down R key during <style=cIsDamage>RASETSUSEN</style>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 5 Zero gain a new saber combo" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 7 Zero receive one Backup Magazine" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 10 Z-Buster's charged shots now gain 1 extra shot" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > On Level 10 zero receives another Hopoo Feather" + Environment.NewLine + Environment.NewLine;

            string outro = "Mission accomplished.";
            string outroFailure = "Sorry...I...Failed....";

            LanguageAPI.Add(prefix + "NAME", "Zero");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Zero, S class Hunter");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Zero's Learning System");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>Zero starts with a double jump and his basic attacks can</style> <style=cIsHealing> stun </style> <style=cIsUtility>some enemies</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "ZSABER_NAME", "Z-Saber");
            LanguageAPI.Add(prefix + "ZSABER_DESCRIPTION", "Zero can perform a 3 combo attack with his saber, dealing <style=cIsDamage> 150 % damage </style>, <style=cIsDamage> 180 % damage </style>,  <style=cIsDamage> 200 % damage </style> and can stun some enemies.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "ZBUSTER_NAME", "Z-Buster");
            LanguageAPI.Add(prefix + "ZBUSTER_DESCRIPTION", "Fire shots that shocks the enemies, dealing <style=cIsDamage> 170 % damage </style>. On charge fire a normal shot and a powerful piercing shot");

            LanguageAPI.Add(prefix + "ZDEF_NAME", "Gokumonken");
            LanguageAPI.Add(prefix + "ZDEF_DESCRIPTION", "<style=cIsDamage>By holding down the attack button </style> while standing still, Zero enters a defensive stance.");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "ZDASH_NAME", "F-Splasher");
            LanguageAPI.Add(prefix + "ZDASH_DESCRIPTION", "<style=cIsDamage>Perform a Dash that damage enemies upon contact</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "UPSLASH_NAME", "Ryuenjin");
            LanguageAPI.Add(prefix + "UPSLASH_DESCRIPTION", "Perform a powerful uppercut flaming attack, dealing <style=cIsDamage>250% damage</style>, on the air he strikes down with Enkoujin that causes burn to the enemies, dealing <style=cIsDamage>350% damage</style> and explodes the surrounding area, dealing <style=cIsDamage>300% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}