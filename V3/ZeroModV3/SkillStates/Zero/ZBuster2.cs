using EntityStates;
using ZeroModV3.SkillStates.BaseStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ZeroModV3.Modules;

namespace ZeroModV3.SkillStates
{
    public class ZBuster2 : BaseSkillState
    {
        public float damageCoefficient = 2.4f;
        public float baseDuration = 0.4f;
        public float recoil = 0.4f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");

        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.25f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Muzzle";


            base.PlayAnimation("Attack", "ZeroTestShoot", "FireArrow.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireNZB()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                //EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);


                if (base.isAuthority)
                {
                    Util.PlaySound(Sounds.zBuster2, base.gameObject);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.Zbusterchargeprefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    if (base.characterBody.level >= 10)
                    {
                        Util.PlaySound(Sounds.zBuster2, base.gameObject);
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.Zbusterchargeprefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
            {
                FireNZB();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
