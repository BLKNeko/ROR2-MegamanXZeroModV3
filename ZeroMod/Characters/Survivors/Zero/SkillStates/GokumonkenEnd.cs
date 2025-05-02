using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Projectile;
using System;
using ZeroMod.Characters.Survivors.Zero.Components;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class GokumonkenEnd : BaseSkillState
    {
        public static float damageCoefficient = ZeroStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.4f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private ZeroRetaliate ZR;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;

            if (NetworkServer.active)
            {
                characterBody.RemoveOldestTimedBuff(ZeroBuffs.GokumonkenBuff);
            }

            base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);
            PlayAnimation("FullBody, Override", "ZCounterEnd", "attackSpeed", this.duration);

        }

        public override void OnExit()
        {

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}