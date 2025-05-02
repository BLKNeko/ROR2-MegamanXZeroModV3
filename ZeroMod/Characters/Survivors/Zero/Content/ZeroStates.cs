using ZeroMod.Modules.BaseContent.BaseStates;
using ZeroMod.Modules.BaseStates;
using ZeroMod.Survivors.Zero.SkillStates;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroStates
    {
        public static void Init()
        {
            //Modules.Content.AddEntityState(typeof(SlashCombo));

            //Modules.Content.AddEntityState(typeof(Shoot));

            //Modules.Content.AddEntityState(typeof(Roll));

            //Modules.Content.AddEntityState(typeof(ThrowBomb));


            Modules.Content.AddEntityState(typeof(ZeroDeathState));
            Modules.Content.AddEntityState(typeof(ZeroSpawnState));
            Modules.Content.AddEntityState(typeof(KuuenzanState));
            Modules.Content.AddEntityState(typeof(BaseMeleeAttackZP));
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack2));

            Modules.Content.AddEntityState(typeof(BFan));
            Modules.Content.AddEntityState(typeof(CFlasher));
            Modules.Content.AddEntityState(typeof(DashPunch));
            Modules.Content.AddEntityState(typeof(Enkoujin));
            Modules.Content.AddEntityState(typeof(Gokumonken));
            Modules.Content.AddEntityState(typeof(GokumonkenAtk));
            Modules.Content.AddEntityState(typeof(GokumonkenEnd));
            Modules.Content.AddEntityState(typeof(HammerFall));
            Modules.Content.AddEntityState(typeof(HammerFallAtk));
            Modules.Content.AddEntityState(typeof(IceDragonRise));
            Modules.Content.AddEntityState(typeof(IceFurySlash));
            Modules.Content.AddEntityState(typeof(KKnuckle));
            Modules.Content.AddEntityState(typeof(Kuuenzan));
            Modules.Content.AddEntityState(typeof(Raikousen));
            Modules.Content.AddEntityState(typeof(Roll));
            Modules.Content.AddEntityState(typeof(Ryuuenjin));
            Modules.Content.AddEntityState(typeof(Shoot));
            Modules.Content.AddEntityState(typeof(SigmaBlade));
            Modules.Content.AddEntityState(typeof(SlashCombo));
            Modules.Content.AddEntityState(typeof(SpinKick));
            Modules.Content.AddEntityState(typeof(TBreaker));
            Modules.Content.AddEntityState(typeof(ThrowBomb));
            Modules.Content.AddEntityState(typeof(ZBuster));
            Modules.Content.AddEntityState(typeof(ZDash));
            Modules.Content.AddEntityState(typeof(ZHammer3));
            Modules.Content.AddEntityState(typeof(ZSaber));
            Modules.Content.AddEntityState(typeof(ZSSlashCombo));
            Modules.Content.AddEntityState(typeof(ZSSlashCombo2));
            Modules.Content.AddEntityState(typeof(ZSSlashCombo3));
            Modules.Content.AddEntityState(typeof(ZSSlashCombo4));
            Modules.Content.AddEntityState(typeof(ZSSlashCombo5));



        }
    }
}
