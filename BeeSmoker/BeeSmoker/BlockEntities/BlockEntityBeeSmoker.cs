using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace BeeSmoker.BlockEntities
{
    internal class BlockEntityBeeSmoker : BlockEntity
    {
        private ICoreAPI coreAPI;
        private ICoreClientAPI capi;
        private ICoreServerAPI sapi;
        private float burnTime = 0;
        private float maxBurnTime = 0;

        public bool IsBurning => burnTime > 0;
        public bool Enabled { get; set; } = false;

        public override void OnBlockBroken(IPlayer byPlayer = null)
        {
            base.OnBlockBroken(byPlayer);
            Enabled = false;
        }

        public void OnServerTick(float dt)
        {
            
        }
    }
}