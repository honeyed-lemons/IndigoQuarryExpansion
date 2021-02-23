using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using SRML;
using SRML.SR;
using SRML.SR.Translation;
using SRML.Utils;
using UnityEngine;

namespace iqe
{
    public class Main : ModEntryPoint
    {
        public override void PreLoad()
        {

            SRCallbacks.OnSaveGameLoaded += t =>
            {
                if (Levels.IsLevel(Levels.WORLD))
                {
                    foreach (DirectedSlimeSpawner spawner in UnityEngine.Object.FindObjectsOfType<DirectedSlimeSpawner>().Where(s => s.GetComponentInParent<MonomiPark.SlimeRancher.Regions.Region>(true).GetZoneId() == ZoneDirector.Zone.QUARRY))
                    {
                        foreach (DirectedActorSpawner.SpawnConstraint constraint in spawner.constraints)
                        {
                            List<SlimeSet.Member> members = new List<SlimeSet.Member>(constraint.slimeset.members)
                {
                    new SlimeSet.Member
                    {
                        prefab = GameContext.Instance.LookupDirector.GetPrefab(Id.OIL_SLIME),
                        weight = 0.5f 
                    }
                };

                            constraint.slimeset.members = members.ToArray();
                        }
                    }
                }
            };
            HarmonyInstance.PatchAll();
        }

        public override void Load()
        {
            GameObject gameObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_PLORT));
            gameObject.GetComponent<Identifiable>().id = Id.OIL_PLORT;
            gameObject.name = "Oil_Plort";
            LookupRegistry.RegisterIdentifiablePrefab(gameObject);
            Color value = HexColour.FromHex("3D2851");
            Color value2 = HexColour.FromHex("28304D");
            Color value3 = HexColour.FromHex("222335");
            gameObject.GetComponent<MeshRenderer>().material = UnityEngine.Object.Instantiate<Material>(gameObject.GetComponent<MeshRenderer>().material);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_TopColor", value);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_MiddleColor", value2);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", value3);
            gameObject.GetComponent<MeshRenderer>().material.SetColor("_CrackColor", value);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_PLORT));
            Sprite sprite = Main.assetBundle.LoadAsset<Sprite>("iconPlortOil");
            LookupRegistry.RegisterVacEntry(Id.OIL_PLORT, HexColour.FromHex("28304D"), sprite);
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_PLORT).GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;
            TranslationPatcher.AddActorTranslation("l.oil_plort", "Oil Plort");
            PlortRegistry.RegisterPlort(Id.OIL_PLORT, 25f, 150f);
            DroneRegistry.RegisterBasicTarget(Id.OIL_PLORT);

            SlimeDefinition pinkSlimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PINK_SLIME);
            SlimeDefinition slimeDefinition = (SlimeDefinition)PrefabUtils.DeepCopyObject(pinkSlimeDefinition);
            slimeDefinition.AppearancesDefault = new SlimeAppearance[1];
            slimeDefinition.Diet.Produces = new Identifiable.Id[1]
            {
                Id.OIL_PLORT
            };
            slimeDefinition.Diet.MajorFoodGroups = new SlimeEat.FoodGroup[1]
            {
                SlimeEat.FoodGroup.VEGGIES
            };
            slimeDefinition.Diet.AdditionalFoods = new Identifiable.Id[0];
            slimeDefinition.Diet.Favorites = new Identifiable.Id[1]
            {
                Identifiable.Id.CARROT_VEGGIE
            };
            slimeDefinition.Diet.EatMap?.Clear();
            slimeDefinition.CanLargofy = false;
            slimeDefinition.FavoriteToys = new Identifiable.Id[0];
            slimeDefinition.Name = "Oil Slime";
            slimeDefinition.IdentifiableId = Id.OIL_SLIME;

            GameObject slimeObject = PrefabUtils.CopyPrefab(SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Identifiable.Id.PINK_SLIME));
            slimeObject.name = "oilSlime";
            slimeObject.GetComponent<PlayWithToys>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeAppearanceApplicator>().SlimeDefinition = slimeDefinition;
            slimeObject.GetComponent<SlimeEat>().slimeDefinition = slimeDefinition;
            slimeObject.GetComponent<Identifiable>().id = Id.OIL_SLIME;
            UnityEngine.Object.Destroy(slimeObject.GetComponent<PinkSlimeFoodTypeTracker>());

            SlimeAppearance slimeAppearance = (SlimeAppearance)PrefabUtils.DeepCopyObject(pinkSlimeDefinition.AppearancesDefault[0]);
            slimeDefinition.AppearancesDefault[0] = slimeAppearance;
            SlimeAppearanceStructure[] structures = slimeAppearance.Structures;
            foreach (SlimeAppearanceStructure slimeAppearanceStructure in structures)
            {
                Material[] defaultMaterials = slimeAppearanceStructure.DefaultMaterials;
                if (defaultMaterials != null && defaultMaterials.Length != 0)
                {
                    Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.HONEY_SLIME
                        ).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);
                    material.SetColor("_TopColor", HexColour.FromHex("3D2851"));
                    material.SetColor("_MiddleColor", HexColour.FromHex("28304D"));
                    material.SetColor("_BottomColor", HexColour.FromHex("222335"));
                    material.SetFloat("_Shininess", 1f);
                    material.SetFloat("_Gloss", 2f);
                    slimeAppearanceStructure.DefaultMaterials[0] = material;
                }
            }
            SlimeExpressionFace[] expressionFaces = slimeAppearance.Face.ExpressionFaces;
            for (int k = 0; k < expressionFaces.Length; k++)
            {
                SlimeExpressionFace slimeExpressionFace = expressionFaces[k];
                if ((bool)slimeExpressionFace.Mouth)
                {
                    slimeExpressionFace.Mouth.SetColor("_MouthBot", HexColour.FromHex("11143D"));
                    slimeExpressionFace.Mouth.SetColor("_MouthMid", HexColour.FromHex("4A4D70"));
                    slimeExpressionFace.Mouth.SetColor("_MouthTop", HexColour.FromHex("FFFFFF"));
                }
                if ((bool)slimeExpressionFace.Eyes)
                {
                    slimeExpressionFace.Eyes.SetColor("_EyeRed", HexColour.FromHex("0E102D"));
                    slimeExpressionFace.Eyes.SetColor("_EyeGreen", HexColour.FromHex("EDEEFF"));
                    slimeExpressionFace.Eyes.SetColor("_EyeBlue", HexColour.FromHex("FFFFFF"));
                }
            }
            slimeAppearance.Face.OnEnable();
            slimeAppearance.ColorPalette = new SlimeAppearance.Palette
            {
                Top = HexColour.FromHex("3D2851"),
                Middle = HexColour.FromHex("28304D"),
                Bottom = HexColour.FromHex("222335")
        };
            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = slimeAppearance;
            LookupRegistry.RegisterIdentifiablePrefab(slimeObject);
            SlimeRegistry.RegisterSlimeDefinition(slimeDefinition);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_SLIME));
            slimeAppearance.ColorPalette.Ammo = HexColour.FromHex("28304D");
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_SLIME).GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;
            TranslationPatcher.AddActorTranslation("l.oil_slime", "Oil Slime");
            Sprite spriteOilSlime = Main.assetBundle.LoadAsset<Sprite>("iconSlimeOil");
            slimeAppearance.Icon = Main.assetBundle.LoadAsset<Sprite>("iconSlimeOil");
        }

        public override void PostLoad()
        {
        }

        public static Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("IQE.iqe");
        public static AssetBundle assetBundle = AssetBundle.LoadFromStream(Main.manifestResourceStream);


    }
}