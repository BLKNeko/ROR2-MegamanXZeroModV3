using BepInEx.Configuration;
using ZeroModV3.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroModV3.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        public override string bodyName => "ZeroV3";

        public const string ZERO_PREFIX = ZeroModV3Plugin.DEVELOPER_PREFIX + "_ZERO_V3_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ZERO_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "ZeroBody",
            bodyNameToken = ZERO_PREFIX + "NAME",
            subtitleNameToken = ZERO_PREFIX + "SUBTITLE",

            characterPortrait = Modules.Assets.ZIcon,
            bodyColor = new Color(0.55f, 0.15f, 0.15f),

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 140f,
            healthGrowth = 28f,
            healthRegen = 1.5f,
            armor = 20f,
            armorGrowth = 1.5f,
            damage = 22f,
            shieldGrowth = 0.3f,
            jumpPowerGrowth = 0.3f,
            jumpCount = 2,
            attackSpeed = 1.15f,
            attackSpeedGrowth = 0.04f,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
            /*
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Materials.CreateHopooMaterial("matHenry"),
                },
                new CustomRendererInfo
                {
                    childName = "GunModel",
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                }
                */
                
                new CustomRendererInfo
                {
                    childName = "HandMeshRM",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMeshRC",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMeshLM",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMeshLC",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "FaceMeshC",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "BodyMeshM",
                    material = Materials.CreateHopooMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "BodyMeshC",
                    material = Materials.CreateHopooMaterial("matZero"),
                }

        };

        public override UnlockableDef characterUnlockableDef => null;

        //public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);
        public override Type characterMainState => typeof(SkillStates.BaseStates.LearningSystem);

        public override Type characterDeathState => typeof(SkillStates.BaseStates.DeathState);

        public override Type characterSpawnState => typeof(SkillStates.BaseStates.SpawnState);


        public override ItemDisplaysBase itemDisplays => new HenryItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            //ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            //GameObject model = childLocator.gameObject;

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("ZSaberHitBox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "ZSaberHitBox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform.localScale = new Vector3(4f, 4f, 4f);

            //----------------------------------


            Transform hitboxTransformGI = childLocator.FindChild("GroundImpact");
            Modules.Prefabs.SetupHitbox(model, hitboxTransformGI, "GroundImpact");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransformGI.localScale = new Vector3(4f, 4f, 4f);
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            //string prefix = ZeroModV3Plugin.DEVELOPER_PREFIX;
            Modules.Skills.PassiveSetup(bodyPrefab);

            #region Primary
            //Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)), "Weapon", prefix + "_HENRY_BODY_PRIMARY_SLASH_NAME", prefix + "_HENRY_BODY_PRIMARY_SLASH_DESCRIPTION", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"), true));

            SkillDef zsaberSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(ZERO_PREFIX + "ZSABER_NAME",
                                                                                      ZERO_PREFIX + "ZSABER_DESCRIPTION",
                                                                                      Modules.Assets.ZSaber,
                                                                                      new EntityStates.SerializableEntityStateType(typeof(SkillStates.Slash)),
                                                                                      "Weapon",
                                                                                      true));

            Modules.Skills.AddPrimarySkills(bodyPrefab, zsaberSkillDef);

            #endregion

            #region Secondary
            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = ZERO_PREFIX + "ZBUSTER_NAME",
                skillNameToken = ZERO_PREFIX + "ZBUSTER_NAME",
                skillDescriptionToken = ZERO_PREFIX + "ZBUSTER_DESCRIPTION",
                skillIcon = Modules.Assets.ZBuster,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZBuster)),
                activationStateMachineName = "Slide",
                baseMaxStock = 2,
                baseRechargeInterval = 6f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);

            //------------------------------------------------------------------

            SkillDef ZDefSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = ZERO_PREFIX + "ZDEF_NAME",
                skillNameToken = ZERO_PREFIX + "ZDEF_NAME",
                skillDescriptionToken = ZERO_PREFIX + "ZDEF_DESCRIPTION",
                skillIcon = Modules.Assets.ZDefense,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZDefense)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                // keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, ZDefSkillDef);


            #endregion

            #region Utility
            SkillDef ZDashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = ZERO_PREFIX + "ZDASH_NAME",
                skillNameToken = ZERO_PREFIX + "ZDASH_NAME",
                skillDescriptionToken = ZERO_PREFIX + "ZDASH_DESCRIPTION",
                skillIcon = Modules.Assets.ZDash,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZDash)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 3.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, ZDashSkillDef);
            #endregion

            #region Special
            SkillDef UpSlashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = ZERO_PREFIX + "UPSLASH_NAME",
                skillNameToken = ZERO_PREFIX + "UPSLASH_NAME",
                skillDescriptionToken = ZERO_PREFIX + "UPSLASH_DESCRIPTION",
                skillIcon = Modules.Assets.ZRyouenjin,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.UpSlash)),
                activationStateMachineName = "Body",
                baseMaxStock = 2,
                baseRechargeInterval = 9f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, UpSlashSkillDef);
            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(ZeroModV3Plugin.DEVELOPER_PREFIX + "_HENRY_BODY_DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                //place your mesh replacements here
                //unnecessary if you don't have multiple skins
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySword"),
                //    renderer = defaultRenderers[0].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryGun"),
                //    renderer = defaultRenderers[1].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                //    renderer = defaultRenderers[2].renderer
                //}
            };

            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            Material masteryMat = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySwordAlt"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryAlt"),
                    renderer = defaultRenderers[2].renderer
                }
            };

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}