﻿using System;
using EntityStates;
using EntityStates.Merc;
using ZeroModV3.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ZeroModV3.Modules;

namespace ZeroModV3.SkillStates
{
    public class Slash4 : BaseSkillState
    {
        public static float damageCoefficient = 2.5f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 0.6f;
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
            this.earlyExitDuration = Slash4.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            //base.characterBody.isSprinting = false;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "ZSaberHitBox");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            base.PlayAnimation("Gesture, Override", "ZSlash4", "attackSpeed", this.duration);



            float dmg = Slash4.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = (Util.CheckRoll(38f, base.characterBody.master) ? DamageType.Stun1s : DamageType.Generic);
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = Slash4.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
        }

        public override void OnExit()
        {
            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public void FireAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                //Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
                Util.PlaySound(Sounds.zSlash4Voice, base.gameObject);
                Util.PlaySound(Sounds.zSlash4SFX, base.gameObject);


                // EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, muzzleString, true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * Slash4.attackRecoil, -2f * Slash4.attackRecoil, -0.5f * Slash4.attackRecoil, 0.5f * Slash4.attackRecoil);

                    Ray aimRay = base.GetAimRay();

                    //if (this.swordController && this.swordController.swordActive)
                    //{
                    //    ProjectileManager.instance.FireProjectile(Modules.Projectiles.swordBeam, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, StaticValues.beamDamageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.WeakPoint, null, StaticValues.beamSpeed);
                    // }

                    if (this.attack.Fire())
                    {
                        if (Util.CheckRoll((50f + base.characterBody.level), base.characterBody.master))
                            base.characterBody.healthComponent.AddBarrier(base.characterBody.damage / 8);
                        Util.PlaySound(EntityStates.Merc.GroundLight.hitSoundString, base.gameObject);
                        //Util.PlaySound(MinerPlugin.Sounds.Hit, base.gameObject);

                        if (!this.hasHopped)
                        {
                            if (base.characterMotor && !base.characterMotor.isGrounded)
                            {
                                base.SmallHop(base.characterMotor, Slash4.hitHopVelocity);
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

            if (base.fixedAge >= (this.duration) && base.isAuthority && base.inputBank.skill1.down)
            {

                //int index = this.swingIndex;
                // if (index == 0) index = 1;
                //else index = 0;

                Slash S1 = new Slash();
                this.outer.SetNextState(S1);

            }

            if (base.fixedAge >= this.duration && base.isAuthority && !base.inputBank.skill1.down)
            {
                base.PlayAnimation("Attack", "Empty", "attackSpeed", this.duration);
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
