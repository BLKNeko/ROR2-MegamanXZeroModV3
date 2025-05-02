using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using ZeroMod.Characters.Survivors.Zero.Components;
using UnityEngine.Networking;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class KKnuckle : BaseSkillState
    {
        public static float damageCoefficient = ZeroStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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

        ZeroBaseComponent ZBC;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            muzzleString = "CWMuzz";

            ZBC = GetComponent<ZeroBaseComponent>();

            ZBC.ChangeZeroWeapon(base.GetModelTransform(), 
                base.GetModelTransform().GetComponent<CharacterModel>(), 
                base.GetModelTransform().GetComponent<CharacterModel>().GetComponent<ChildLocator>(), 
                3);

            ZBC.RemoveWeaponBuffs();

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);
                characterBody.AddBuff(ZeroBuffs.KKnuckleBuff);

            }

            EffectManager.SimpleMuzzleFlash(ZeroAssets.ChangeWeaponVFX, base.gameObject, muzzleString, true);

            AkSoundEngine.PostEvent(ZeroStaticValues.zeroWeaponChange, this.gameObject);

            PlayAnimation("FullBody, Override", "ChangeWeapon", "attackSpeed", duration);
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
            return InterruptPriority.Frozen;
        }
    }
}