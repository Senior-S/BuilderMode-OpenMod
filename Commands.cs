using System;
using Cysharp.Threading.Tasks;
using OpenMod.Core.Commands;
using System.Threading.Tasks;
using OpenMod.Unturned.Users;
using Microsoft.Extensions.Localization;

namespace BuilderMode
{
    [Command("buildermode")]
    [CommandAlias("bm")]
    [CommandAlias("bmode")]
    [CommandDescription("Command to enable or disable the builder mode.")]
    public class CommandAwesome : Command
    {
        private readonly IStringLocalizer ro_StringLocalizer;

        public CommandAwesome(IServiceProvider serviceProvider, IStringLocalizer stringLocalizer) : base(serviceProvider)
        {
            ro_StringLocalizer = stringLocalizer;
        }

        protected override async Task OnExecuteAsync()
        {
            var user = (UnturnedUser)Context.Actor;
            var z = PlayerManager.InBuilderMode;
            if (z.Contains(user.Id))
            {
                z.Remove(user.Id);
                await user.PrintMessageAsync(ro_StringLocalizer["plugin_translations:buildermode_off"], System.Drawing.Color.Red);
            }
            else
            {
                z.Add(user.Id);
                await user.PrintMessageAsync(ro_StringLocalizer["plugin_translations:buildermode_on"], System.Drawing.Color.Blue);
            }
        }
    }
}
