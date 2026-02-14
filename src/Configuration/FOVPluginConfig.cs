namespace BetterFOV;

public sealed class FOVPluginConfig
{
    public bool Enabled { get; set; } = true;
    public string Prefix { get; set; } = "[gold][BetterFOV][default] ";
    public int MinFOV { get; set; } = 80;
    public int MaxFOV { get; set; } = 130;
    public int DefaultFOV { get; set; } = 90;
    public string CookieKey { get; set; } = "betterfov";
    public List<string> Commands { get; set; } = ["fov"];
}
