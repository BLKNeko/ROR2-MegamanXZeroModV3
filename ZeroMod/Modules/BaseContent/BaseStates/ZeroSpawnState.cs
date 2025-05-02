using EntityStates;
using ZeroMod.Modules;
using ZeroMod.Survivors.Zero;
using ZeroMod.Survivors.Zero.Components;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroMod.Modules.BaseContent.BaseStates
{
    public class ZeroSpawnState : GenericCharacterSpawnState
    {
        private float duration;
        public float baseDuration = 1f;
        private Animator animator;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            

        }
        public override void OnExit()
        {

            AkSoundEngine.PostEvent(ZeroStaticValues.zReady, this.gameObject);

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

