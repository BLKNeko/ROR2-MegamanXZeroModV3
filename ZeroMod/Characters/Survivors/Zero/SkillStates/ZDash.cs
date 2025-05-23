﻿using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using On.RoR2.UI;
using UnityEngine.UI;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class ZDash : BaseSkillState
    {

        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 4f;
        public static float dodgeFOV = global::EntityStates.Commando.DodgeState.dodgeFOV;

        private float rollSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        private ChildLocator childLocator;

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";

        public static float duration = 0.8f;

        public override void OnEnter()
        {

            if (characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                DashPunch D = new DashPunch();
                outer.SetNextState(D);
                return;
            }

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, RDashPos, true);
            AkSoundEngine.PostEvent(ZeroStaticValues.zDash, this.gameObject);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction.normalized;
            }

            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * initialSpeedCoefficient;
            }

            base.PlayAnimation("FullBody, Override", "DashStart", "attackSpeed", duration);

            if(characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value && !characterBody.HasBuff(ZeroBuffs.KKnuckleBuff) && ZeroConfig.enableToolTipBool.Value && base.isAuthority)
                ZeroSurvivor.instance.SetMouseIconActive(true);

            base.OnEnter();
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            

            if (base.inputBank.skill1.down && base.inputBank.skill2.down && characterBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value && !characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                Raikousen R = new Raikousen();
                outer.SetNextState(R);
                return;
            }

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterDirection) characterDirection.forward = forwardDirection;

            if (cameraTargetParams)
                cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);


            if (characterMotor && characterDirection)
            {
                characterMotor.velocity = forwardDirection.normalized * moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration);
            }

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", duration);

            //On.RoR2.UI.HUD.Awake -= HUD_Awake;

            if(base.isAuthority)
                ZeroSurvivor.instance.SetMouseIconActive(false);

            base.OnExit();
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            forwardDirection = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}