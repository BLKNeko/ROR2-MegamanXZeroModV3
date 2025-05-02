using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using ZeroMod.Survivors.Zero;
using ZeroMod.Characters.Survivors.Zero.Components;
using UnityEngine.Networking;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class GokumonkenAtk : BaseMeleeAttack2
    {

        private ZeroRetaliate ZR;

        public override void OnEnter()
        {
            hitboxGroupName = "ZSaberHitBox";

            damageType = DamageType.Generic;
            damageType = DamageTypeCombo.GenericSpecial;
            damageCoefficient = ZeroStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 0.5f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.8f;

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
            swingEffectPrefab = ZeroAssets.swordSwingEffect;
            hitEffectPrefab = ZeroAssets.swordHitImpactEffect;

            impactSound = ZeroAssets.swordHitSoundEvent.index;

            SetHitReset(true, 3);

            //ZR = GetComponent<ZeroRetaliate>();

            if (NetworkServer.active)
            {
                characterBody.RemoveOldestTimedBuff(ZeroBuffs.GokumonkenBuff);
            }

            if (ZeroConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zTakeThis, this.gameObject);
            }
            AkSoundEngine.PostEvent(ZeroStaticValues.zSlash3SFX, this.gameObject);
            AkSoundEngine.PostEvent(ZeroStaticValues.zSlash4SFX, this.gameObject);

            base.OnEnter();
        }

        protected override void PlayAttackAnimation()
        {
            //PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
            base.PlayAnimation("FullBody, Override", "ZCounterAtk", "attackSpeed", this.duration);
        }

        protected virtual void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(swingEffectPrefab, gameObject, muzzleString, true);
            EffectManager.SimpleMuzzleFlash(swingEffectPrefab, gameObject, "SwingRight", true);
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {

            //ZR.SetAttackBool(false);

            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);
            PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}