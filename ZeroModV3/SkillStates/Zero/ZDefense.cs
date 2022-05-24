using EntityStates;
using ZeroModV3.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using ZeroModV3.Modules;

namespace ZeroModV3.SkillStates
{
    public class ZDefense : BaseSkillState
    {

        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ZDefense.baseDuration / this.attackSpeedStat;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            base.PlayAnimation("FullBody, Override", "ZDefenseIn", "attackSpeed", 1.8f);

            if (NetworkServer.active)
            {
                base.characterBody.AddBuff(RoR2Content.Buffs.Immune);
            }
            Util.PlaySound(Sounds.zGuard, base.gameObject);
        }

        public override void OnExit()
        {

            if (NetworkServer.active)
            {
                base.characterBody.RemoveBuff(RoR2Content.Buffs.Immune);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f);
            }

            base.OnExit();
        }

        public override void Update()
        {
            Vector3 velocity = base.characterMotor.velocity;
            velocity.x = 0f;
            velocity.z = 0f;
            velocity.y = base.characterMotor.velocity.y;
            base.characterMotor.velocity = velocity;
            
                
            
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.SetAimTimer(2f);

            if (base.fixedAge >= this.duration && base.isAuthority && !base.inputBank.skill2.down)
            {
                base.PlayAnimation("FullBody, Override", "ZDefenseOut", "attackSpeed", 1.8f);
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}