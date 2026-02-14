using SwiftlyS2.Shared.Commands;

namespace BetterFOV;

public sealed partial class BetterFOV
{
    private void RegisterCommands()
    {
        foreach (var command in config.Commands)
        {
            if (Core.Command.IsCommandRegistered(command))
            {
                continue;
            }

            var commandGuid = Core.Command.RegisterCommand(command, HandleFOVCommand);
            registeredCommandIds.Add(commandGuid);
        }
    }

    private void UnregisterCommands()
    {
        foreach (var commandId in registeredCommandIds)
        {
            Core.Command.UnregisterCommand(commandId);
        }

        registeredCommandIds.Clear();
    }

    private void HandleFOVCommand(ICommandContext context)
    {
        var player = context.Sender;
        if (!IsPlayable(player))
        {
            return;
        }

        var sourcePlayer = player!;
        var localizer = Core.Translation.GetPlayerLocalizer(sourcePlayer);

        if (!config.Enabled)
        {
            sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.disabled"]);
            return;
        }

        if (context.Args.Length == 0)
        {
            var appliedDefault = fovPreferences.Set(sourcePlayer, config.DefaultFOV, config, playerCookiesApi);
            FOVApplier.TryApply(sourcePlayer, appliedDefault);
            sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.reset_success", appliedDefault]);
            return;
        }

        if (!int.TryParse(context.Args[0], out var requestedFOV))
        {
            sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.invalid_value"]);
            sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.usage", config.Commands[0]]);
            return;
        }

        if (requestedFOV < config.MinFOV || requestedFOV > config.MaxFOV)
        {
            sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.out_of_range", config.MinFOV, config.MaxFOV]);
            return;
        }

        var appliedFOV = fovPreferences.Set(sourcePlayer, requestedFOV, config, playerCookiesApi);
        FOVApplier.TryApply(sourcePlayer, appliedFOV);
        sourcePlayer.SendChat(config.Prefix + localizer["betterfov.chat.set_success", appliedFOV]);
    }
}
