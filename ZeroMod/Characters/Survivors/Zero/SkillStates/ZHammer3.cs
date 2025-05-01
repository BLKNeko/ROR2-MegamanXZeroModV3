using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using HG;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZHammer3 : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 10f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private string muzzleString2;

        private BlastAttack blastAttack;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "SwingDown";
            muzzleString2 = "ZHAMuzz";

            EffectManager.SimpleMuzzleFlash(ZeroAssets.ZSwordVFX, gameObject, muzzleString, true);
            base.PlayAnimation("Gesture, Override", "ZHammer3", "attackSpeed", this.duration);
        }

        public override void OnExit()
        {

            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);

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

                EffectManager.SimpleMuzzleFlash(ZeroAssets.HammerAtkVFX, gameObject, muzzleString2, false);
                Util.PlaySound("HenryShootPistol", gameObject);

                if (isAuthority)
                {

                    blastAttack = new BlastAttack();
                    blastAttack.attacker = base.gameObject;
                    blastAttack.inflictor = base.gameObject;
                    blastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                    blastAttack.baseDamage = damageCoefficient;
                    blastAttack.baseForce = force;
                    blastAttack.position = gameObject.transform.position;
                    blastAttack.radius = range;
                    blastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                    blastAttack.damageType |= DamageType.Stun1s;
                    blastAttack.damageType |= DamageType.SlowOnHit;
                    blastAttack.damageType |= DamageTypeCombo.GenericPrimary;
                    blastAttack.damageColorIndex = DamageColorIndex.Default;

                    blastAttack.Fire();

                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}