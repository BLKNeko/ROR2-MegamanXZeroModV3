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
    public class Gokumonken : BaseSkillState
    {
        public static float damageCoefficient = ZeroStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 5f;
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
        private bool canAttack = false;
        private float damagebonus = 1f;

        ZeroBaseComponent ZBC;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            muzzleString = "Muzzle";

            PlayAnimation("FullBody, Override", "ZCounterStart", "attackSpeed", this.duration);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 5f);
                characterBody.AddBuff(ZeroBuffs.GokumonkenBuff);
            }

            //ZR = GetComponent<ZeroRetaliate>();

            if (characterBody.HasBuff(ZeroBuffs.BFanBuff))
            {
                damagebonus = 2f;
            }
            else
            {
                damagebonus = 1f;
            }


            if (ZeroConfig.enableVoiceBool.Value)
            {
                AkSoundEngine.PostEvent(ZeroStaticValues.zGuard, this.gameObject);
            }

            ZBC = GetComponent<ZeroBaseComponent>();

            ZBC.ChangeZeroHand(base.GetModelTransform(),
                base.GetModelTransform().GetComponent<CharacterModel>(),
                base.GetModelTransform().GetComponent<CharacterModel>().GetComponent<ChildLocator>(),
                base.characterBody,
                false);


        }

        public override void OnExit()
        {

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ((fixedAge >= duration && isAuthority) || !base.inputBank.skill4.down)
            {

                if (characterBody.HasBuff(ZeroBuffs.GokumonkenAtkBuff))
                {
                    GokumonkenAtk GA = new GokumonkenAtk();
                    //ZeroSurvivor.instance.SetGKAtk(false);
                    outer.SetNextState(GA);
                }
                else
                {
                    GokumonkenEnd GE = new GokumonkenEnd();
                    outer.SetNextState(GE);
                }

                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}