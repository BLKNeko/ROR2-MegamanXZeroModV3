﻿using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using ZeroMod.Survivors.Zero;
using EntityStates.BrotherMonster;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZSSlashCombo : BaseMeleeAttackZP
    {

        public override void OnEnter()
        {
            hitboxGroupName = "ZSaberHitBox";

            damageType = DamageType.Generic;
            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = ZeroStaticValues.ZSaber1DamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.1f;
            attackEndPercentTime = 0.9f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.8f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 5f;

            //swingSoundString = swingIndex % 2 == 0 ? XStaticValues.X_Slash3_SFX : XStaticValues.X_Slash2_SFX;

            hitSoundString = "";
            //muzzleString = "SwordMuzzPos";
            muzzleString = "SwingLeft";
            playbackRateParam = "attackSpeed";
            hitEffectPrefab = ZeroAssets.swordHitImpactEffect;

            impactSound = ZeroAssets.swordHitSoundEvent.index;

            if(base.characterBody.skinIndex == 0)
                swingEffectPrefab = ZeroAssets.ZSwordVFX;
            if (base.characterBody.skinIndex == 1)
                swingEffectPrefab = ZeroAssets.BZSwordVFX;
            if (base.characterBody.skinIndex == 2)
                swingEffectPrefab = ZeroAssets.NZSwordVFX;
            if (base.characterBody.skinIndex == 3)
                swingEffectPrefab = ZeroAssets.ZSwordVFX;


            //SetHitReset(true, 3);

            ZSSlashCombo2 ZSS2 = new ZSSlashCombo2();

            SetNextEntityState(ZSS2);

            if (ZeroConfig.enableVoiceBool.Value)
            {
                if (ZeroConfig.x4VoicesBool.Value)
                    AkSoundEngine.PostEvent(ZeroStaticValues.zeroX4Hu, this.gameObject);  
                else
                    AkSoundEngine.PostEvent(ZeroStaticValues.zSlash1Voice, this.gameObject);
            }

            if (characterBody.HasBuff(ZeroBuffs.TBreakerBuff))
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zeroHunmmerSFX, this.gameObject);
            }
            else if (characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zeroKnuckeSFX, this.gameObject);
            }
            else
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zSlash1SFX, this.gameObject);
            }



            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("Gesture, Override", "ZSSlash1", "attackSpeed", this.duration);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(swingEffectPrefab, gameObject, muzzleString, true);
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {

            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}