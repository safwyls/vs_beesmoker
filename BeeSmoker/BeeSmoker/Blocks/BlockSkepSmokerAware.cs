using Vintagestory.API.Common.Entities;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using Vintagestory.API.Util;

namespace BeeSmoker.Blocks
{
    internal class BlockSkepSmokerAware : BlockSkep
    {
        private float smokerAwareSpawnChance = 0.4f;

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            #region Original OnBlockBroken Method
            // Call the original Block OnBlockBroken method manually to avoid calling base.OnBlockBroken which spawns bees
            bool preventDefault = false;
            foreach (BlockBehavior blockBehavior in this.BlockBehaviors)
            {
                EnumHandling handled = EnumHandling.PassThrough;
                blockBehavior.OnBlockBroken(world, pos, byPlayer, ref handled);
                if (handled == EnumHandling.PreventDefault)
                {
                    preventDefault = true;
                }
                if (handled == EnumHandling.PreventSubsequent)
                {
                    return;
                }
            }
            if (preventDefault)
            {
                return;
            }
            if (this.EntityClass != null)
            {
                BlockEntity entity = world.BlockAccessor.GetBlockEntity(pos);
                if (entity != null)
                {
                    entity.OnBlockBroken(byPlayer);
                }
            }
            if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
            {
                ItemStack[] drops = this.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);
                if (drops != null)
                {
                    for (int i = 0; i < drops.Length; i++)
                    {
                        if (this.SplitDropStacks)
                        {
                            for (int j = 0; j < drops[i].StackSize; j++)
                            {
                                ItemStack stack = drops[i].Clone();
                                stack.StackSize = 1;
                                world.SpawnItemEntity(stack, pos, null);
                            }
                        }
                        else
                        {
                            world.SpawnItemEntity(drops[i].Clone(), pos, null);
                        }
                    }
                }
                BlockSounds sounds = this.Sounds;
                world.PlaySoundAt((sounds != null) ? sounds.GetBreakSound(byPlayer) : null, pos, 0.0, byPlayer, true, 32f, 1f);
            }
            this.SpawnBlockBrokenParticles(pos);
            world.BlockAccessor.SetBlock(0, pos);
            #endregion

            bool smokerInAction = PlayerHoldingSmoker(byPlayer);

            if (!smokerInAction) 
            {
                if (world.Side == EnumAppSide.Server && !this.IsEmpty() && world.Rand.NextDouble() < (double)this.smokerAwareSpawnChance)
                {
                    EntityProperties type = world.GetEntityType(new AssetLocation("beemob"));
                    Entity entity = world.ClassRegistry.CreateEntity(type);
                    if (entity != null)
                    {
                        entity.ServerPos.X = (double)((float)pos.X + 0.5f);
                        entity.ServerPos.Y = (double)((float)pos.Y + 0.5f);
                        entity.ServerPos.Z = (double)((float)pos.Z + 0.5f);
                        entity.ServerPos.Yaw = (float)world.Rand.NextDouble() * 2f * 3.1415927f;
                        entity.Pos.SetFrom(entity.ServerPos);
                        entity.Attributes.SetString("origin", "brokenbeehive");
                        world.SpawnEntity(entity);
                    }
                }
            }
        }

        private bool PlayerHoldingSmoker(IPlayer player)
        {
            // Check main hand
            AssetLocation mainHandCode = player.Entity.ActiveHandItemSlot?.Itemstack?.Block?.Code;            
            bool inMainHand = false;
            
            if (mainHandCode != null)
            {
                inMainHand = WildcardUtil.Match("beesmoker:beesmoker-*", mainHandCode.ToShortString());
            }

            if (inMainHand) return true;

            // Check off hand
            AssetLocation offhandCode = player.Entity.LeftHandItemSlot?.Itemstack?.Block?.Code;
            bool inOffHand = false;

            if (offhandCode != null)
            {
                inOffHand = WildcardUtil.Match("beesmoker:beesmoker-*", offhandCode.ToShortString());
            }

            return inOffHand;
        }

    }
}
