using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Gemserk.Utilities.Editor;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Gemserk.BuildTools.Editor
{
    public class BuildConfigurationsWindow : AssetListBaseWindow
    {
        public BuildConfigurationsWindow() : base(typeof(BuildConfiguration))
        {
            
        }
        
        [MenuItem("Window/Gemserk/Build Configurations")]
        public static void ShowWindow()
        {
            // This method is called when the user selects the menu item in the Editor
            EditorWindow wnd = GetWindow<BuildConfigurationsWindow>();
            wnd.titleContent = new GUIContent("Build Configurations");
        }

        [MenuItem("Gemserk/Builds/Build Open Scenes")]
        public static void BuildCurrentScenes()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }
            
            var buildScenes = new List<string>();
            var mainScene = "";
            
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                buildScenes.Add(scene.path);

                if (i == 0)
                {
                    mainScene = scene.name;
                }
            }

            Build(new BuildPlayerOptions
            {
                target = BuildTarget.StandaloneWindows64,
                targetGroup = BuildTargetGroup.Standalone,
                options = BuildOptions.AutoRunPlayer,
            },Path.Combine($"builds/windows/dev/{mainScene}/", $"{PlayerSettings.productName}_{mainScene}.exe"), buildScenes.ToArray());
        }
        
        public static void Build(BuildPlayerOptions buildOptions, string path, string[] scenes)
        {
            buildOptions.locationPathName = path;
            buildOptions.scenes = scenes;

            var timerBuild = Stopwatch.StartNew();
            
            var buildReport = BuildPipeline.BuildPlayer(buildOptions);

            var buildSummary = new StringBuilder();
            buildSummary.AppendLine("BUILDSUMMARY");
            buildSummary.AppendFormat("Build: {0}\n", timerBuild.Elapsed.TotalSeconds);

            buildSummary.AppendLine("-----------");
            buildSummary.AppendLine("BUILD REPORT");
            buildSummary.AppendLine("-----------");
            foreach (var buildReportStep in buildReport.steps)
            {
                buildSummary.AppendFormat("Step: {0} - Duration: {1}\n", buildReportStep.name,
                    buildReportStep.duration.TotalSeconds);
            }

            var executableName = Path.GetFileNameWithoutExtension(path);
            Debug.LogFormat($"BUILD-{executableName}\n{buildSummary}" );

            if (buildReport.summary.result == BuildResult.Failed)
            {
                var errorReport = new StringBuilder();
                foreach (var step in buildReport.steps)
                {
                    foreach (var message in step.messages)
                    {
                        if (message.type == LogType.Error || message.type == LogType.Exception || message.type == LogType.Assert)
                        {
                            errorReport.AppendLine($"ERROR: step:{step.name} - {message.content}");
                        }
                    }
                }

                var errorMessage = $"Build Failed with :{buildReport.summary.totalErrors} errors";
                Debug.Log($"{errorMessage}\n{errorReport.ToString()}");
                throw new Exception(errorMessage);
            }
        }
    }
}