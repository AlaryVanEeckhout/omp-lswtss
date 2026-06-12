using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OMP.LSWTSS;

public partial class GalaxyUnleashed
{
    static class Keybinds
    {
        static KeybindInfo[]? _keybindsInfo = null;

        static public ushort GetKeyCode(string id)
        {
            if (_keybindsInfo == null)
            {
                return 0;
            }

            foreach (KeybindInfo keybindInfo in _keybindsInfo)
            {
                if (keybindInfo.Id == id)
                {
                    return keybindInfo.KeyCode;
                }
            }

            return 0;
        }

        static public void LoadConfig(string path)
        {
            string jsonString = File.ReadAllText(Directory.GetCurrentDirectory() + @"\" + path);

            KeybindsInfo? keybindsInfo = JsonConvert.DeserializeObject<KeybindsInfo>(jsonString);
            if (keybindsInfo == null || keybindsInfo._keybindsInfo == null)
            {
                return;
            }

            _keybindsInfo = keybindsInfo._keybindsInfo;
        }
    }
}