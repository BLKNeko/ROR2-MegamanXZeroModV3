﻿using RoR2;
using UnityEngine;
using ZeroMod.Modules;
using System;
using RoR2.Projectile;
using ZeroMod.Characters.Survivors.Zero.Components;

namespace ZeroMod.Survivors.Zero
{
    public static class ZeroAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;


        public static GameObject raikousenVFX;
        public static GameObject raikousen2VFX;
        public static GameObject raikousen3VFX;


        public static GameObject ZSwordVFX;
        public static GameObject BZSwordVFX;
        public static GameObject NZSwordVFX;


        public static GameObject CFlasherVFX;
        public static GameObject ChangeWeaponVFX;
        public static GameObject SpinKickVFX;
        public static GameObject IceDragonRiseVFX;
        public static GameObject RyuenjinVFX;
        public static GameObject EnkoujinVFX;
        public static GameObject IceFurySlashVFX;
        public static GameObject HammerAtkVFX;

        public static GameObject ZDeathVFX;

        public static GameObject ZeroEmotePrefab;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        public static Sprite ZMouseIcon;

        public static Sprite ZPassiveIcon;

        public static Sprite ZeroEmoteIcon;

        public static Sprite ZSaberIcon;
        public static Sprite TBreakerIcon;
        public static Sprite BFanIcon;
        public static Sprite KKnuckleIcon;
        public static Sprite SigmaBladeIcon;


        public static Sprite ZSaberSkillIcon;
        public static Sprite ZBusterSkillIcon;
        public static Sprite GokumonkenSkillIcon;
        public static Sprite CFlasherSkillIcon;
        public static Sprite RyuenjinSkillIcon;
        public static Sprite IceDragonRiseSkillIcon;
        public static Sprite ZDashSkillIcon;

        public static Sprite ZeroSkinIcon;
        public static Sprite BZeroSkinIcon;
        public static Sprite NZeroSkinIcon;
        public static Sprite VZeroSkinIcon;


        //projectiles
        public static GameObject bombProjectilePrefab;

        public static GameObject CFlasherProjectile;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();
            ConfigureZeroRaikousen();


            ZeroEmotePrefab = _assetBundle.LoadAsset<GameObject>("ZeroEmoteSkeleton");


            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");


            //raikousenVFX = _assetBundle.LoadEffect("ElectricLine");

            ZSwordVFX = _assetBundle.LoadEffect("ZSaberSwing", true);
            BZSwordVFX = _assetBundle.LoadEffect("BZSaberSwing", true);
            NZSwordVFX = _assetBundle.LoadEffect("NZSaberSwing", true);

            CFlasherVFX = _assetBundle.LoadEffect("CFlasherVFX", true);
            ChangeWeaponVFX = _assetBundle.LoadEffect("ChangeWeaponVFX", true);
            SpinKickVFX = _assetBundle.LoadEffect("SpinKickVFX", true);
            IceDragonRiseVFX = _assetBundle.LoadEffect("IceDragonRise2VFX", true);
            IceFurySlashVFX = _assetBundle.LoadEffect("IceFurySlashVFX", true);
            RyuenjinVFX = _assetBundle.LoadEffect("RyuenjinVFX", true);
            EnkoujinVFX = _assetBundle.LoadEffect("EnkoujinVFX", true);
            HammerAtkVFX = _assetBundle.LoadEffect("HammerAtkVFX", false);

            ZDeathVFX = _assetBundle.LoadEffect("ZDeathEffect", false);


            ZMouseIcon = _assetBundle.LoadAsset<Sprite>("MouseIcon");

            ZPassiveIcon = _assetBundle.LoadAsset<Sprite>("ZPassiveSkillIcon");

            ZeroEmoteIcon = _assetBundle.LoadAsset<Sprite>("ZeroEmoteIcon");


            ZSaberIcon = _assetBundle.LoadAsset<Sprite>("ZSaberIcon");
            TBreakerIcon = _assetBundle.LoadAsset<Sprite>("TBreakerIcon");
            BFanIcon = _assetBundle.LoadAsset<Sprite>("BFanIcon");
            KKnuckleIcon = _assetBundle.LoadAsset<Sprite>("KKnuckleIcon");
            SigmaBladeIcon = _assetBundle.LoadAsset<Sprite>("SigmaBladeIcon");


