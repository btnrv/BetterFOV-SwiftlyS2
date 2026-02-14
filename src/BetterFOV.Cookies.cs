using Cookies.Contract;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace BetterFOV;

public sealed partial class BetterFOV
{
    private IPlayerCookiesAPIv1? ResolveCookiesApi(IInterfaceManager interfaceManager)
    {
        if (!interfaceManager.HasSharedInterface(CookiesInterfaceKey))
            return null;

        try
        {
            return interfaceManager.GetSharedInterface<IPlayerCookiesAPIv1>(CookiesInterfaceKey);
        }
        catch
        {
            return null;
        }
    }
}
