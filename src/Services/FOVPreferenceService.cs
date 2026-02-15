using Cookies.Contract;
using SwiftlyS2.Shared.Players;

namespace BetterFOV;

internal sealed class FOVPreferenceService
{
    private readonly Dictionary<int, int> fovByPlayerId = [];
    private readonly Dictionary<long, int> pendingFovBySteamId = [];

    public void Load(IPlayer player, FOVPluginConfig config, IPlayerCookiesAPIv1? cookiesApi)
    {
        var fov = config.DefaultFOV;
        var steamId = (long)player.SteamID;

        if (pendingFovBySteamId.TryGetValue(steamId, out var pendingFov))
        {
            fovByPlayerId[player.PlayerID] = Clamp(pendingFov, config);
            return;
        }

        if (cookiesApi is not null)
        {
            cookiesApi.Load(player);
            if (cookiesApi.Has(player, config.CookieKey))
            {
                fov = cookiesApi.Get<int>(player, config.CookieKey);
            }
        }

        fovByPlayerId[player.PlayerID] = Clamp(fov, config);
    }

    public int GetOrLoad(IPlayer player, FOVPluginConfig config, IPlayerCookiesAPIv1? cookiesApi)
    {
        if (fovByPlayerId.TryGetValue(player.PlayerID, out var fov))
        {
            return fov;
        }

        Load(player, config, cookiesApi);
        return fovByPlayerId[player.PlayerID];
    }

    public int Set(IPlayer player, int value, FOVPluginConfig config, IPlayerCookiesAPIv1? cookiesApi)
    {
        var clampedFOV = Clamp(value, config);
        fovByPlayerId[player.PlayerID] = clampedFOV;
        var steamId = (long)player.SteamID;

        if (cookiesApi is null)
        {
            return clampedFOV;
        }

        if (!config.EnableCookiesCaching)
        {
            pendingFovBySteamId.Remove(steamId);
            cookiesApi.Set(player, config.CookieKey, clampedFOV);
            cookiesApi.Save(player);
            return clampedFOV;
        }

        pendingFovBySteamId[steamId] = clampedFOV;

        return clampedFOV;
    }

    public int FlushPending(FOVPluginConfig config, IPlayerCookiesAPIv1? cookiesApi)
    {
        if (cookiesApi is null || pendingFovBySteamId.Count == 0)
        {
            return 0;
        }

        var flushedCount = 0;
        foreach (var (steamId, fov) in pendingFovBySteamId.ToArray())
        {
            cookiesApi.Set(steamId, config.CookieKey, fov);
            pendingFovBySteamId.Remove(steamId);
            flushedCount++;
        }

        return flushedCount;
    }

    public void Remove(int playerId)
    {
        fovByPlayerId.Remove(playerId);
    }

    public void Clear()
    {
        fovByPlayerId.Clear();
        pendingFovBySteamId.Clear();
    }

    private static int Clamp(int value, FOVPluginConfig config)
    {
        return Math.Clamp(value, config.MinFOV, config.MaxFOV);
    }
}
