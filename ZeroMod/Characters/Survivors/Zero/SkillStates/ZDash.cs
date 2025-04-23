using EntityStates;
using ZeroMod.Modules.BaseStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

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
            //his.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();


            //EffectManager.SimpleMuzzleFlash(XAssets.NovaStrikeVFX, base.gameObject, "NovaDashPos", true);

            //EffectManager.SpawnEffect(XAssets.NovaStrikeVFX, new EffectData
            //{
            //    origin = childLocator.FindChild("NovaDashPos").transform.position,
            //    scale = 8f,
            //    rootObject = characterBody.transform.gameObject,
            //    //rotation = Quaternion.Euler(0, 0, 180),


            //}, true);

            //impactSound = XAssets.swordHitSoundEvent.index;

            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, LDashPos, true);
            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, gameObject, RDashPos, true);
            //AkSoundEngine.PostEvent(XStaticValues.X_Dash_SFX, this.gameObject);

            //XRathalosSlashCombo2 xRathalosSlashCombo2 = new XRathalosSlashCombo2();

            //SetNextEntityState(xRathalosSlashCombo2);

            animator = GetModelAnimator();
            characterBody.SetAimTimer(0.8f);
            Ray aimRay = GetAimRay();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (isAuthority && inputBank && characterDirection)
            {
                forwardDirection = aimRay.direction;
            }

            

            Vector3 rhs = characterDirection ? characterDirection.forward : forwardDirection;
            Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            float num = Vector3.Dot(forwardDirection, rhs);
            float num2 = Vector3.Dot(forwardDirection, rhs2);

            RecalculateRollSpeed();

            if (characterMotor && characterDirection)
            {
                //characterMotor.velocity.y = 0f;
                characterMotor.velocity = (forwardDirection * rollSpeed) + Vector3.one;
            }

            Vector3 b = characterMotor ? characterMotor.velocity : Vector3.zero;
            previousPosition = transform.position - b;

            //Debug.Log("forwardDirection: " + forwardDirection);
            //Debug.Log("characterDirection: " + characterDirection);
            //Debug.Log("characterMotor: " + characterMotor);
            //Debug.Log("rollSpeed: " + rollSpeed);
            //Debug.Log("inputBank: " + inputBank);

            base.PlayAnimation("FullBody, Override", "DashStart", "attackSpeed", duration);

            base.OnEnter();
        }

        private void RecalculateRollSpeed()
        {
            rollSpeed = (moveSpeedStat * Mathf.Lerp(initialSpeedCoefficient, finalSpeedCoefficient, fixedAge / duration)) + 0.5f;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //characterMotor.velocity *= 1.5f;

            RecalculateRollSpeed();

            base.characterMotor.Motor.ForceUnground(0.1f);

            if (characterDirection) characterDirection.forward = forwardDirection;
            if (cameraTargetParams) cameraTargetParams.fovOverride = Mathf.Lerp(dodgeFOV, 60f, fixedAge / duration);

            Vector3 normalized = (transform.position - previousPosition).normalized;
            if (characterMotor && characterDirection && normalized != Vector3.zero)
            {
                Vector3 vector = normalized * rollSpeed;
                float d = Mathf.Max(Vector3.Dot(vector, forwardDirection), 0f);
                vector = forwardDirection * d;

                //if(inputBank.moveVector != Vector3.zero)
                //{
                //   vector = forwardDirection * d;
                //}
                //else
                //{
                //    //vector = Vector3.zero;
                //    // forwardDirection = Vector3.zero;
                //    float num4 = base.characterMotor.velocity.y;
                //    num4 = Mathf.MoveTowards(num4, hoverVelocity, hoverAcceleration * base.GetDeltaTime());
                //    //base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num4, base.characterMotor.velocity.z);
                //    vector = new Vector3(base.characterMotor.velocity.x, num4, base.characterMotor.velocity.z);
                //}

                //vector = forwardDirection * d;
                //vector.y = 0f;

                characterMotor.velocity = vector;
            }
            previousPosition = transform.position;

            if (isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {

            base.PlayAnimation("FullBody, Override", "DashEnd", "attackSpeed", duration);

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