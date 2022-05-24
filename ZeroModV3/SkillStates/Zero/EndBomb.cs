using EntityStates;
using ZeroModV3.SkillStates.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.SkillStates
{
    public class EndBomb : BaseSkillState
    {
        public float damageCoefficient = 2f;
        public float baseDuration = 0.05f;
        public float recoil = 1f;
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



            base.PlayAnimation("FullBody, Override", "ZeroHitGound", "attackSpeed", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireBomb()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    //ProjectileManager.instance.FireProjectile(ExampleSurvivor.MegamanXZeroMod.eBomb, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //EffectManager.SimpleMuzzleFlash(ExampleSurvivor.Assets.fireeffect, base.gameObject, "Sword", false);


            if (base.fixedAge >= this.fireDuration)
            {
                FireBomb();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                base.cameraTargetParams.fovOverride = -1f;
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
