﻿using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ZeroMod.Characters.Survivors.Zero.Components;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class Ryuuenjin : BaseMeleeAttack2
    {

        private float upwardForce = 2000f;
        private float yVelocityCurve = 20f;
        private float moveSpeedBonusCoefficient = 4f;

        ZeroBaseComponent ZBC;

        public override void OnEnter()
        {
            hitboxGroupName = "ZSaberHitBox";

            damageType |= DamageType.IgniteOnHit;
            damageType |= DamageTypeCombo.GenericSpecial;
            damageCoefficient = ZeroStaticValues.RyuuenjinDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 1000f;
            bonusForce = Vector3.up * upwardForce;
            baseDuration = 1.1f;            

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.01f;
            attackEndPercentTime = 0.99f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.99f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            hitSoundString = "";
            muzzleString = "RyuMuzz";
            playbackRateParam = "Slash.playbackRate";
            swingEffectPrefab = ZeroAssets.RyuenjinVFX;
            //hitEffectPrefab = XAssets.swordHitImpactEffect;

            //impactSound = XAssets.swordHitSoundEvent.index;

            //XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            //SetHitReset(true, 8);

            EffectManager.SimpleMuzzleFlash(ZeroAssets.RyuenjinVFX, gameObject, muzzleString, true);

            if (ZeroConfig.enableVoiceBool.Value && characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zeroShoryukenVFX, this.gameObject);
            }
            else if (ZeroConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zeroAttackVFX, this.gameObject);
            }
            AkSoundEngine.PostEvent(ZeroStaticValues.zEnkoujinSFX, this.gameObject);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, duration * 3);
            }

            if (characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value && ZeroConfig.enableToolTipBool.Value && base.isAuthority)
                ZeroSurvivor.instance.SetMouseIconActive(true);

            ZBC = GetComponent<ZeroBaseComponent>();

            ZBC.ChangeZeroHand(base.GetModelTransform(),
                base.GetModelTransform().GetComponent<CharacterModel>(),
                base.GetModelTransform().GetComponent<CharacterModel>().GetComponent<ChildLocator>(),
                base.characterBody,
                false);

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "RyuuenjinStart", "attackSpeed", this.duration);
        }

        protected override void PlaySwingEffect()
        {
            //base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;
            if (base.isAuthority)
            {

                base.characterMotor.Motor.ForceUnground(0.1f);

                if (base.inputBank.skill1.down && base.inputBank.skill2.down && characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value && GetNextEntityState() == null)
                {

                    if (characterBody.HasBuff(ZeroBuffs.TBreakerBuff))
                    {
                        HammerFall HF = new HammerFall();
                        SetNextEntityState(HF);
                        return;
                    }
                    else
                    {
                        IceFurySlash I = new IceFurySlash();
                        SetNextEntityState(I);
                        return;
                    }

                    
                }

                if (!inHitPause)
                {
                    if (base.characterMotor && base.characterDirection)
                    {
                        Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(moveSpeedBonusCoefficient, 0f, base.age / this.duration);
                        velocity.y = Mathf.Lerp(yVelocityCurve, yVelocityCurve/2, fixedAge / duration);
                        base.characterMotor.velocity = velocity;
                    }
                }
                else
                {
                    base.characterMotor.velocity = Vector3.zero;
                }

            }
        }


 

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "RyuuenjinEnd", "attackSpeed", this.duration);
            //base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            if(base.isAuthority)
                ZeroSurvivor.instance.SetMouseIconActive(false);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}