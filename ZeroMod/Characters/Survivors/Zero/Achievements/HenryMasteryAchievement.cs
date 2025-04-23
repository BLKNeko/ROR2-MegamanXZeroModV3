using RoR2;
using ZeroMod.Modules.Achievements;

namespace ZeroMod.Survivors.Zero.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class HenryMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = ZeroSurvivor.ZERO_X_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = ZeroSurvivor.ZERO_X_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => ZeroSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}