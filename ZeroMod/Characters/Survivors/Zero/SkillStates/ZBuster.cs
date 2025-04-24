using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using RoR2.Projectile;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZBuster : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.9f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("Gesture, Override", "ZeroBuster", "attackSpeed", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    characterBody.AddSpreadBloom(0.8f);
                    //EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    //if (XConfig.enableVoiceBool.Value)
                    //{
                    //    AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_VSFX, this.gameObject);
                    //}
                    //AkSoundEngine.PostEvent(XStaticValues.X_ChameleonSting_SFX, this.gameObject);

                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    FireProjectileInfo ZeroBusterProjectille = new FireProjectileInfo();
                    ZeroBusterProjectille.projectilePrefab = ZeroAssets.CFlasherProjectile;
                    ZeroBusterProjectille.position = aimRay.origin;
                    ZeroBusterProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    ZeroBusterProjectille.owner = gameObject;
                    ZeroBusterProjectille.damage = damageCoefficient * damageStat;
                    ZeroBusterProjectille.force = force;
                    ZeroBusterProjectille.crit = RollCrit();
                    ZeroBusterProjectille.damageColorIndex = DamageColorIndex.Luminous;
                    ZeroBusterProjectille.damageTypeOverride = DamageTypeCombo.GenericSecondary;



                    ProjectileManager.instance.FireProjectile(ZeroBusterProjectille);

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}