﻿using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using ZeroMod.Modules;
using static Rewired.Utils.Classes.Utility.ObjectInstanceTracker;
using UnityEngine.Networking;
using System.Xml.Linq;
using static RoR2.OutlineHighlight;
using ZeroMod.Survivors.Zero;
using EntityStates.GameOver;

namespace ZeroMod.Characters.Survivors.Zero.Components
{
    internal class ZeroBaseComponent : MonoBehaviour
    {

        private Transform ZmodelTransform;

        private Animator ZAnim;

        private HealthComponent ZHealth;

        private CharacterBody ZBody;

        private bool isWeak;

        private bool giveExtraLife { get; set; }
        private bool HasUsedExtraLife { get; set; }

        private float minHpWeak, initialStoreTime;

        private ChildLocator childLocator;

        private FootstepHandler footstepHandler;

        private Vector3 RaikousenStartPos {  get; set; }

        private float BarrierCooldown = 0f;

        private float bFanMaxBarrier, bFanBarrierMult;

        

        private void Start()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            if (ZBody == null)
            {
                ZBody = GetComponent<CharacterBody>();
            }

            ZHealth = ZBody.GetComponent<HealthComponent>();

            ZmodelTransform = ZBody.transform;

            ZAnim = ZBody.characterDirection.modelAnimator;

            minHpWeak = 0.45f;

            giveExtraLife = false;


            footstepHandler = ZBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<FootstepHandler>();

            //Debug.Log("footstepHandler: " + footstepHandler);

            switch (ZeroConfig.enableZFootstep.Value)
            {
                case 0:
                    footstepHandler.baseFootstepString = "";
                    footstepHandler.sprintFootstepOverrideString = "";
                    break;
                case 1:
                    footstepHandler.baseFootstepString = "Play_Z_Footstep_SFX";
                    footstepHandler.sprintFootstepOverrideString = "Play_Z_Footstep_SFX";
                    break;
                case 2:
                    footstepHandler.baseFootstepString = "Play_Z_Footstep_X8_SFX";
                    footstepHandler.sprintFootstepOverrideString = "Play_Z_Footstep_X8_SFX";
                    break;
                default:
                    footstepHandler.baseFootstepString = "";
                    footstepHandler.sprintFootstepOverrideString = "";
                    break;
            }


        }


