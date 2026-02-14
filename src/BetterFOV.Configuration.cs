using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace BetterFOV;

public sealed partial class BetterFOV
{
    private static readonly Regex LegacyColorTagRegex = new(@"\{([a-z_]+)\}", RegexOptions.Compiled);

    private FOVPluginConfig LoadConfiguration()
    {
        Core.Configuration
            .InitializeJsonWithModel<FOVPluginConfig>(ConfigFileName, ConfigSectionName)
            .Configure(builder =>
            {
                builder.AddJsonFile(
                    Core.Configuration.GetConfigPath(ConfigFileName),
                    optional: false,
                    reloadOnChange: false
                );
            });

        var loadedConfig = Core.Configuration.Manager
            .GetSection(ConfigSectionName)
            .Get<FOVPluginConfig>() ?? new FOVPluginConfig();

        loadedConfig.Prefix = NormalizeChatColorTags(loadedConfig.Prefix);
        return loadedConfig;
    }

    private static string NormalizeChatColorTags(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return string.Empty;
        }

        return LegacyColorTagRegex.Replace(text, "[$1]");
    }
}