            ZSaberSkillIcon = _assetBundle.LoadAsset<Sprite>("ZSaberSkillIcon");
            ZBusterSkillIcon = _assetBundle.LoadAsset<Sprite>("ZBusterSkillIcon");
            GokumonkenSkillIcon = _assetBundle.LoadAsset<Sprite>("GokumonkenSkillIcon");
            CFlasherSkillIcon = _assetBundle.LoadAsset<Sprite>("CFlasherSkillIcon");
            RyuenjinSkillIcon = _assetBundle.LoadAsset<Sprite>("RyuuenjinSkillIcon");
            IceDragonRiseSkillIcon = _assetBundle.LoadAsset<Sprite>("IceDragonRiseSkillIcon");
            ZDashSkillIcon = _assetBundle.LoadAsset<Sprite>("ZDashSkillIcon");

            ZeroSkinIcon = _assetBundle.LoadAsset<Sprite>("ZeroSkinIcon");
            BZeroSkinIcon = _assetBundle.LoadAsset<Sprite>("ZeroBSkinIcon");
            NZeroSkinIcon = _assetBundle.LoadAsset<Sprite>("ZeroNSkinIcon");
            VZeroSkinIcon = _assetBundle.LoadAsset<Sprite>("ZeroViaSkinIcon");


        }

        private static void ConfigureZeroRaikousen()
        {
            raikousenVFX = _assetBundle.LoadEffect("ElectricLine");

            if (!raikousenVFX)
                return;

            raikousenVFX.GetComponent<DestroyOnTimer>().duration = 1.25f;
            raikousenVFX.AddComponent<ElectricTrailFollow>();
            raikousenVFX.GetComponent<EffectComponent>().parentToReferencedTransform = false;
            raikousenVFX.GetComponent<EffectComponent>().positionAtReferencedTransform = false;
            raikousenVFX.GetComponent<EffectComponent>().didResolveReferencedChildTransform = false;
            raikousenVFX.GetComponent<VFXAttributes>().DoNotPool = true;

            // ---------------

            raikousen2VFX = _assetBundle.LoadEffect("ElectricLine2");

            if (!raikousen2VFX)
                return;

            raikousen2VFX.GetComponent<DestroyOnTimer>().duration = 1.25f;
            raikousen2VFX.AddComponent<ElectricTrailFollow>();
            raikousen2VFX.GetComponent<EffectComponent>().parentToReferencedTransform = false;
            raikousen2VFX.GetComponent<EffectComponent>().positionAtReferencedTransform = false;
            raikousen2VFX.GetComponent<EffectComponent>().didResolveReferencedChildTransform = false;
            raikousen2VFX.GetComponent<VFXAttributes>().DoNotPool = true;

            // ---------------

            raikousen3VFX = _assetBundle.LoadEffect("ElectricLine3");

            if (!raikousen3VFX)
                return;

            raikousen3VFX.GetComponent<DestroyOnTimer>().duration = 1.25f;
            raikousen3VFX.AddComponent<ElectricTrailFollow>();
            raikousen3VFX.GetComponent<EffectComponent>().parentToReferencedTransform = false;
            raikousen3VFX.GetComponent<EffectComponent>().positionAtReferencedTransform = false;
            raikousen3VFX.GetComponent<EffectComponent>().didResolveReferencedChildTransform = false;
            raikousen3VFX.GetComponent<VFXAttributes>().DoNotPool = true;


        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            CreateCFlasherProjectile();


            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(CFlasherProjectile);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "";
        }

        private static void CreateCFlasherProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            CFlasherProjectile = Asset.CloneProjectilePrefab("FMJ", "CFlasherProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(CFlasherProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion CFlasherExplosion = CFlasherProjectile.AddComponent<ProjectileImpactExplosion>();

            CFlasherExplosion.blastRadius = 10f;
            CFlasherExplosion.blastDamageCoefficient = 1f;
            CFlasherExplosion.falloffModel = BlastAttack.FalloffModel.None;
            CFlasherExplosion.destroyOnEnemy = true;
            CFlasherExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            CFlasherExplosion.timerAfterImpact = true;
            CFlasherExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            CFlasherProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            CFlasherProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            CFlasherProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            CFlasherProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            //if (xBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xBusterChargeProjectile);


            ProjectileController CFlasherController = CFlasherProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("XBusterChargeProjectille") != null)
            //    XBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XBusterChargeProjectille");

            CFlasherController.startSound = "";
        }

        #endregion projectiles
    }
}
