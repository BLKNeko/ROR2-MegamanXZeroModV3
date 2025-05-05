using EntityStates;
using ZeroMod.Modules;
using ZeroMod.Survivors.Zero;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroMod.Modules.BaseContent.BaseStates
{
    public class ZeroDeathState : GenericCharacterDeath
    {
        private float duration;
        public float baseDuration = 0.5f;
        private Animator animator;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;


            //base.PlayAnimation("FullBody, Override", "Deleted", "attackSpeed", this.duration);

            if (ZeroConfig.enableVoiceBool.Value)
            {
                if (ZeroConfig.x4VoicesBool.Value)
                    AkSoundEngine.PostEvent(ZeroStaticValues.zeroX4Death, this.gameObject);
                else
                    AkSoundEngine.PostEvent(ZeroStaticValues.zDeath, this.gameObject);
            }


            EffectManager.SimpleMuzzleFlash(ZeroAssets.ZDeathVFX, base.gameObject, "CorePosition", true);

            modelTransform = GetModelTransform();
            if ((bool)modelTransform)
            {
                animator = modelTransform.GetComponent<Animator>();
                characterModel = modelTransform.GetComponent<CharacterModel>();
                hurtboxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }

            if ((bool)characterModel)
            {
                characterModel.invisibilityCount++;
            }





        }

        public override void OnExit()
        {

            //if ((bool)characterModel)
            //{
            //    characterModel.invisibilityCount--;
            //}

            base.PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }

    }
}
