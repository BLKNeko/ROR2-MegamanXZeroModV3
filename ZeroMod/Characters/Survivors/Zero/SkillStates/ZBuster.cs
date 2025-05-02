using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using RoR2.Projectile;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZBuster : BaseSkillState
    {
        public static float damageCoefficient = ZeroStaticValues.gunDamageCoefficient;
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
        private GameObject muzzleEffectPrefab;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzz";
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashFMJ");

            if (characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                PlayAnimation("FullBody, Override", "Hadouken", "attackSpeed", this.duration);
            }
            else
            {
                PlayAnimation("Gesture, Override", "ZeroBuster", "attackSpeed", this.duration);
            }

            
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
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    if (ZeroConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(ZeroStaticValues.zeroAttackVFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(ZeroStaticValues.zBuster, this.gameObject);

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