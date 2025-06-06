﻿using BepInEx.Configuration;
using ZeroMod.Modules;
using ZeroMod.Modules.Characters;
using ZeroMod.Survivors.Zero.Components;
using ZeroMod.Survivors.Zero.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RoR2.OutlineHighlight;
using ZeroMod.Characters.Survivors.Zero.Components;
using ZeroMod.Modules.BaseContent.BaseStates;
using EmotesAPI;
using RoR2.UI;
using UnityEngine.UI;
using RoR2.Projectile;
using static UnityEngine.ParticleSystem.PlaybackState;
using UnityEngine.Networking;

namespace ZeroMod.Survivors.Zero
{
    public class ZeroSurvivor : SurvivorBase<ZeroSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "mmxzerobundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "MMXZeroBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "ZeroMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlZero";
        public override string displayPrefabName => "ZeroDisplay";

        public const string ZERO_X_PREFIX = ZeroPlugin.DEVELOPER_PREFIX + "_ZERO_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => ZERO_X_PREFIX;

        internal bool setupEmoteSkeleton = false;

        private float zTakeDamageValue = 0f;
        private CharacterMaster zMaster;

        private GameObject ZMouseIconGO;
        private HUD hud = null;

        private bool gokumonkenAtk = false;

        //SKILL DEFS

        //WEAPONS
        internal static SkillDef ZSaberSkillDef;
        internal static SkillDef TBreakerSkillDef;
        internal static SkillDef BFanSkillDef;
        internal static SkillDef KKnuckleSkillDef;
        internal static SkillDef SigmaBladeSkillDef;

        //PRIMARY
        internal static SkillDef ZSaberComboSkillDef;

        //SECONDARY
        internal static SkillDef ZBusterSkillDef;
        internal static SkillDef CFlasherSkillDef;

        //UTILITY
        internal static SkillDef ZDashSkillDef;
        internal static SkillDef RaikousenSkillDef;

        //SPECIAL
        
