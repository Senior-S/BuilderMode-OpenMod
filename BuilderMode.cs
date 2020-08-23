using System;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
using OpenMod.API.Users;
using OpenMod.Core.Helpers;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;

[assembly: PluginMetadata("SS.BuilderMode", DisplayName = "BuilderMode")]
namespace BuilderMode
{
    public static class PlayerManager
    {
        public static List<string> InBuilderMode = new List<string>();
    }

    public class BuilderMode : OpenModUnturnedPlugin
    {
        private readonly IUserManager ro_UserManager;
        private readonly ILogger<BuilderMode> ro_Logger;

        public BuilderMode(
            IUserManager userManager, 
            ILogger<BuilderMode> logger, 
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            ro_UserManager = userManager;
            ro_Logger = logger;
        }

        protected override async UniTask OnLoadAsync()
        {
            await UniTask.SwitchToMainThread();
            BarricadeManager.onDeployBarricadeRequested += OnDeployBarricade;
            ro_Logger.LogInformation("Plugin loaded correctly!");
            ro_Logger.LogInformation("If you have any error you can contact the owner in discord: Senior S#9583");
        }

        private void OnDeployBarricade(Barricade barricade, ItemBarricadeAsset asset, Transform hit, ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner, ref ulong group, ref bool shouldAllow)
        {
            var z = owner;
            shouldAllow = true;
            AsyncHelper.RunSync(async () => {
                var user = (UnturnedUser)await ro_UserManager.FindUserAsync(KnownActorTypes.Player, z.ToString(), UserSearchMode.FindById);
                var x = PlayerManager.InBuilderMode;
                if (x.Contains(user.Id))
                {
                    var c = new Item(asset.id, true);
                    user.Player.inventory.tryAddItem(c, true);
                }
            });
        }

        protected override async UniTask OnUnloadAsync()
        {
            await UniTask.SwitchToMainThread();
            BarricadeManager.onDeployBarricadeRequested -= OnDeployBarricade;
            ro_Logger.LogInformation("Plugin unloaded correctly!");
        }
    }
}
