using System.IO;
using Newtonsoft.Json.Linq;

namespace OMP.LSWTSS;

public static class BuildGalaxyUnleashed
{
    public static void Execute()
    {
        var galaxyUnleashedDistDirPath = GetGalaxyUnleashedDistDirPath.Execute();

        if (Directory.Exists(galaxyUnleashedDistDirPath))
        {
            Directory.Delete(galaxyUnleashedDistDirPath, true);
        }

        Directory.CreateDirectory(galaxyUnleashedDistDirPath);

        var galaxyUnleashedRuntimeDotnetPackageDirPath = Path.Combine(
            GetDotnetWorkspaceDirPath.Execute(),
            "galaxy-unleashed-runtime"
        );

        var galaxyUnleashedOverlayJsPackageDirPath = Path.Combine(
            GetJsWorkspaceDirPath.Execute(),
            "galaxy-unleashed-overlay"
        );

        BuildDotnetPackage.Execute(galaxyUnleashedRuntimeDotnetPackageDirPath);
        BuildJsPackage.Execute(galaxyUnleashedOverlayJsPackageDirPath);

        CopyDirectory.IO.CopyDirectory(
            Path.Combine(
                galaxyUnleashedRuntimeDotnetPackageDirPath,
                "bin",
                "Release",
                "net8.0"
            ),
            galaxyUnleashedDistDirPath
        );

        File.Copy(
            Path.Combine(
                galaxyUnleashedOverlayJsPackageDirPath,
                "dist",
                "index.html"
            ),
            Path.Combine(
                galaxyUnleashedDistDirPath,
                "index.html"
            )
        );

        File.WriteAllText(
            Path.Combine(
                galaxyUnleashedDistDirPath,
                "mod.json"
            ),
            new JObject
            {
                ["name"] = "Galaxy Unleashed",
                ["actions"] = new JArray
                {
                    new JObject
                    {
                        ["typeId"] = "register-scripting-module-action",
                        ["payload"] = new JObject
                        {
                            ["typeName"] = "OMP.LSWTSS.GalaxyUnleashed",
                            ["assemblyPath"] = "omp-lswtss-galaxy-unleashed-runtime.dll",
                        },
                    },
                },
                ["dependencies"] = new JArray
                {
                    new JObject
                    {
                        ["id"] = "c-func-hook1",
                    },
                    new JObject
                    {
                        ["id"] = "c-api1",
                    },
                    new JObject
                    {
                        ["id"] = "input-hook1",
                    },
                    new JObject
                    {
                        ["id"] = "overlay1",
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );

        File.WriteAllText(
            Path.Combine(
                galaxyUnleashedDistDirPath,
                "keybinds.json"
            ),
            new JObject
            {
                ["_keybindsInfo"] = new JArray
                {
                    new JObject
                    {
                        ["Id"] = "OpenMenu",
                        ["KeyCode"] = 112,
                    },
                    new JObject
                    {
                        ["Id"] = "QuickAction",
                        ["KeyCode"] = 113,
                    },
                },
            }.ToString(Newtonsoft.Json.Formatting.Indented)
        );
    }
}