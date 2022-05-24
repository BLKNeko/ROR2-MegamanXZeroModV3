using EntityStates;
using ZeroModV3.SkillStates.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using ZeroModV3.Modules;

namespace ZeroModV3.SkillStates
{
    public class UpSlash : BaseSkillState
    {
        public static float damageCoefficient = 2f;
        public static float buffDamageCoefficient = 1f;
        public float baseDuration = 1f;
        public static float attackRecoil = 0.5f;
        public static float hitHopVelocity = 5.5f;
        public static float baseEarlyExit = 0.25f;
        public int swingIndex;

        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/ImpactMercAssaulter");

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
            this.duration = this.baseDuration;
            this.earlyExitDuration = UpSlash.baseEarlyExit / this.attackSpeedStat;
            this.hasFired = false;
            this.animator = base.GetModelAnimator();
            //this.swordController = base.GetComponent<PaladinSwordController>();
            base.StartAimMode(0.5f + this.duration, false);
            base.characterBody.isSprinting = false;

            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "ZSword");
            }

            //if (this.swingIndex == 0) base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            //else base.PlayAnimation("Gesture, Override", "ZSlash1", "FireArrow.playbackRate", this.duration);
            base.PlayAnimation("FullBody, Override", "UpSlash", "attackSpeed", this.duration);

            Util.PlaySound(Sounds.zTakeThis, base.gameObject);
            Util.PlaySound(Sounds.zEnkoujinSFX, base.gameObject);

            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "zSword", true);



            float dmg = UpSlash.damageCoefficient;
            //if (this.swordController && this.swordController.swordActive) dmg = Slash.buffDamageCoefficient;

            this.attack = new OverlapAttack();
            this.attack.damageType = DamageType.Stun1s;
            this.attack.attacker = base.gameObject;
            this.attack.inflictor = base.gameObject;
            this.attack.teamIndex = base.GetTeam();
            this.attack.damage = dmg * this.damageStat;
            this.attack.procCoefficient = 1;
            this.attack.hitEffectPrefab = UpSlash.hitEffectPrefab;
            this.attack.forceVector = Vector3.zero;
            this.attack.pushAwayForce = 1f;
            this.attack.hitBoxGroup = hitBoxGroup;
            this.attack.isCrit = base.RollCrit();
           // Util.PlayScaledSound(EntityStates.Merc.GroundLight.comboAttackSoundString, base.gameObject, 0.5f);
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


                //string muzzleString = null;
                //if (this.swingIndex == 0) muzzleString = "SwingLeft";
                //else muzzleString = "SwingRight";

                // EffectManager.SimpleMuzzleFlash(Modules.Assets.swordSwing, base.gameObject, muzzleString, true);

                //EffectManager.SimpleMuzzleFlash(ExampleSurvivor.Assets.fireeffect2, base.gameObject, "Sword", true);

                if (base.isAuthority)
                {
                    base.AddRecoil(-1f * UpSlash.attackRecoil, -2f * UpSlash.attackRecoil, -0.5f * UpSlash.attackRecoil, 0.5f * UpSlash.attackRecoil);

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
                                base.SmallHop(base.characterMotor, UpSlash.hitHopVelocity);
                            }

                            this.hasHopped = true;
                        }

                        if (!this.inHitPause)
                        {
                            this.hitStopCachedState = base.CreateHitStopCachedState(base.characterMotor, this.animator, "attackSpeed");
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

            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "zSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "zSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "ZSword", true);
            EffectManager.SimpleMuzzleFlash(Modules.Assets.fireeffect, base.gameObject, "zSword", true);

            this.hitPauseTimer -= Time.fixedDeltaTime;

            if (this.hitPauseTimer <= 0f && this.inHitPause)
            {
                base.ConsumeHitStopCachedState(this.hitStopCachedState, base.characterMotor, this.animator);
                this.inHitPause = false;
            }

            if (!this.inHitPause)
            {
                this.stopwatch += Time.fixedDeltaTime;
                //Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(6f, 6f, base.age / this.duration);
                Vector3 velocity;
                base.characterMotor.Motor.ForceUnground();
                velocity = base.characterMotor.velocity;
                velocity.x = 0f;
                velocity.y += 2.4f;
                velocity.z = 0f;
                base.characterMotor.velocity = velocity;
            }
            else
            {
                if (base.characterMotor) base.characterMotor.velocity = Vector3.zero;
                if (this.animator) this.animator.SetFloat("attackSpeed", 1f);
            }

            if (this.stopwatch >= this.duration * 0.2f)
            {
                this.FireAttack();
            }



            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                //base.cameraTargetParams.fovOverride = Mathf.Lerp(DodgeState.dodgeFOV, 60f, base.fixedAge / this.duration);
                base.cameraTargetParams.fovOverride = -2.5f;
                if (NetworkServer.active)
                    base.characterBody.AddBuff(RoR2Content.Buffs.Immune);


                DownSlash DS = new DownSlash();
                RasetsusenCombo RSC = new RasetsusenCombo();
                //this.outer.SetNextStateToMain();
                if (base.inputBank.skill3.down && base.characterBody.level >= 4)
                    this.outer.SetNextState(RSC);
                else
                {
                    this.outer.SetNextState(DS);
                }
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