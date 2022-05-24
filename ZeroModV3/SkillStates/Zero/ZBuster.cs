using EntityStates;
using ZeroModV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroModV3.SkillStates
{
    public class ZBuster : BaseSkillState
    {
        public float damageCoefficient = 1.7f;
        public float baseDuration = 0.6f;
        public float recoil = 0.7f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerCommandoShotgun");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/FMJImpact");



        public float chargeTime = 0f;
        public float LastChargeTime = 0f;
        public bool chargeFullSFX = false;
        public bool hasTime = false;
        public bool hasCharged = false;
        public bool chargingSFX = false;
        public bool shootCharged = false;


        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.25f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Muzzle";
            base.PlayAnimation("Attack", "ZeroTestShoot", "FireArrow.playbackRate", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireZB()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                shootCharged = false;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
               // EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {
                    Util.PlaySound(Sounds.zBuster1, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(MegamanX.MegamanXMod.XShot, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.4f,
                        damage = damageCoefficient * this.damageStat,
                        force = 1f,
                        tracerEffectPrefab = ZBuster.tracerEffectPrefab,
                        muzzleName = muzzleString,
                        hitEffectPrefab = ZBuster.hitEffectPrefab,
                        damageType = DamageType.Shock5s,
                        radius = 1f,
                        isCrit = Util.CheckRoll(this.critStat, base.characterBody.master)
                    }.Fire();
                }
            }
        }

        private void FireZBC()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                shootCharged = true;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                //EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol.effectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {
                    Util.PlaySound(Sounds.zBuster1, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(MegamanX.MegamanXMod.XShot, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.4f,
                        damage = (damageCoefficient * 1.5f) * this.damageStat,
                        force = 1f,
                        tracerEffectPrefab = ZBuster.tracerEffectPrefab,
                        muzzleName = muzzleString,
                        hitEffectPrefab = ZBuster.hitEffectPrefab,
                        damageType = DamageType.Shock5s,
                        radius = 1f,
                        isCrit = Util.CheckRoll(this.critStat, base.characterBody.master)
                    }.Fire();
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (base.inputBank.skill2.down)
            {
                chargeTime += Time.deltaTime;

                if (chargeTime > 0.5f && chargeTime <= 1.8f && chargingSFX == false)
                {
                    Util.PlaySound(Sounds.zCharging, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1C, base.gameObject, "Center", true);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1W, base.gameObject, "Center", true);
                    chargingSFX = true;
                }

                if (chargeTime >= 1.8f && chargeFullSFX == false)
                {
                    Util.PlaySound(Sounds.zFullCharge, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    chargeFullSFX = true;
                    LastChargeTime = chargeTime;
                }

                if ((chargeTime - LastChargeTime) >= 0.68f && chargeFullSFX == true)
                {
                    Util.PlaySound(Sounds.zFullCharge, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    LastChargeTime = chargeTime;
                }
            }

            if (!base.inputBank.skill2.down)
            {
                if (chargeTime >= 1.8f)
                    hasCharged = true;
                chargingSFX = false;
                hasTime = true;
            }

        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill2.down) && hasCharged == true && hasTime == true)
            {
                FireZBC();
            }

            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill2.down) && hasCharged == false && hasTime == true)
            {
                FireZB();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;
                chargeTime = 0f;
                ZBuster2 ZB2 = new ZBuster2();
                if (shootCharged)
                    this.outer.SetNextState(ZB2);
                else
                    this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

    }
}
