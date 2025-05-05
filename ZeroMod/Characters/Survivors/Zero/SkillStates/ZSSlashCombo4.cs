using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using ZeroMod.Survivors.Zero;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZSSlashCombo4 : BaseMeleeAttackZP
    {

        public override void OnEnter()
        {
            hitboxGroupName = "ZSaberHitBox";

            damageType = DamageType.Generic;
            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = ZeroStaticValues.ZSaber4DamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f;

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
            //muzzleString = "SwordMuzzPos";
            muzzleString = "SwingRight";
            playbackRateParam = "attackSpeed";
            swingEffectPrefab = ZeroAssets.ZSwordVFX;
            hitEffectPrefab = ZeroAssets.swordHitImpactEffect;

            impactSound = ZeroAssets.swordHitSoundEvent.index;

            SetHitReset(true, 3);

            ZSSlashCombo ZSS = new ZSSlashCombo();
            ZSSlashCombo5 ZSS5 = new ZSSlashCombo5();

            if (base.characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value)
                SetNextEntityState(ZSS5);
            else
                SetNextEntityState(ZSS);

            if (ZeroConfig.enableVoiceBool.Value)
            {
                if (ZeroConfig.x4VoicesBool.Value)
                    AkSoundEngine.PostEvent(ZeroStaticValues.zeroX4Ha, this.gameObject);
                else
                    AkSoundEngine.PostEvent(ZeroStaticValues.zSlash4Voice, this.gameObject);
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
                AkSoundEngine.PostEvent(ZeroStaticValues.zSlash4SFX, this.gameObject);
            }

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "ZSSlash4", "attackSpeed", this.duration);
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

            base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}