using System;
using System.Collections.Generic;
using System.IO;
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
            HarmonyInstance.PatchAll();
        }

        public override void Load()
        {

            SlimeDefinition pinkSlimeDefinition = SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PUDDLE_SLIME);
            SlimeDefinition slimeDefinition = (SlimeDefinition)PrefabUtils.DeepCopyObject(pinkSlimeDefinition);
            slimeDefinition.AppearancesDefault = new SlimeAppearance[1];
            slimeDefinition.Diet.Produces = new Identifiable.Id[1]
            {
                Identifiable.Id.PINK_PLORT
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
                    Material material = UnityEngine.Object.Instantiate(SRSingleton<GameContext>.Instance.SlimeDefinitions.GetSlimeByIdentifiableId(Identifiable.Id.PUDDLE_SLIME).AppearancesDefault[0].Structures[0].DefaultMaterials[0]);
                    material.SetColor("_TopColor", HexColour.FromHex("3D2851"));
                    material.SetColor("_MiddleColor", HexColour.FromHex("28304D"));
                    material.SetColor("_Color", HexColour.FromHex("3D2851"));
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
                    slimeExpressionFace.Mouth.SetColor("_MouthBot", HexColour.FromHex("FF0000"));
                    slimeExpressionFace.Mouth.SetColor("_MouthMid", HexColour.FromHex("00FF00"));
                    slimeExpressionFace.Mouth.SetColor("_MouthTop", HexColour.FromHex("0000FF"));
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
                Top = HexColour.FromHex("731442"),
                Middle = HexColour.FromHex("731442"),
                Bottom = HexColour.FromHex("731442")
        };
            slimeObject.GetComponent<SlimeAppearanceApplicator>().Appearance = slimeAppearance;
            LookupRegistry.RegisterIdentifiablePrefab(slimeObject);
            SlimeRegistry.RegisterSlimeDefinition(slimeDefinition);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_SLIME));
            slimeAppearance.ColorPalette.Ammo = new Color32(210, 236, 247, 225);
            SRSingleton<GameContext>.Instance.LookupDirector.GetPrefab(Id.OIL_SLIME).GetComponent<Vacuumable>().size = Vacuumable.Size.NORMAL;
            TranslationPatcher.AddActorTranslation("l.oil_slime", "Oil Slime");
        }

        public override void PostLoad()
        {

        }

    }
}