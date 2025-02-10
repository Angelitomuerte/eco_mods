﻿// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Eco.Core.Items;
    using Eco.Gameplay.Blocks;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.DynamicValues;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Housing;
    using Eco.Gameplay.Interactions;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Modules;
    using Eco.Gameplay.Minimap;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Occupancy;
    using Eco.Gameplay.Players;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Skills;
    using Eco.Gameplay.Systems;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Gameplay.Pipes.LiquidComponents;
    using Eco.Gameplay.Pipes.Gases;
    using Eco.Shared;
    using Eco.Shared.Math;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Utils;
    using Eco.Shared.View;
    using Eco.Shared.Items;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Pipes;
    using Eco.World.Blocks;
    using Eco.Gameplay.Housing.PropertyValues;
    using Eco.Gameplay.Civics.Objects;
    using Eco.Gameplay.Settlements;
    using Eco.Gameplay.Systems.NewTooltip;
    using Eco.Core.Controller;
    using Eco.Core.Utils;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items.Recipes;

    [Serialized]
    [RequireComponent(typeof(PropertyAuthComponent))]
    [RequireComponent(typeof(CustomTextComponent))]
    [RequireComponent(typeof(WorldObjectComponent))]
    [RequireComponent(typeof(OccupancyRequirementComponent))]
    [Tag("Usable")]
    [Ecopedia("Crafted Objects", "Signs", subPageName: "Sci-Fi Display Sign Item")]
    public partial class SciFiSignObject : WorldObject, IRepresentsItem
    {

        static SciFiSignObject()
        {

            WorldObject.AddOccupancy<SciFiSignObject>(new List<BlockOccupancy>(){
            new BlockOccupancy(new Vector3i(0, 1, 0),
            new BlockOccupancy(new Vector3i(0, 1, -1),
            new BlockOccupancy(new Vector3i(0, 0, 0),
            new BlockOccupancy(new Vector3i(0, 0, -1)
         });

        }
        
    }

    public partial class SciFiSignObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SciFiSignItem);
        public override LocString DisplayName => Localizer.DoStr("Sci-Fi Display Sign");

        protected override void Initialize()
        {
            this.ModsPreInitialize();
            this.GetComponent<CustomTextComponent>().Initialize(1000);  // Capacity for characters displayed
            this.ModsPostInitialize();
        }

        /// <summary>Hook for mods to customize WorldObject before initialization. You can change housing values here.</summary>
        partial void ModsPreInitialize();
        /// <summary>Hook for mods to customize WorldObject after initialization.</summary>
        partial void ModsPostInitialize();
    }

    [Serialized]
    [LocDisplayName("Sci-Fi Display Sign")]
    [LocDescription("A futuristic sign with an LED-like glowing text display.")]
    [Ecopedia("Crafted Objects", "Signs", createAsSubPage: true)]
    [Weight(1500)]
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Backward, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    [Serialized, SyncToView, NewTooltipChildren(CacheAs.Instance, flags: TTFlags.AllowNonControllerTypeForChildren)] public object PersistentData { get; set; }

    /// <summary>
    /// Server-side recipe definition for "Sci-Fi Display Sign".
    /// </summary>
    [RequiresSkill(typeof(SmeltingSkill), 2)]
    [Ecopedia("Crafted Objects", "Signs", subPageName: "Sci-Fi Display Sign Item")]
    public partial class SciFiSignRecipe : RecipeFamily
    {
        public SciFiSignRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "SciFiSign",
                displayName: Localizer.DoStr("Sci-Fi Display Sign"),
                ingredients: new List<IngredientElement>
                {
                    new IngredientElement("IronBarItem", 8, typeof(SmeltingSkill)),
                    new IngredientElement("GlassItem", 6, typeof(SmeltingSkill))
                },
                items: new List<CraftingElement>
                {
                    new CraftingElement<SciFiSignItem>()
                }
            );
            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 2;

            this.LaborInCalories = CreateLaborInCaloriesValue(200, typeof(SmeltingSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(SciFiSignRecipe), start: 4, skillType: typeof(SmeltingSkill));

            this.ModsPreInitialize();
            this.Initialize(Localizer.DoStr("Sci-Fi Display Sign"), typeof(SciFiSignRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(typeof(AnvilObject), this);  // Adjust the crafting station if needed
        }

        partial void ModsPreInitialize();
        partial void ModsPostInitialize();
    }
}
