using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using ZeroMod.Characters.Survivors.Zero.Components;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class HammerFall : BaseMeleeAttack2
    {

        public static float initialSpeedCoefficient = 8f;
        public static float finalSpeedCoefficient = 6f;
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private ChildLocator childLocator;

        Vector3 startpos;

        public GameObject lightningEffectPrefab;
        public GameObject lightningEffectPrefab2;
        public GameObject lightningEffectPrefab3;

        public override void OnEnter()
        {

            hitboxGroupName = "ZSaberHitBox";

            damageType |= DamageType.Stun1s;
            damageType |= DamageTypeCombo.GenericSpecial;
            damageCoefficient = ZeroStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.9f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.9f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            //swingSoundString = swingIndex % 2 == 0 ? XStaticValues.X_Slash3_SFX : XStaticValues.X_Slash2_SFX;

            hitSoundString = "";
            muzzleString = "SwingDown";
            playbackRateParam = "attackSpeed";
            swingEffectPrefab = ZeroAssets.ZSwordVFX;
            hitEffectPrefab = ZeroAssets.swordHitImpactEffect;

            impactSound = ZeroAssets.swordHitSoundEvent.index;

            SetHitReset(true, 3);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, baseDuration);
                characterBody.AddTimedBuff(RoR2Content.Buffs.Intangible, baseDuration);
            }


            EffectManager.SimpleMuzzleFlash(ZeroAssets.ZSwordVFX, gameObject, muzzleString, true);
            //AkSoundEngine.PostEvent(XStaticValues.X_Dash_SFX, this.gameObject);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            if (characterMotor && characterDirection && !characterMotor.isGrounded)
            {
                characterMotor.velocity = Vector3.down.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            base.PlayAnimation("FullBody, Override", "ZHammer3", "attackSpeed", baseDuration);



            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            
            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / baseDuration);


            if (characterMotor && characterDirection && !characterMotor.isGrounded)
            {
                characterMotor.velocity = Vector3.down.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / baseDuration);
            }

            if (characterMotor.isGrounded)
            {
                HammerFallAtk HFA = new HammerFallAtk();
                this.outer.SetNextState(HFA);
            }

        }

        public override void OnExit()
        {

            //base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", baseDuration);


            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}