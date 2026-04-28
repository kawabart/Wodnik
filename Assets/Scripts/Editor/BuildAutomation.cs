#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class BuildAutomation
    {
        [MenuItem("Itch/Build and upload")]
        public static void BuildGame()
        {
            var scenes = new[] { "Assets/Scenes/TestSceneForVisuals.unity" }; // Scenes to include in the build
            BuildOptions buildOptions = BuildOptions.None; // Build Options (e.g. Development build)
            BuildTarget buildTarget = BuildTarget.WebGL; // Target platform

            var buildPathRelative = Path.Combine("Builds", buildTarget.ToString()); // Output path
            
            var buildReport = BuildPipeline.BuildPlayer(scenes, buildPathRelative, buildTarget, buildOptions);
            if (buildReport.summary.result != BuildResult.Succeeded)
            {
                Debug.LogError("Build Failed");
                return;
            }

            var buildPathAbsolute = buildReport.summary.outputPath;
            //Debug.Log(System.Environment.GetEnvironmentVariable("PATH"));     // If Unity doesn't see butler in your PATH,
            // you probably need to restart Unity Hub and reopen the project

            ButlerItchUpload(buildPathAbsolute, "kawabart/nymph-the-hairy-tale:web");
            Debug.Log("Upload complete");
        }

        private static void ButlerItchUpload(string directoryPath, string itchTarget)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "butler",
                Arguments = $"push \"{directoryPath}\" {itchTarget}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
        }
    }
}
#endif
