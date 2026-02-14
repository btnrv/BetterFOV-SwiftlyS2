using Cookies.Contract;
using SwiftlyS2.Shared.Players;

namespace BetterFOV;

internal sealed class FOVPreferenceService
{
    private readonly Dictionary<int, int> fovByPlayerId = [];

    public void Load(IPlayer player, FOVPluginConfig config, IPlayerCookiesAPIv1? cookiesApi)
    {
        var fov = config.DefaultFOV;

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

        if (cookiesApi is not null)
        {
            cookiesApi.Set(player, config.CookieKey, clampedFOV);
            cookiesApi.Save(player);
        }

        return clampedFOV;
    }

    public void Remove(int playerId)
    {
        fovByPlayerId.Remove(playerId);
    }

    public void Clear()
    {
        fovByPlayerId.Clear();
    }

    private static int Clamp(int value, FOVPluginConfig config)
    {
        return Math.Clamp(value, config.MinFOV, config.MaxFOV);
    }
}
