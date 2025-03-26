using BeeSmoker.Blocks;
using BeeSmoker.BlockEntities;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BeeSmoker
{
    public class BeeSmokerModSystem : ModSystem
    {
        // Called on server and client
        // Useful for registering block/entity classes on both sides
        public override void Start(ICoreAPI api)
        {
            base.Start(api);

            api.RegisterBlockClass(Mod.Info.ModID + ".blockbeesmoker", typeof(BlockBeeSmoker));
            api.RegisterBlockEntityClass(Mod.Info.ModID + ".blockentitybeesmoker", typeof(BlockEntityBeeSmoker));
            api.RegisterBlockClass(Mod.Info.ModID + ".skepsmokeraware", typeof(BlockSkepSmokerAware));
        }
    }
}
