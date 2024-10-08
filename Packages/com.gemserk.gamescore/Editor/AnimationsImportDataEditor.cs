﻿using System.Collections.Generic;
using System.IO;
using Game.Definitions;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    [CustomEditor(typeof(AnimationsImportData))]
    public class AnimationsImportDataEditor : UnityEditor.Editor
    {
        private List<string> sourceFiles = new ();

        private Vector2 scroll;
        
        private GUIContent importIcon, searchIcon, openFolderIcon;
        
        private void OnEnable()
        {
            ReloadFiles();
            
            importIcon = EditorGUIUtility.IconContent("Download-Available");
            searchIcon = EditorGUIUtility.IconContent("Search Icon");
            openFolderIcon = EditorGUIUtility.IconContent("FolderOpened Icon");
        }

        private void ReloadFiles()
        {
            var importData = target as AnimationsImportData;
            sourceFiles = importData.GetSourceFiles();
        }

        public override void OnInspectorGUI()
        {
            var importData = target as AnimationsImportData;

            EditorGUI.BeginChangeCheck();
        
            importData.defaultFps = EditorGUILayout.FloatField("Fps", importData.defaultFps);
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Source Folder", GUILayout.Width(90));
            importData.sourceFolder = EditorGUILayout.TextField(importData.sourceFolder, GUILayout.ExpandWidth(true));
         
            var selectSourceFolder = new GUIContent(openFolderIcon.image, "Select Source Folder");
            if (GUILayout.Button(selectSourceFolder, GUILayout.Width(25), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                var absolutePath = Path.GetFullPath(importData.sourceFolder, Application.dataPath);
                var newFolder = EditorUtility.OpenFolderPanel("Source Folder", absolutePath, "");
                if (!string.IsNullOrEmpty(newFolder))
                {
                    importData.sourceFolder = Path.GetRelativePath(Application.dataPath, newFolder);
                    ReloadFiles();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Output Folder", GUILayout.Width(90));
            importData.outputFolder = EditorGUILayout.TextField(importData.outputFolder, GUILayout.ExpandWidth(true));
   
            var selectOutputGuiContent = new GUIContent(openFolderIcon.image, "Select Output Folder");
            if (GUILayout.Button(selectOutputGuiContent, GUILayout.Width(25), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                var absolutePath = Path.GetFullPath(importData.outputFolder, Application.dataPath);
                var newFolder = EditorUtility.OpenFolderPanel("Output Folder", absolutePath, "");

                if (!string.IsNullOrEmpty(newFolder))
                {
                    importData.outputFolder = Path.GetRelativePath(Application.dataPath, newFolder);
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            scroll = EditorGUILayout.BeginScrollView(scroll);
            foreach (var folderAbsolutePath in sourceFiles)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(folderAbsolutePath);
                var fileName = Path.GetFileName(folderAbsolutePath);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(fileNameWithoutExtension, GUI.skin.textArea);
                
                var buttonGuiContent = new GUIContent(importIcon.image, $"Generate {fileName}");
                if (GUILayout.Button(buttonGuiContent, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight), 
                        GUILayout.Width(25)))
                {
                    var sourceAssetPath = Path.GetRelativePath(Application.dataPath, folderAbsolutePath);
                    // var sourceAssetPath = folderAbsolutePath.Replace(Application.dataPath, "");
                    
                    AnimationCreatorTool.CreateAnimationAssetFromFolder(Path.Combine("Assets", sourceAssetPath),
                        importData.outputFolder, importData.defaultFps);
                    
                    // AsepriteImporter.ImportFile(asepriteExecutablePath, file, 
                    //     importData.outputAbsolutePath, importData.format);
                    AssetDatabase.Refresh();
                }

                var previousAnimation = AssetDatabase.LoadAssetAtPath<AnimationsAsset>(Path.Combine("Assets", importData.outputFolder,
                    fileNameWithoutExtension) +  ".asset");
                
                EditorGUI.BeginDisabledGroup(!previousAnimation);
                var openFolderGuiIcon = new GUIContent(searchIcon.image, "Ping");
                if (GUILayout.Button(openFolderGuiIcon, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight),
                        GUILayout.Width(25)))
                {
                    EditorGUIUtility.PingObject(previousAnimation);
                }
                EditorGUI.EndDisabledGroup();
                
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(importData);
            }
            
            if (GUILayout.Button("Reload List", GUILayout.ExpandWidth(true)))
            {
                ReloadFiles();
                Repaint();
            }
        }
    }
}