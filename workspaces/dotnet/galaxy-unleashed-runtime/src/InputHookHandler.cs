using OMP.LSWTSS.CApi1;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static OMP.LSWTSS.CApi1.CollectableSelector;

namespace OMP.LSWTSS;

public partial class GalaxyUnleashed : IDisposable
{
    /*
    private static Overlay1 this_overlay;
    public void Stuff()
    {
        this_overlay = _overlay;
    }
    */
    public static bool InputHookClientHandler(in InputHook1.NativeMessage inputHookClientNativeMessage)
    {
        string char_id = "0";//_instance._charactersInfo[0].Id;
        if (_instance == null)
        {
            return false;
        }

        if (_instance._modeState is PlayModeState)
        {
            if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F1)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    //this_overlay.ChromiumWebBrowser.LoadUrl("file:///C:/Program%20Files%20(x86)/Steam/steamapps/common/LEGO%20Star%20Wars%20-%20The%20Skywalker%20Saga/mods/galaxy-unleashed/index.html");
                    var closestNpcSpawnerInPlayerEntityRange = _instance!._closestNpcSpawnerInPlayerEntityRange;

                    if (closestNpcSpawnerInPlayerEntityRange != null)
                    {
                        _instance!._modeState = new MenuModeState
                        {
                            Config = new()
                            {
                                NavigateParams = new()
                                {
                                    To = $"/menu-mode/menu/npcs/manage-spawners/{closestNpcSpawnerInPlayerEntityRange.State.Id}",
                                    Search = [],
                                }
                            }
                        };
                    }
                    else
                    {
                        _instance!._modeState = new MenuModeState
                        {
                            Config = new()
                            {
                                NavigateParams = new()
                                {
                                    To = "/menu-mode/menu",
                                    Search = [],
                                }
                            }
                        };
                    }
                }

                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.ReloadKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    Config.LoadConfig(); // reload config
                    for (int i = 0; i < _instance._charactersInfo?.Length; i++)
                    {
                        if (_instance._charactersInfo[i].PrefabResourcePath.IndexOf(Config.NPCDefault) != -1)
                        {
                            char_id = _instance._charactersInfo[i].Id;
                            Process.Start(new ProcessStartInfo //using System.Diagnostics;
                            {
                                FileName = "explorer",
                                Arguments = "http://localhost/" + char_id
                            });
                            break;
                        }
                    }
                }
            }
            else if (inputHookClientNativeMessage.WParam == Config.ToggleBattleKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    _instance.LoadOverlay(false);
                }
            }
            else if (inputHookClientNativeMessage.WParam == Config.PlaceNPCKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        var _ = new SpawnNpcTask(
                            new()
                            {
                                CharacterId = char_id,
                                CharacterOverrideFactionId = null
                            },
                            true,
                            _instance._playerEntityLastPosition.Value,
                            isGlobal: true
                        );
                    }
                }
                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.PlaceSpawnerKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        var _ = new NpcSpawner(
                            new()
                            {
                                MaxNpcsCount = 20,
                                AreNpcsBattleParticipants = true,
                                NpcSpawningIntervalSeconds = 5,
                                NpcPreset = new()
                                {
                                    CharacterId = Config.NPCDefault,
                                    CharacterOverrideFactionId = null
                                }
                            },
                            _instance._playerEntityLastPosition.Value
                        );
                    }
                }
                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.PlaceFlagKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        _instance._battle.PlaceFlag(_instance._playerEntityLastPosition.Value);
                    }
                }
                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.ClearNPCKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        //_instance._battle.Dispose(); // reset battle?
                        _instance._npcSpawners.Clear(); // despawn?
                        _instance._npcs.Clear(); // despawn?
                    }
                }
                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.BoostJumpKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        _instance._jumpBooster.State.Config.JumpHeightMultiplier = Config.JumpSpeed;
                        _instance._jumpBooster.State.IsEnabled = !_instance._jumpBooster.State.IsEnabled;
                    }
                }
                return true;
            }
            else if (inputHookClientNativeMessage.WParam == Config.BoostJetpackKey)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        _instance._jetpackBooster.State.Config.TurboSpeedMultiplier = Config.JetpackSpeed;
                        _instance._jetpackBooster.State.IsEnabled = !_instance._jetpackBooster.State.IsEnabled;
                    }
                }
                return true;
            }
        }
        else if (_instance._modeState is MenuModeState)
        {
            if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_ESCAPE)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    _instance!._modeState = new PlayModeState
                    {
                    };
                }

                return true;
            }
        }
        else if (_instance._modeState is QuickSpawnNpcsModeState quickSpawnNpcsModeState)
        {
            if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F1)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    _instance!._modeState = new MenuModeState
                    {
                        Config = new()
                        {
                            NavigateParams = new()
                            {
                                To = "/menu-mode/menu/npcs/quick-spawn",
                                Search = [],
                            }
                        }
                    };
                }

                return true;
            }
            else if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F2)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        var _ = new SpawnNpcTask(
                            quickSpawnNpcsModeState.Config.NpcPreset,
                            quickSpawnNpcsModeState.Config.IsNpcBattleParticipant,
                            _instance._playerEntityLastPosition.Value,
                            isGlobal: true
                        );
                    }
                }

                return true;
            }
        }
        else if (_instance._modeState is CreateNpcSpawnersModeState createNpcSpawnersModeState)
        {
            if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F1)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    _instance!._modeState = new MenuModeState
                    {
                        Config = new()
                        {
                            NavigateParams = new()
                            {
                                To = "/menu-mode/menu/npcs/create-spawners",
                                Search = [],
                            }
                        }
                    };
                }

                return true;
            }
            else if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F2)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        var _ = new NpcSpawner(
                            createNpcSpawnersModeState.Config.NpcSpawner,
                            _instance._playerEntityLastPosition.Value
                        );
                    }
                }

                return true;
            }
        }
        else if (_instance._modeState is ManageBattleFlagModeState)
        {
            if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F1)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    _instance!._modeState = new MenuModeState
                    {
                        Config = new()
                        {
                            NavigateParams = new()
                            {
                                To = "/menu-mode/menu/battle",
                                Search = [],
                            }
                        }
                    };
                }

                return true;
            }
            else if ((PInvoke.User32.VirtualKey)inputHookClientNativeMessage.WParam == PInvoke.User32.VirtualKey.VK_F2)
            {
                if ((PInvoke.User32.WindowMessage)inputHookClientNativeMessage.Type == PInvoke.User32.WindowMessage.WM_KEYUP)
                {
                    if (_instance._playerEntityLastPosition != null)
                    {
                        _instance._battle.PlaceFlag(_instance._playerEntityLastPosition.Value);
                    }
                }

                return true;
            }
        }
        return false;
    }
}