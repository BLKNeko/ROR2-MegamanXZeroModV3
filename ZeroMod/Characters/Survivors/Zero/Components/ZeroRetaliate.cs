using RoR2.Projectile;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using ZeroMod.Survivors.Zero;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    public class ZeroRetaliate : MonoBehaviour
    {
        private CharacterBody characterBody;

        private bool CanAttack = false;

        void Awake()
        {
             On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;

        }

        public bool GetAttackBool()
        {
            return CanAttack;
        }

        public void SetAttackBool(bool b)
        {
            CanAttack = b;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {

            if (victim != null && damageInfo != null && damageInfo.attacker != null)
                return;

            Debug.Log(self.name);
            Debug.Log(victim.name);
            Debug.Log(damageInfo.damage);
            Debug.Log(damageInfo.inflictor);

            if (victim.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.GokumonkenBuff))
            {

                Debug.Log("Has BUFF");

                Vector3 direction = (damageInfo.attacker.transform.position - victim.transform.position).normalized;


                FireProjectileInfo ZeroBusterProjectille = new FireProjectileInfo();
                ZeroBusterProjectille.projectilePrefab = ZeroAssets.CFlasherProjectile;
                ZeroBusterProjectille.position = victim.transform.position;
                ZeroBusterProjectille.rotation = Util.QuaternionSafeLookRotation(direction);
                ZeroBusterProjectille.owner = victim;
                ZeroBusterProjectille.damage = victim.GetComponent<CharacterBody>().damage * 2f;
                ZeroBusterProjectille.force = 500f;
                ZeroBusterProjectille.crit = victim.GetComponent<CharacterBody>().RollCrit();
                ZeroBusterProjectille.damageColorIndex = DamageColorIndex.Luminous;



                ProjectileManager.instance.FireProjectile(ZeroBusterProjectille);

                CanAttack = true;

            }

        }

        void OnDestroy()
        {
             On.RoR2.GlobalEventManager.OnHitEnemy -= GlobalEventManager_OnHitEnemy;
        }

        private void OnTakeDamage(DamageReport damageReport)
        {
            if (damageReport == null || damageReport.attacker == null) return;

            // Verifica se o buff está ativo
            if (characterBody.HasBuff(ZeroBuffs.GokumonkenBuff))
            {
                Vector3 direction = (damageReport.attacker.transform.position - transform.position).normalized;

                FireProjectileInfo info = new FireProjectileInfo
                {
                    projectilePrefab = ZeroAssets.CFlasherProjectile,
                    position = transform.position + Vector3.up, // Ajuste se quiser sair do ombro, por exemplo
                    rotation = Quaternion.LookRotation(direction),
                    owner = gameObject,
                    damage = characterBody.damage * 1.5f, // ou use sua lógica de damageCoefficient
                    force = 100f,
                    crit = characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Luminous
                };

                ProjectileManager.instance.FireProjectile(info);
            }
        }
    }
}
