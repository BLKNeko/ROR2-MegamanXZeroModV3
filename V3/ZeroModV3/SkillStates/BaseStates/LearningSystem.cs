using EntityStates;
using ZeroModV3.Modules;
using RoR2;
using RoR2.Audio;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.SkillStates.BaseStates
{
    public class LearningSystem : GenericCharacterMain
    {
        public static bool JumpLvl3;
        public static bool JumpLvl10;
        public static bool BkupMgLvl7;
        public float baseDuration = 1f;
        private float duration;
        private Animator animator;
        public override void OnEnter()
        {
            base.OnEnter();

        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if(base.characterBody.level == 1)
            {
                JumpLvl3 = false;
                JumpLvl10 = false;
                BkupMgLvl7 = false;
            }


            if (!JumpLvl3 && base.characterBody.level == 3)
            {
                base.characterBody.inventory.GiveItem(RoR2Content.Items.Feather, 1);
                JumpLvl3 = true;
            }

            if (!BkupMgLvl7 && base.characterBody.level == 7)
            {
                base.characterBody.inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine, 1);
                BkupMgLvl7 = true;
            }

            //gain a ramdom item on level 5,10...
            //base.characterBody.inventory.GiveItem(ItemIndex.)

            if (!JumpLvl10 && base.characterBody.level == 4)
            {
                base.characterBody.inventory.GiveItem(RoR2Content.Items.Feather, 1);
                JumpLvl10 = true;
            }


            return;

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
