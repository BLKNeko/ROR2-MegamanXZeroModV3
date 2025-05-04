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

            if(ZeroSurvivor.instance.GetApplyUpgrades())
                ReapplyUpgrades();

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

        private void ReapplyUpgrades()
        {
            if (characterBody.level >= ZeroConfig.ZeroFirstUpgradeInt.Value)
            {
                characterBody.baseJumpCount += 1;
            }

            if (characterBody.level >= ZeroConfig.ZeroSecondUpgradeInt.Value)
            {

                characterBody.skillLocator.utility.maxStock += 1;
            }

            if (characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value)
            {

                characterBody.skillLocator.special.maxStock += 1;
            }

            if (characterBody.level >= ZeroConfig.ZeroFourthUpgradeInt.Value)
            {

                characterBody.skillLocator.secondary.maxStock += 1;
                characterBody.baseJumpCount += 1;
            }

            ZeroSurvivor.instance.SetApplyUpgrades(false);
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}

