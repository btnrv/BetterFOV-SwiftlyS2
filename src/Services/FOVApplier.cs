using SwiftlyS2.Shared.Players;

namespace BetterFOV;

internal static class FOVApplier
{
    public static bool TryApply(IPlayer player, int fov)
    {
        var controller = player.Controller;
        if (controller is null)
        {
            return false;
        }

        controller.DesiredFOV = (uint)fov;
        controller.DesiredFOVUpdated();
        return true;
    }
}
