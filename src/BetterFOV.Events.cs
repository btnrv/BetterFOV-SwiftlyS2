using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.GameEventDefinitions;
using SwiftlyS2.Shared.GameEvents;
using SwiftlyS2.Shared.Misc;

namespace BetterFOV;

public sealed partial class BetterFOV
{
    [GameEventHandler(HookMode.Post)]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event)
    {
        if (!config.Enabled)
        {
            return HookResult.Continue;
        }

        var player = Core.PlayerManager.GetPlayer(@event.UserId);
        if (!IsPlayable(player))
        {
            return HookResult.Continue;
        }

        var fov = fovPreferences.GetOrLoad(player!, config, playerCookiesApi);
        FOVApplier.TryApply(player!, fov);

        return HookResult.Continue;
    }

    private void SubscribeCoreEvents()
    {
        Core.Event.OnClientPutInServer += OnClientPutInServer;
        Core.Event.OnClientDisconnected += OnClientDisconnected;
    }

    private void UnsubscribeCoreEvents()
    {
        Core.Event.OnClientPutInServer -= OnClientPutInServer;
        Core.Event.OnClientDisconnected -= OnClientDisconnected;
    }

    private void OnClientPutInServer(IOnClientPutInServerEvent @event)
    {
        if (!config.Enabled)
        {
            return;
        }

        var player = Core.PlayerManager.GetPlayer(@event.PlayerId);
        if (!IsPlayable(player))
        {
            return;
        }

        fovPreferences.Load(player!, config, playerCookiesApi);
    }

    private void OnClientDisconnected(IOnClientDisconnectedEvent @event)
    {
        fovPreferences.Remove(@event.PlayerId);
    }

    private void PrimeConnectedPlayers()
    {
        if (!config.Enabled)
        {
            return;
        }

        foreach (var player in Core.PlayerManager.GetAllPlayers())
        {
            if (!IsPlayable(player))
            {
                continue;
            }

            fovPreferences.Load(player, config, playerCookiesApi);
        }
    }
}
