﻿using EntityStates;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.SkillStates
{
    class GroundImpact : BaseSkillState
    {
        public static float damageCoefficient = 8f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 0.8f;
        public static float attackRecoil = 0.5f;
        public static float hitHopVelocity = 5.5f;
        public static float baseEarlyExit = 0.25f;
        public int swingIndex;

        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactMercSwing");

        private float earlyExitDuration;
        private float duration;
        private bool hasFired;
        private float hitPauseTimer;
        private OverlapAttack attack;
        private bool inHitPause;
        private bool hasHopped;
        private float stopwatch;
        private Animator animator;
        private BaseState.HitStopCachedState hitStopCachedState;
        //private PaladinSwordController swordController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.earlyExitDuration = GroundImpact.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            //base.characterBody.isSprinting = false;

           // Util.PlaySound(Sounds.vileAttack, base.gameObject);

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "GroundImpact");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            base.PlayAnimation("FullBody, Override", "ZeroHitGround", "attackSpeed", this.duration);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.GroundImpactFire, base.gameObject, "GroundImpact", true);




            float dmg = GroundImpact.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = (Util.CheckRoll(75f, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic);
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = GroundImpact.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 8f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                //this.hasFired = true;
                //Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                //Util.PlaySound(Sounds.zSlash1Voice, base.gameObject);
                //Util.PlaySound(Sounds.zSlash1SFX, base.gameObject);

                //string muzzleString = null;
                // if (this.swingIndex == 0) muzzleString = "SwingLeft";
                //else muzzleString = "SwingRight";


                // EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, muzzleString, true);
                EffectManager.SimpleMuzzleFlash(EntityStates.Merc.GroundLight.comboSwingEffectPrefab, base.gameObject, "SwingLeft", true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * GroundImpact.attackRecoil, -2f * GroundImpact.attackRecoil, -0.5f * GroundImpact.attackRecoil, 0.5f * GroundImpact.attackRecoil);

                    Ray aimRay = base.GetAimRay();

                    //if (this.swordController && this.swordController.swordActive)
                    //{
                    //    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                    // }

                    if (this.attack.Fire())
                    {
                        Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                        //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);

                        if (!this.hasHopped)
                        {
                            if (base.characterMotor && !base.characterMotor.isGrounded)
                            {
                                base.SmallHop(base.characterMotor, GroundImpact.hitHopVelocity);
                            }

                            this.hasHopped = true;
                        }

                        if (!this.inHitPause)
                        {
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "FireArrow.playbackRate");
                            this.hitPauseTimer = (0.6f * EntityStates.Merc.GroundLight.hitPauseDuration) / this.attackSpeedStat;
                            this.inHitPause = true;
                        }
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("FireArrow.playbackRate", 1f);
            }

            if (this.stopwatch >= this.duration * 0.2f)
            {
                this.FireAttack();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.swingIndex);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.swingIndex = reader.ReadInt32();
        }
    }
}
