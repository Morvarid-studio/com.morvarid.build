using System.IO;
using Common;
using UnityEngine;

public class GradleManager
{
    private const string TEMPLATE_PATH = "Packages/com.morvarid.build/GradleTemplates";
    private const string UNITY_GRADLE_PATH = "Assets/Plugins/Android";
    private const string UNITY_EDITOR_PATH = "Assets/Plugins/Android/Editor";

    public static void ApplyGradleTemplates(BuildTypeSO.BuildType marketFolder)
    {
        string sourcePath = Path.Combine(TEMPLATE_PATH, marketFolder.ToString());

        if (!Directory.Exists(sourcePath))
        {
            Debug.LogError($"❌ Gradle template folder not found: {sourcePath}");
            return;
        }

        Directory.CreateDirectory(UNITY_GRADLE_PATH);
        Directory.CreateDirectory(UNITY_EDITOR_PATH);

        // 🧹 پاک‌سازی فایل‌های قدیمی Gradle
        string[] gradleFiles =
        {
            "mainTemplate.gradle",
            "launcherTemplate.gradle",
            "baseProjectTemplate.gradle",
            "gradleTemplate.properties",
            "proguard-user.txt",
            "settingsTemplate.gradle"
        };

        foreach (var oldFile in gradleFiles)
        {
            var fullPath = Path.Combine(UNITY_GRADLE_PATH, oldFile);
            DeleteWithMeta(fullPath);
        }

        // 🧹 پاک‌سازی فایل‌های dependency قدیمی
        foreach (var file in Directory.GetFiles(UNITY_EDITOR_PATH))
        {
            if (file.ToLower().Contains("dependenc"))
            {
                DeleteWithMeta(file);
                Debug.Log($"🗑️ Removed old dependency: {Path.GetFileName(file)}");
            }
        }

        // 📦 کپی فایل‌های جدید
        foreach (var file in Directory.GetFiles(sourcePath))
        {
            string fileName = Path.GetFileName(file);

            if (fileName.ToLower().Contains("dependenc"))
            {
                string dest = Path.Combine(UNITY_EDITOR_PATH, fileName);
                File.Copy(file, dest, true);
                Debug.Log($"📦 Applied dependency file: {fileName}");
            }
            else
            {
                string dest = Path.Combine(UNITY_GRADLE_PATH, fileName);
                File.Copy(file, dest, true);
                Debug.Log($"✅ Applied gradle template: {fileName}");
            }
        }

        Debug.Log($"🎯 Gradle templates applied successfully for {marketFolder}");
    }

    private static void DeleteWithMeta(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            string meta = path + ".meta";
            if (File.Exists(meta))
                File.Delete(meta);
        }
    }
}