        internal static SkillDef RyuuenjinSkillDef;
        internal static SkillDef GokumonkenSkillDef;
        internal static SkillDef IceDragonRiseSkillDef;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = ZERO_X_PREFIX + "NAME",
            subtitleNameToken = ZERO_X_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("TexZero"),
            bodyColor = new Color(0.55f, 0.15f, 0.15f),
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 150f,
            healthGrowth = 30f,
            healthRegen = 2f,
            regenGrowth = 0.5f,
            armor = 50f,
            armorGrowth = 4f,
            damage = 15f,
            damageGrowth = 0.25f,
            shieldGrowth = 0.5f,
            jumpPowerGrowth = 0.3f,
            jumpCount = 2,
            attackSpeed = 1.15f,
            attackSpeedGrowth = 0.04f,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "ZeroBodyMesh",
                    material = assetBundle.LoadMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "ZSaberMesh",
                    material = assetBundle.LoadMaterial("matZSaber"),
                },
                new CustomRendererInfo
                {
                    childName = "ZBusterMesh",
                    material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "ZeroLHand",
                    material = assetBundle.LoadMaterial("matZero"),
                },
                new CustomRendererInfo
                {
                    childName = "TBreaker",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "BFan",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "BFan2",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "KKnuckle",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "KKnuckle2",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "SigmaBlade",
                    //material = assetBundle.LoadMaterial("matZeroBuster"),
                }
        };

        public override UnlockableDef characterUnlockableDef => ZeroUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new ZeroItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Zero");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            ZeroUnlockables.Init();

            base.InitializeCharacter();

            ZeroConfig.Init();
            ZeroStates.Init();
            ZeroTokens.Init();

            ZeroAssets.Init(assetBundle);
            ZeroBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<ZeroBaseComponent>();
            bodyPrefab.AddComponent<ElectricTrailFollow>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            // Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("ZSaberHitBox");
            Prefabs.SetupHitBoxGroup(model, "ZSaberHitBox", "ZSaberHitBox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform.localScale = new Vector3(6f, 6f, 6f);

            Transform hitboxTransform2 = childLocator.FindChild("KuuenzanHitBox");
            Prefabs.SetupHitBoxGroup(model, "KuuenzanHitBox", "KuuenzanHitBox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform.localScale = new Vector3(6f, 6f, 6f);

        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(ZeroDeathState));
            bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new EntityStates.SerializableEntityStateType(typeof(ZeroSpawnState));

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            //add our own
            CreateSkillDefs();
            Skills.CreateFirstExtraSkillFamily(bodyPrefab);
            Skills.CreateSecondExtraSkillFamily(bodyPrefab);
            Skills.CreateThirdExtraSkillFamily(bodyPrefab);
            Skills.CreateFourthExtraSkillFamily(bodyPrefab);
            AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();

            AddExtraFirstSkills();
            AddExtraSecondSkills();
            AddExtraThirdSkills();
            AddExtraFourthSkills();
        }

        private void CreateSkillDefs()
        {

            ZSaberSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ZSaber",
                skillNameToken = ZERO_X_PREFIX + "WEAPON_ZSABER_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "WEAPON_ZSABER_DESCRIPTION",
                skillIcon = ZeroAssets.ZSaberIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZSaber)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });


            TBreakerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "TBreaker",
                skillNameToken = ZERO_X_PREFIX + "WEAPON_TBREAKER_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "WEAPON_TBREAKER_DESCRIPTION",
                skillIcon = ZeroAssets.TBreakerIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(TBreaker)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            BFanSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BFan",
                skillNameToken = ZERO_X_PREFIX + "WEAPON_BFAN_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "WEAPON_BFAN_DESCRIPTION",
                skillIcon = ZeroAssets.BFanIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(BFan)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            KKnuckleSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "KKnuckle",
                skillNameToken = ZERO_X_PREFIX + "WEAPON_KKNUCKLE_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "WEAPON_KKNUCKLE_DESCRIPTION",
                skillIcon = ZeroAssets.KKnuckleIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(KKnuckle)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            SigmaBladeSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SigmaBlade",
                skillNameToken = ZERO_X_PREFIX + "WEAPON_SIGMABLADE_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "WEAPON_SIGMABLADE_DESCRIPTION",
                skillIcon = ZeroAssets.SigmaBladeIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SigmaBlade)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            ZSaberComboSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ZSaberCombo",
                skillNameToken = ZERO_X_PREFIX + "PRIMARY_ZSABER_COMBO_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "PRIMARY_ZSABER_COMBO_DESCRIPTION",
                skillIcon = ZeroAssets.ZSaberSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZSSlashCombo)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });


            ZBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ZBuster",
                skillNameToken = ZERO_X_PREFIX + "SECONDARY_ZBUSTER_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SECONDARY_ZBUSTER_DESCRIPTION",
                skillIcon = ZeroAssets.ZBusterSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZBuster)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            ZDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Dash",
                skillNameToken = ZERO_X_PREFIX + "UTILITY_ZDASH_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "UTILITY_ZDASH_DESCRIPTION",
                skillIcon = ZeroAssets.ZDashSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZDash)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            //RaikousenSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "Raikousen",
            //    skillNameToken = ZERO_X_PREFIX + "SECONDARY_SQUEEZE_BOMB_NAME",
            //    skillDescriptionToken = ZERO_X_PREFIX + "SECONDARY_SQUEEZE_BOMB_DESCRIPTION",
            //    //skillIcon = XAssets.IconSqueezeBomb,

            //    activationState = new EntityStates.SerializableEntityStateType(typeof(Raikousen)),
            //    activationStateMachineName = "Weapon",
            //    interruptPriority = EntityStates.InterruptPriority.Skill,

            //    baseRechargeInterval = 1f,
            //    baseMaxStock = 5,

            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1,

            //    resetCooldownTimerOnUse = false,
            //    fullRestockOnAssign = true,
            //    dontAllowPastMaxStocks = false,
            //    mustKeyPress = false,
            //    beginSkillCooldownOnSkillEnd = false,

            //    isCombatSkill = true,
            //    canceledFromSprinting = false,
            //    cancelSprintingOnActivation = false,
            //    forceSprintDuringState = false,
            //});

            #region Special

            CFlasherSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CFlasher",
                skillNameToken = ZERO_X_PREFIX + "SPECIAL_SFLASHER_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SPECIAL_SFLASHER_DESCRIPTION",
                skillIcon = ZeroAssets.CFlasherSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(CFlasher)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            RyuuenjinSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Ryuuenjin",
                skillNameToken = ZERO_X_PREFIX + "SPECIAL_RYUUENJIN_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SPECIAL_RYUUENJIN_DESCRIPTION",
                skillIcon = ZeroAssets.RyuenjinSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(Ryuuenjin)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            GokumonkenSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Gokumonken",
                skillNameToken = ZERO_X_PREFIX + "SPECIAL_GOKUMONKEN_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SPECIAL_GOKUMONKEN_DESCRIPTION",
                skillIcon = ZeroAssets.GokumonkenSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(Gokumonken)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            IceDragonRiseSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "IceDragonRise",
                skillNameToken = ZERO_X_PREFIX + "SPECIAL_ICEDRAGONRISE_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SPECIAL_ICEDRAGONRISE_DESCRIPTION",
                skillIcon = ZeroAssets.IceDragonRiseSkillIcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(IceDragonRise)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });



            #endregion


        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = ZERO_X_PREFIX + "PASSIVE_LEARNINGSYSTEM_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "PASSIVE_LEARNINGSYSTEM_DESCRIPTION",
                icon = ZeroAssets.ZPassiveIcon,
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryPassive",
            //    skillNameToken = ZERO_X_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = ZERO_X_PREFIX + "PASSIVE_DESCRIPTION",
            //    keywordTokens = new string[] { "KEYWORD_AGILE" },
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            //    //unless you're somehow activating your passive like a skill, none of the following is needed.
            //    //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //    //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    //activationStateMachineName = "Weapon1",
            //    //interruptPriority = EntityStates.InterruptPriority.Skill,

            //    //baseRechargeInterval = 1f,
            //    //baseMaxStock = 1,

            //    //rechargeStock = 1,
            //    //requiredStock = 1,
            //    //stockToConsume = 1,

            //    //resetCooldownTimerOnUse = false,
            //    //fullRestockOnAssign = true,
            //    //dontAllowPastMaxStocks = false,
            //    //mustKeyPress = false,
            //    //beginSkillCooldownOnSkillEnd = false,

            //    //isCombatSkill = true,
            //    //canceledFromSprinting = false,
            //    //cancelSprintingOnActivation = false,
            //    //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    ZERO_X_PREFIX + "PRIMARY_SLASH_NAME",
                    ZERO_X_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.ZSSlashCombo)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 5;
            primarySkillDef1.stepGraceDuration = 0.5f;

            //Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
            Skills.AddPrimarySkills(bodyPrefab, ZSaberComboSkillDef);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = ZERO_X_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, ZBusterSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, CFlasherSkillDef);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = ZERO_X_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(ZDash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 4f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            //Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
            Skills.AddUtilitySkills(bodyPrefab, ZDashSkillDef);
            //Skills.AddUtilitySkills(bodyPrefab, RaikousenSkillDef);
        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = ZERO_X_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = ZERO_X_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CFlasher)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 10f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            //Skills.AddSpecialSkills(bodyPrefab, CFlasherSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, RyuuenjinSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, IceDragonRiseSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, GokumonkenSkillDef);
        }

        private void AddExtraFirstSkills()
        {

            Skills.AddFirstExtraSkill(bodyPrefab, ZSaberSkillDef);
            Skills.AddFirstExtraSkill(bodyPrefab, SigmaBladeSkillDef);
        }

        private void AddExtraSecondSkills()
        {

            Skills.AddSecondExtraSkill(bodyPrefab, TBreakerSkillDef);
        }

        private void AddExtraThirdSkills()
        {

            Skills.AddThirdExtraSkill(bodyPrefab, BFanSkillDef);
        }

        private void AddExtraFourthSkills()
        {

            Skills.AddFourthExtraSkill(bodyPrefab, KKnuckleSkillDef);
        }


        #endregion skills

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef(ZERO_X_PREFIX + "DEFAULT_SKIN_NAME",
                ZeroAssets.ZeroSkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "ZeroBodyMesh",
                null,
                null,
                "ZeroLHandMesh",
                null,
                null,
                null,
                null,
                null,
                null);

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ZBusterMesh"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("TBreaker"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("SigmaBlade"),
                    shouldActivate = false,
                }
            };

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //creating a new skindef as we did before
            SkinDef BZSkin = Modules.Skins.CreateSkinDef(ZERO_X_PREFIX + "BZ_SKIN_NAME",
                ZeroAssets.BZeroSkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            BZSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "ZeroBodyMesh",
                null,
                null,
                "ZeroLHandMesh",
                null,
                null,
                null,
                null,
                null,
                null);

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            BZSkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matBlackZero");
            BZSkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matBSaber");
            BZSkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matBlackZeroBuster");
            BZSkin.rendererInfos[3].defaultMaterial = assetBundle.LoadMaterial("matBlackZero");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matGaea");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            BZSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ZBusterMesh"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("TBreaker"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("SigmaBlade"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(BZSkin);

            //creating a new skindef as we did before
            SkinDef NZSkin = Modules.Skins.CreateSkinDef(ZERO_X_PREFIX + "NZ_SKIN_NAME",
                ZeroAssets.NZeroSkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            NZSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "ZeroBodyMesh",
                null,
                null,
                "ZeroLHandMesh",
                null,
                null,
                null,
                null,
                null,
                null);

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            NZSkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matNightmareZero");
            NZSkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matNSaber");
            NZSkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matZeroBusterNightmare");
            NZSkin.rendererInfos[3].defaultMaterial = assetBundle.LoadMaterial("matNightmareZero");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matGaea");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            NZSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ZBusterMesh"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("TBreaker"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("SigmaBlade"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(NZSkin);

            //creating a new skindef as we did before
            SkinDef VZSkin = Modules.Skins.CreateSkinDef(ZERO_X_PREFIX + "VZ_SKIN_NAME",
                ZeroAssets.VZeroSkinIcon,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            VZSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "ViaBodyMesh",
                null,
                null,
                "ViaLHandMesh",
                null,
                null,
                null,
                null,
                null,
                null);

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            VZSkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matVia");
            VZSkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matZSaber");
            VZSkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matViaBuster");
            VZSkin.rendererInfos[3].defaultMaterial = assetBundle.LoadMaterial("matVia");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matGaea");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            VZSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("ZBusterMesh"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("TBreaker"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("BFan2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("KKnuckle2"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("SigmaBlade"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(VZSkin);

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(ZERO_X_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            ZeroAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            On.RoR2.CharacterMaster.OnBodyStart += RestoreHPAfterRespawn;
            On.RoR2.UI.HUD.Awake += HUD_Awake;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            //Debug.Log(self.name);
            //Debug.Log(damageInfo.damage);
            //Debug.Log(damageInfo.inflictor);

            if (self == null || damageInfo == null)
                return;

            if (damageInfo.inflictor == null || damageInfo.attacker == null)
                return;

            if (self.GetComponent<CharacterBody>() == null)
                return;

            if (!damageInfo.attacker.name.Contains("Zero") && self.name.Contains("Zero"))
            {

                if (self.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.GokumonkenBuff))
                {
                    Vector3 direction = (damageInfo.attacker.transform.position - self.transform.position).normalized;

                    float dmgBonus = self.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.BFanBuff) ? 2f : 1f;

                    FireProjectileInfo ZeroBusterProjectille = new FireProjectileInfo();
                    ZeroBusterProjectille.projectilePrefab = ZeroAssets.CFlasherProjectile;
                    ZeroBusterProjectille.position = self.transform.position;
                    ZeroBusterProjectille.rotation = Util.QuaternionSafeLookRotation(direction);
                    ZeroBusterProjectille.owner = self.GetComponent<CharacterBody>().gameObject;
                    ZeroBusterProjectille.damage = (damageInfo.damage * (self.GetComponent<CharacterBody>().damage * 0.1f)) * dmgBonus;
                    ZeroBusterProjectille.force = 500;
                    ZeroBusterProjectille.crit = self.GetComponent<CharacterBody>().RollCrit();
                    ZeroBusterProjectille.damageColorIndex = DamageColorIndex.Luminous;
                    ZeroBusterProjectille.damageTypeOverride = DamageTypeCombo.GenericSpecial;

                    //ZeroSurvivor.instance.SetGKAtk(true);

                    if (NetworkServer.active)
                    {
                        if(!self.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.GokumonkenAtkBuff))
                            self.GetComponent<CharacterBody>().AddBuff(ZeroBuffs.GokumonkenAtkBuff);
                    }

                    ProjectileManager.instance.FireProjectile(ZeroBusterProjectille);

                    float barrier = damageInfo.damage / (self.GetComponent<CharacterBody>().HasBuff(ZeroBuffs.BFanBuff) ? 5 : 10);
                    self.GetComponent<CharacterBody>().healthComponent.AddBarrier(barrier);

                }

            }
            

        }

        private void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self); // Don't forget to call this, or the vanilla / other mods' codes will not execute!
            hud = self;
            // hud.mainContainer.transform // This will return the main container. You should put your UI elements under it or its children!
            // Rest of the code is to go here

            //ANCHOR MAX = rectTransform.anchorMax = new Vector2(0.782f, 0.72f);



            //---------------BG--------------

            ZMouseIconGO = new GameObject("ZeroMouseIcon");
            //GameObject myObject = Modules.Assets.MoraleBar;
            ZMouseIconGO.transform.SetParent(hud.mainContainer.transform);

            RectTransform ZMrectTransform = ZMouseIconGO.AddComponent<RectTransform>();

            // Faz com que o pivot fique no centro e as âncoras também
            ZMrectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            ZMrectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            ZMrectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Define o tamanho fixo da imagem
            ZMrectTransform.sizeDelta = new Vector2(128f, 128f);

            // Define a posição relativa ao centro da tela (0,0 é bem no meio)
            ZMrectTransform.anchoredPosition = new Vector2(0f, 80f); // por exemplo, 100px acima da mira

            ZMouseIconGO.AddComponent<Image>();
            ZMouseIconGO.GetComponent<Image>().sprite = ZeroAssets.ZMouseIcon;

            ZMouseIconGO.SetActive(false);

        }

        public void SetMouseIconActive(bool b)
        {
            ZMouseIconGO.SetActive(b);
        }

        private void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self)
        {
            orig(self);

            //Chat.AddMessage($"<color=#00FF00>{self.GetUserName()} subiu para o nível {self.level}!</color>");
            //Chat.AddMessage($"<color=#00FF00>{self.name} subiu para o nível {self.level}!</color>");

            if (self.isPlayerControlled && self.master && self.master.playerCharacterMasterController && self.name.Contains("Zero") && self.hasAuthority) // exemplo de filtro
            {
                //Log.Debug($"Personagem {self.GetUserName()} subiu para o nível {self.level}.");

                if (self.level == ZeroConfig.ZeroFirstUpgradeInt.Value)
                {
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has adapted — Level {self.level} reached.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has mastered a new <color=#00FF00>Saber Combo+</color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has enhanced mobility: Extra jump acquired.");

                    self.baseJumpCount += 1;
                }

                if (self.level == ZeroConfig.ZeroSecondUpgradeInt.Value)
                {
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has adapted — Level {self.level} reached.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has mastered a new <color=#00FF00>Saber Combo++</color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has upgraded mobility: <color=#00FF00>+2 Dash Charge</color> acquired.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Rasetsusen</color> — <color=#FFFFFF>Press <style=cIsUtility>Primary Skill</style> while airborne</color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Senpuukyaku</color> — <color=#FFFFFF>Press <style=cIsUtility>Primary Skill</style> mid-air while using <style=cIsUtility>K-Knuckle</style></color>.");

                    self.inventory.GiveItem(RoR2Content.Items.UtilitySkillMagazine, 1);

                }

                if (self.level == ZeroConfig.ZeroThirdUpgradeInt.Value)
                {
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has adapted — Level {self.level} reached.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has mastered a new <color=#00FF00>Saber Combo+++</color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has upgraded his arsenal: <color=#00FF00>+1 Special Skill Charge</color> acquired.");

                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Hyouretsuzan</color> — <color=#FFFFFF>Hold <style=cIsUtility>Primary</style> + <style=cIsUtility>Secondary</style> during <style=cIsDamage>Ryuenjin</style></color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Enkoujin</color> — <color=#FFFFFF>Hold <style=cIsUtility>Primary</style> + <style=cIsUtility>Secondary</style> during <style=cIsDamage>Hyouryuushou</style></color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Dairettsui</color> — <color=#FFFFFF>Hold <style=cIsUtility>Primary</style> + <style=cIsUtility>Secondary</style> during <style=cIsDamage>Ryuenjin</style> or <style=cIsDamage>Hyouryuushou</style> while using <style=cIsUtility>T-Breaker</style></color>.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has unlocked <color=#00FF00>Raikousen</color> — <color=#FFFFFF>Press <style=cIsUtility>Primary</style> + <style=cIsUtility>Secondary</style> during <style=cIsUtility>DASH</style></color> while <color=#FF0000>NOT</color> using <color=#00FF00>K-Knuckles</color>.");


                    self.inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine, 1);
                }

                if (self.level == ZeroConfig.ZeroFourthUpgradeInt.Value)
                {
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has adapted — Level {self.level} reached.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has enhanced mobility: <color=#00FF00>+1 Extra Jump</color> acquired.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color> has increased firepower: <color=#00FF00>+1 Z-Buster Charge</color> acquired.");
                    Chat.AddMessage($"<color=#FF0000>ZERO</color>'s <color=#00FF00>BFan Barrier</color> has become stronger.");

                    self.inventory.GiveItem(RoR2Content.Items.SecondarySkillMagazine, 1);

                    self.baseJumpCount += 1;
                }

                //Chat.AddMessage($"<color=#00FF00>{self.name} subiu para o nível {self.level}!</color>");

            }

        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self)
            {

                if (self.gameObject.name.Contains("Zero"))
                {
                    AkSoundEngine.PostEvent(ZeroStaticValues.zeroAwake, self.gameObject);

                    //I think TeaL used this on DekuMod to make the character select menu audio
                }

            }


        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(ZeroBuffs.armorBuff))
            {
                args.armorAdd += 300;
                
            }

            if (sender.HasBuff(ZeroBuffs.TBreakerBuff))
            {
                args.damageMultAdd += 3f;
                args.armorAdd += sender.baseArmor * 0.5f;
                args.moveSpeedMultAdd -= 0.3f;
                args.jumpPowerMultAdd -= 0.3f;
            }

            if (sender.HasBuff(ZeroBuffs.BFanBuff))
            {
                args.damageMultAdd -= 0.2f;
                args.armorAdd += 100;
                args.armorAdd += sender.baseArmor * 2f;
                args.moveSpeedMultAdd += 0.5f;
                args.jumpPowerMultAdd += 1f;
                args.attackSpeedMultAdd += 0.5f;
                args.shieldMultAdd += 1f;
                args.regenMultAdd += 0.5f;

            }

            if (sender.HasBuff(ZeroBuffs.KKnuckleBuff))
            {
                args.damageMultAdd -= 0.3f;
                args.moveSpeedMultAdd += 1f;
                args.jumpPowerMultAdd += 0.5f;
                args.attackSpeedMultAdd += 1f;
            }

            if (sender.HasBuff(ZeroBuffs.SigmaBladeBuff))
            {
                args.damageMultAdd += 1f;
                args.armorAdd += sender.baseArmor * 1.5f;
                args.moveSpeedMultAdd += 0.5f;
                args.jumpPowerMultAdd += 0.5f;
                args.attackSpeedMultAdd += 0.25f;
                args.shieldMultAdd += 0.2f;
                args.critAdd += sender.baseCrit * 1.5f;
                args.critDamageMultAdd += 0.5f;
            }

        }


        //EMOTE API

        private void RestoreHPAfterRespawn(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody newBody)
        {
            orig(self, newBody);

            //Debug.Log("xTakeDamageValue: " + xTakeDamageValue);
            //Debug.Log("xMaster: " + xMaster);
            //Debug.Log("self: " + self);

            if (self == zMaster) // Certifica-se de que estamos restaurando o HP do personagem correto
            {
                float restoredHP = zTakeDamageValue;

                if (newBody && newBody.healthComponent)
                {
                    newBody.healthComponent.health = Mathf.Clamp(restoredHP, 1f, newBody.healthComponent.fullHealth);
                    //Debug.Log($"HP restaurado para {newBody.healthComponent.health}");
                }

                //needToReApplyUpgrades = true;


                //RE-APPLY THE JUMP UPGRADES GRANTED BY LEVEL UP

                if (newBody.level >= ZeroConfig.ZeroFirstUpgradeInt.Value)
                {
                    newBody.baseJumpCount += 1;
                }

                //if (newBody.level >= ZeroConfig.ZeroSecondUpgradeInt.Value)
                //{

                //    newBody.skillLocator.utility.maxStock += 1;
                //}

                //if (newBody.level >= ZeroConfig.ZeroThirdUpgradeInt.Value)
                //{

                //    newBody.skillLocator.special.maxStock += 1;
                //}

                if (newBody.level >= ZeroConfig.ZeroFourthUpgradeInt.Value)
                {

                    newBody.baseJumpCount += 1;
                }


            }
        }

        public bool GetGKAtk()
        {
            return gokumonkenAtk;
        }

        public void SetGKAtk(bool b)
        {
            gokumonkenAtk = b;
        }

        private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            //Debug.Log("newAnimation: " + newAnimation);
            //Debug.Log("mapper: " + mapper);
            //Debug.Log("mapper.bodyPrefab.name: " + mapper.bodyPrefab.name);

            if (mapper.bodyPrefab.name.Contains("ZeroBody"))
            {
                if (newAnimation == "none")
                {
                    if (mapper.bodyPrefab.GetComponent<CharacterBody>())
                    {

                        //NA MORAL VOU DEIXAR ISSO TUDO COMENTADO PELO ÓDIO QUE EU SENTI!
                        //Acho que eu tava bem irritado enquanto eu fazia isso no X

                        float savedHP = mapper.bodyPrefab.GetComponent<CharacterBody>().healthComponent.health;

                        zTakeDamageValue = savedHP;
                        zMaster = mapper.bodyPrefab.GetComponent<CharacterBody>().master;



                        // Mata o personagem atual (sem contar como "morte real")
                        GameObject.Destroy(mapper.bodyPrefab.GetComponent<CharacterBody>().gameObject);

                        // Força o CharacterMaster a reaparecer o personagem
                        mapper.bodyPrefab.GetComponent<CharacterBody>().master.Respawn(mapper.bodyPrefab.GetComponent<CharacterBody>().footPosition, Quaternion.identity);

                    }
                }
            }
        }



        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            if (!setupEmoteSkeleton)
            {
                setupEmoteSkeleton = true;
                foreach (var item in SurvivorCatalog.allSurvivorDefs)
                {
                    if (item.bodyPrefab.name == "ZeroBody")
                    {
                        var skele = ZeroAssets.ZeroEmotePrefab;
                        //Debug.Log("Before Emote: " + item.bodyPrefab.transform.position);
                        CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele);
                        CustomEmotesAPI.CreateNameTokenSpritePair(ZERO_X_PREFIX + "NAME", ZeroAssets.ZeroEmoteIcon);
                        //skele.GetComponentInChildren<BoneMapper>().scale = 1.05f;
                        //item.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //skele.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //Debug.Log("after Emote: " + item.bodyPrefab.transform.position);
                        //Debug.Log("skele pos: " + skele.transform.position);
                    }
                }
            }
        }


    }
}