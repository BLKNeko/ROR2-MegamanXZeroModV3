using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;

        internal static GameObject Zbusterchargeprefab;

        internal static GameObject EBombPrefab;

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateZBusterChargeProjectile();
            CreateebombProjectile();

            AddProjectile(bombPrefab);
            AddProjectile(Zbusterchargeprefab);
            AddProjectile(EBombPrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }

        private static void CreateZBusterChargeProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            Zbusterchargeprefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/FMJ"), "Prefabs/Projectiles/ZBusterChargeProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXZeroMod\\MegamanXZeroMod\\MegamanXZeroMod\\MegamanXZeroMod.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            Zbusterchargeprefab.GetComponent<ProjectileDamage>().damage = 1f;
            Zbusterchargeprefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            Zbusterchargeprefab.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;

            // register it for networking
            if (Zbusterchargeprefab) PrefabAPI.RegisterNetworkPrefab(Zbusterchargeprefab);

            ProjectileController ZBCController = Zbusterchargeprefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ZBCGhost") != null) ZBCController.ghostPrefab = CreateGhostPrefab("ZBCGhost");
            ZBCController.startSound = "";
        }

        private static void CreateebombProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            EBombPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageFirewallWalkerProjectile"), "Prefabs/Projectiles/EndBombProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanXZeroMod\\MegamanXZeroMod\\MegamanXZeroMod\\MegamanXZeroMod.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            EBombPrefab.GetComponent<ProjectileDamage>().damage = 1f;
            EBombPrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            EBombPrefab.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            // register it for networking
            if (EBombPrefab) PrefabAPI.RegisterNetworkPrefab(Zbusterchargeprefab);

            ProjectileController EBController = Zbusterchargeprefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("EBGhost") != null) EBController.ghostPrefab = CreateGhostPrefab("EBGhost");
            EBController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}