        private void Awake()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            //XAnim = this.GetComponent<Animator>();
            //XHealth = this.GetComponent<HealthComponent>();

        }


        void FixedUpdate()
        {
            //Debug.Log("isWeak: " + isWeak);
            //Debug.Log("XAnim: " + XAnim);
            //Debug.Log("XHealth: " + XHealth);
            //Debug.Log("XBody: " + XBody);

            //Debug.Log("Xbody: " + XBody.transform.localPosition);
            //Debug.Log("Xhurtbox: " + XBody.mainHurtBox.transform.localPosition);
            //Debug.Log("Xmodel: " + XBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().transform.localPosition);

            ZeroAnimBool();
            BFanBarrier();
        }

        public CharacterBody GetXBody()
        {
            return ZBody;
        }

        private void ZeroAnimBool()
        {
            isWeak = ZHealth.combinedHealthFraction < minHpWeak;

            ZAnim.SetBool("isWeak", isWeak);

            if(ZeroConfig.ChungLeePoseBool.Value)
            {
                ZAnim.SetBool("isBFan", ZBody.HasBuff(ZeroBuffs.BFanBuff));
            }

            

        }

        private void BFanBarrier()
        {
            if (ZBody.HasBuff(ZeroBuffs.BFanBuff))
            {

                //Debug.Log("Health 0.3 " + ZHealth.health * 0.3f);
                //Debug.Log("ZHealth.barrier " + ZHealth.barrier);
                //Debug.Log("BarrierCooldown " + BarrierCooldown);

                if(ZBody.level >= ZeroConfig.ZeroFourthUpgradeInt.Value)
                {
                    bFanMaxBarrier = 0.5f;
                    bFanBarrierMult = 0.2f;
                }
                else
                {
                    bFanMaxBarrier = 0.3f;
                    bFanBarrierMult = 0.1f;
                }

                if(ZHealth.barrier <= ZHealth.fullCombinedHealth * bFanMaxBarrier && BarrierCooldown >= 2f)
                {
                    ZHealth.AddBarrier(ZHealth.fullCombinedHealth * bFanBarrierMult);
                    BarrierCooldown = 0f;
                }
                BarrierCooldown += Time.fixedDeltaTime;
                
                

            }
        }

        public void ChangeZeroWeapon(Transform modelTransform, CharacterModel characterModel, ChildLocator childLocator, int id)
        {
            if (modelTransform)
            {

                if (characterModel)
                {

                    childLocator.FindChildGameObject("ZSaberMesh").SetActive(false);
                    childLocator.FindChildGameObject("TBreaker").SetActive(false);
                    childLocator.FindChildGameObject("BFan").SetActive(false);
                    childLocator.FindChildGameObject("BFan2").SetActive(false);
                    childLocator.FindChildGameObject("KKnuckle").SetActive(false);
                    childLocator.FindChildGameObject("KKnuckle2").SetActive(false);
                    childLocator.FindChildGameObject("SigmaBlade").SetActive(false);

                    switch (id)
                    {
                        case 0:
                            childLocator.FindChildGameObject("ZSaberMesh").SetActive(true);
                        break;

                        case 1:
                            childLocator.FindChildGameObject("TBreaker").SetActive(true);
                        break;

                        case 2:
                            childLocator.FindChildGameObject("BFan").SetActive(true);
                            childLocator.FindChildGameObject("BFan2").SetActive(true);
                        break;

                        case 3:
                            childLocator.FindChildGameObject("KKnuckle").SetActive(true);
                            childLocator.FindChildGameObject("KKnuckle2").SetActive(true);
                        break;

                        case 4:
                            childLocator.FindChildGameObject("SigmaBlade").SetActive(true);
                        break;
                    }


                }
            }
        }

        public void ChangeZeroHand(Transform modelTransform, CharacterModel characterModel, ChildLocator childLocator,CharacterBody characterBody, bool buster)
        {
            if (modelTransform)
            {

                if (characterModel)
                {

                    childLocator.FindChildGameObject("ZeroLHand").SetActive(false);
                    childLocator.FindChildGameObject("ZBusterMesh").SetActive(false);
                    childLocator.FindChildGameObject("BFan2").SetActive(false);
                    childLocator.FindChildGameObject("KKnuckle2").SetActive(false);

                    if (buster)
                    {
                        childLocator.FindChildGameObject("ZBusterMesh").SetActive(true);

                    }
                    else
                    {
                        childLocator.FindChildGameObject("ZeroLHand").SetActive(true);

                        if(characterBody.HasBuff(ZeroBuffs.BFanBuff))
                            childLocator.FindChildGameObject("BFan2").SetActive(true);

                        if(characterBody.HasBuff(ZeroBuffs.KKnuckleBuff))
                            childLocator.FindChildGameObject("KKnuckle2").SetActive(true);

                    }


                }
            }
        }

        public void RemoveWeaponBuffs()
        {

            if (ZBody.HasBuff(ZeroBuffs.TBreakerBuff))
            {
                if (NetworkServer.active)
                {
                    ZBody.RemoveBuff(ZeroBuffs.TBreakerBuff);
                }
            }

            if (ZBody.HasBuff(ZeroBuffs.BFanBuff))
            {
                if (NetworkServer.active)
                {
                    ZBody.RemoveBuff(ZeroBuffs.BFanBuff);
                }
            }

            if (ZBody.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                if (NetworkServer.active)
                {
                    ZBody.RemoveBuff(ZeroBuffs.KKnuckleBuff);
                }
            }

            if (ZBody.HasBuff(ZeroBuffs.SigmaBladeBuff))
            {
                if (NetworkServer.active)
                {
                    ZBody.RemoveBuff(ZeroBuffs.SigmaBladeBuff);
                }
            }

        }

        public Transform GetZTransform()
        {
            return ZmodelTransform;
        }

        public void SetRaikousenPos(Vector3 pos)
        {
            RaikousenStartPos = pos;
        }

        public Vector3 GetRaikousenPos()
        {
            return RaikousenStartPos;
        }

    }
}
