using ZeroModV3.SkillStates;
using ZeroModV3.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace ZeroModV3.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(Slash));
            Modules.Content.AddEntityState(typeof(Slash2));
            Modules.Content.AddEntityState(typeof(Slash3));
            Modules.Content.AddEntityState(typeof(UpSlash));
            Modules.Content.AddEntityState(typeof(DownSlash));
            Modules.Content.AddEntityState(typeof(GroundImpact));
            Modules.Content.AddEntityState(typeof(ZDefense));
            Modules.Content.AddEntityState(typeof(ZDash));
            Modules.Content.AddEntityState(typeof(ZBuster));
            Modules.Content.AddEntityState(typeof(ZBuster2));
            Modules.Content.AddEntityState(typeof(Slash4));
            Modules.Content.AddEntityState(typeof(RasetsusenCombo));
            Modules.Content.AddEntityState(typeof(LearningSystem));
            Modules.Content.AddEntityState(typeof(DeathState));
            Modules.Content.AddEntityState(typeof(SpawnState));
        }
    }
}