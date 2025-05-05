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
                characterBody.AddTimedBuff(ZeroBuffs.GokumonkenBuff, 5f);
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

            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;

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

            //On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ((fixedAge >= duration && isAuthority) || !base.inputBank.skill4.down)
            {

                if (canAttack)
                {
                    GokumonkenAtk GA = new GokumonkenAtk();
                    canAttack = false;
                    On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
                    outer.SetNextState(GA);
                }
                else
                {
                    GokumonkenEnd GE = new GokumonkenEnd();
                    On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
                    outer.SetNextState(GE);
                }

                return;
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            Debug.Log(self.name);
            Debug.Log(victim.name);
            Debug.Log(damageInfo.damage);
            Debug.Log(damageInfo.inflictor);

            if (NetworkServer.active)
            {

                if (victim != null && damageInfo != null && damageInfo.attacker != null)
                {
                    if (victim.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.GokumonkenBuff))
                    {

                        Vector3 direction = (damageInfo.attacker.transform.position - victim.transform.position).normalized;


                        FireProjectileInfo ZeroBusterProjectille = new FireProjectileInfo();
                        ZeroBusterProjectille.projectilePrefab = ZeroAssets.CFlasherProjectile;
                        ZeroBusterProjectille.position = victim.transform.position;
                        ZeroBusterProjectille.rotation = Util.QuaternionSafeLookRotation(direction);
                        ZeroBusterProjectille.owner = gameObject;
                        ZeroBusterProjectille.damage = (damageInfo.damage * (damageStat * 0.1f)) * damagebonus;
                        ZeroBusterProjectille.force = force;
                        ZeroBusterProjectille.crit = RollCrit();
                        ZeroBusterProjectille.damageColorIndex = DamageColorIndex.Luminous;
                        ZeroBusterProjectille.damageTypeOverride = DamageTypeCombo.GenericSpecial;

                        canAttack = true;

                        ProjectileManager.instance.FireProjectile(ZeroBusterProjectille);

                        if (characterBody.HasBuff(ZeroBuffs.BFanBuff))
                        {
                            characterBody.healthComponent.AddBarrier(((damageInfo.damage * (damageStat * 0.1f)) * damagebonus) / 5);
                        }
                        else
                        {
                            characterBody.healthComponent.AddBarrier(((damageInfo.damage * (damageStat * 0.1f)) * damagebonus) / 10);
                        }

                    }
                }

            }

            

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}