﻿using EntityStates;
using ZeroMod.Survivors.Zero;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using ZeroMod.Modules;
using ZeroMod.Characters.Survivors.Zero.Components;

namespace ZeroMod.Survivors.Zero.SkillStates
{
    public class CFlasher : BaseSkillState
    {
        public static float damageCoefficient = ZeroStaticValues.CFlasherDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.3f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private GameObject muzzleEffectPrefab;

        int numberOfProjectiles = 12;
        float angleStep, angle;

        ZeroBaseComponent ZBC;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);

            angleStep = 360f / numberOfProjectiles;
            angle = 0f;

            muzzleString = "CFMuzz";
            muzzleEffectPrefab = ZeroAssets.CFlasherVFX;

            PlayAnimation("FullBody, Override", "CFlasher", "attackSpeed", (this.duration / 4));

            ZBC = GetComponent<ZeroBaseComponent>();

            ZBC.ChangeZeroHand(base.GetModelTransform(),
                base.GetModelTransform().GetComponent<CharacterModel>(),
                base.GetModelTransform().GetComponent<CharacterModel>().GetComponent<ChildLocator>(),
                base.characterBody,
                false);

        }

        public override void OnExit()
        {

            PlayAnimation("FullBody, Override", "BufferEmpty", "attackSpeed", this.duration);

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {
                    characterBody.AddSpreadBloom(0.8f);
                    EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, gameObject, muzzleString, true);

                    if (ZeroConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(ZeroStaticValues.zeroAttackVFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(ZeroStaticValues.zeroCFlasher, this.gameObject);

                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    Vector3 origin = base.characterBody.corePosition;

                    for (int i = 0; i < numberOfProjectiles; i++)
                    {
                        // Calcula a direção a partir do ângulo
                        float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
                        float dirZ = Mathf.Sin(angle * Mathf.Deg2Rad);
                        Vector3 direction = new Vector3(dirX, 0f, dirZ).normalized;

                        FireProjectileInfo projectileInfo = new FireProjectileInfo
                        {
                            projectilePrefab = ZeroAssets.CFlasherProjectile,
                            position = origin,
                            rotation = Util.QuaternionSafeLookRotation(direction),
                            owner = gameObject,
                            damage = damageCoefficient * damageStat,
                            force = force,
                            crit = RollCrit(),
                            damageColorIndex = DamageColorIndex.Luminous
                        };

                        ProjectileManager.instance.FireProjectile(projectileInfo);

                        angle += angleStep;
                    }

                    // 1 projétil para cima
                    FireProjectileInfo upwardProjectile = new FireProjectileInfo
                    {
                        projectilePrefab = ZeroAssets.CFlasherProjectile,
                        position = base.characterBody.corePosition,
                        rotation = Util.QuaternionSafeLookRotation(Vector3.up),
                        owner = gameObject,
                        damage = damageCoefficient * damageStat,
                        force = force,
                        crit = RollCrit(),
                        damageColorIndex = DamageColorIndex.Luminous
                    };
                    ProjectileManager.instance.FireProjectile(upwardProjectile);

                    // 4 inclinados
                    Vector3[] upwardOffsets = new Vector3[]
                    {
                        (Vector3.up + Vector3.forward).normalized,
                        (Vector3.up + Vector3.back).normalized,
                        (Vector3.up + Vector3.left).normalized,
                        (Vector3.up + Vector3.right).normalized
                    };

                    foreach (var direction in upwardOffsets)
                    {
                        FireProjectileInfo angledProjectile = new FireProjectileInfo
                        {
                            projectilePrefab = ZeroAssets.CFlasherProjectile,
                            position = base.characterBody.corePosition,
                            rotation = Util.QuaternionSafeLookRotation(direction),
                            owner = gameObject,
                            damage = damageCoefficient * damageStat,
                            force = force,
                            crit = RollCrit(),
                            damageColorIndex = DamageColorIndex.Luminous
                        };
                        ProjectileManager.instance.FireProjectile(angledProjectile);
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