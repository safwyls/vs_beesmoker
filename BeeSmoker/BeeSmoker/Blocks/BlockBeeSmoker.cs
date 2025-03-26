using BeeSmoker.BlockEntities;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BeeSmoker.Blocks
{
    internal class BlockBeeSmoker : Block
    {
        public SimpleParticleProperties IdleParticles { get; private set; }

        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);

            if (api is ICoreServerAPI sapi)
            {
                // Do server stuff
            }
        }
    }
}
