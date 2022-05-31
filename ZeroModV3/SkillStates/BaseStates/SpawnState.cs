using EntityStates;
using ZeroModV3.Modules;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.SkillStates.BaseStates
{
    public class SpawnState : GenericCharacterSpawnState
    {
        private float duration;
        public float baseDuration = 1f;
        private Animator animator;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration;
            

            //base.PlayAnimation("Gesture, Override", "TBSwap", "attackSpeed", this.duration);
            base.PlayAnimation("FullBody, Override", "ZeroSpawn", "attackSpeed", this.duration);

        }
        public override void OnExit()
        {
            //Util.PlaySound(Sounds.HaseoSpawn, base.gameObject);
            Util.PlaySound(Sounds.zReady, base.gameObject);
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

