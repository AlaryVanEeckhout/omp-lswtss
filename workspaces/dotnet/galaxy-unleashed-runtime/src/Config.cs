using OMP.LSWTSS.CApi1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMP.LSWTSS;

public static class Config
{
    private const string configPath = @"mods/galaxy-unleashed-workaround/config.ini";

    private static INIFile configFile;

    // default values from OMP boosters
    public static int JetpackSpeed = 5; // TurboSpeedMultiplier
    public static int JumpSpeed = 2; // JumpHeightMultiplier

    public static int ToggleBattleKey = (int)PInvoke.User32.VirtualKey.VK_INSERT; // useless for now
    public static int ReloadKey = (int)PInvoke.User32.VirtualKey.VK_END;

    public static string NPCDefault = "";
    public static int NPCnextKey = (int)PInvoke.User32.VirtualKey.VK_F9;
    public static int NPCpreviousKey = (int)PInvoke.User32.VirtualKey.VK_F10;
    public static int PlaceNPCKey = (int)PInvoke.User32.VirtualKey.VK_F3;
    public static int PlaceSpawnerKey = (int)PInvoke.User32.VirtualKey.VK_F4;
    public static int PlaceFlagKey = (int)PInvoke.User32.VirtualKey.VK_F5;
    public static int ClearNPCKey = (int)PInvoke.User32.VirtualKey.VK_F6;

    public static int BoostJumpKey = (int)PInvoke.User32.VirtualKey.VK_F7;
    public static int BoostJetpackKey = (int)PInvoke.User32.VirtualKey.VK_F8;

    public static void LoadConfig()
    {
        configFile = new INIFile(configPath);

        NPCDefault = configFile.IniReadValue("Config", "NPCDefault");
        Int32.TryParse(configFile.IniReadValue("Config", "JetpackSpeed"), out JetpackSpeed);
        Int32.TryParse(configFile.IniReadValue("Config", "JumpSpeed"), out JumpSpeed);

        Int32.TryParse(configFile.IniReadValue("Inputs", "ToggleBattleKey"), out ToggleBattleKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "ReloadKey"), out ReloadKey);

        Int32.TryParse(configFile.IniReadValue("Inputs", "NPCnextKey"), out NPCnextKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "NPCpreviousKey"), out NPCpreviousKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "PlaceNPCKey"), out PlaceNPCKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "PlaceSpawnerKey"), out PlaceSpawnerKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "PlaceFlagKey"), out PlaceFlagKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "ClearNPCKey"), out ClearNPCKey);

        Int32.TryParse(configFile.IniReadValue("Inputs", "BoostJumpKey"), out BoostJumpKey);
        Int32.TryParse(configFile.IniReadValue("Inputs", "BoostJetpackKey"), out BoostJetpackKey);
    }
}
