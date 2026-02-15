using Cookies.Contract;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;

namespace BetterFOV;

[PluginMetadata(Id = "BetterFOV", Version = "1.0.0", Name = "BetterFOV", Author = "gyro", Description = "Configurable FOV manager with Cookies persistence")]
public sealed partial class BetterFOV : BasePlugin
{
    private const string ConfigFileName = "config.jsonc";
    private const string ConfigSectionName = "BetterFOV";
    private const string CookiesInterfaceKey = "Cookies.Player.v1";

    public static new ISwiftlyCore Core { get; private set; } = null!;

    private readonly List<Guid> registeredCommandIds = [];
    private readonly FOVPreferenceService fovPreferences = new();

    private FOVPluginConfig config = new();
    private IPlayerCookiesAPIv1? playerCookiesApi;

    public BetterFOV(ISwiftlyCore core) : base(core)
    {
    }

    public override void Load(bool hotReload)
    {
        Core = base.Core;

        config = LoadConfiguration();
        RegisterCommands();
        SubscribeCoreEvents();
        PrimeConnectedPlayers();
    }

    public override void Unload()
    {
        fovPreferences.FlushPending(config, playerCookiesApi);
        UnsubscribeCoreEvents();
        UnregisterCommands();
        fovPreferences.Clear();
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        playerCookiesApi = ResolveCookiesApi(interfaceManager);
    }

    public override void OnSharedInterfaceInjected(IInterfaceManager interfaceManager)
    {
        playerCookiesApi = ResolveCookiesApi(interfaceManager);
        PrimeConnectedPlayers();
    }

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
    }

    private static bool IsPlayable(IPlayer? player)
    {
        return player is { IsValid: true, IsFakeClient: false };
    }
} 
