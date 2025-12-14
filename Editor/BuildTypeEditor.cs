
using UnityEditor;
using UnityEngine;
using Common;
using UnityEditor.Build;

[CustomEditor(typeof(BuildTypeSO))]
public class BuildTypeSOEditor : Editor
{

    public override void OnInspectorGUI()
    {
        BuildTypeSO buildTypeSO = (BuildTypeSO)target;

        // نمایش منوی کشویی برای انتخاب BuildType
        var lastBuildType = buildTypeSO.buildType;
        buildTypeSO.buildType =
            (BuildTypeSO.BuildType)EditorGUILayout.EnumPopup("Build Type", buildTypeSO.buildType);

        // بررسی اینکه آیا برای تست بیلد است یا خیر
        buildTypeSO.testMarketBuild = EditorGUILayout.Toggle("Is Test Market Build", buildTypeSO.testMarketBuild);
        if (GUILayout.Button("Apply"))
        {
            ApplyBuildType();
        }
        if (GUILayout.Button("Apply Gradle"))
        {
            GradleManager.ApplyGradleTemplates(buildTypeSO.buildType);
        }
        if (GUILayout.Button("V7 Build for Selected Market"))
        {
            MultiStoreBuild.BuildForMarket(buildTypeSO.GetBuildType(),MultiStoreBuild.BuildArchitecture.V7);
        }
        if (GUILayout.Button("AllInOne Build for Selected Market"))
        {
            MultiStoreBuild.BuildForMarket(buildTypeSO.GetBuildType(),MultiStoreBuild.BuildArchitecture.AllInOne);
        }
        if (GUILayout.Button("Seperated Build for Selected Market"))
        {
            MultiStoreBuild.BuildForMarket(buildTypeSO.GetBuildType(),MultiStoreBuild.BuildArchitecture.Seperated);
        }
        if (GUILayout.Button("Seperated Build for Both Market"))
        {
            buildTypeSO.buildType = BuildTypeSO.BuildType.Bazzar;
            MultiStoreBuild.BuildForMarket(buildTypeSO.GetBuildType(),MultiStoreBuild.BuildArchitecture.Seperated);
            buildTypeSO.buildType = BuildTypeSO.BuildType.Myket;
            MultiStoreBuild.BuildForMarket(buildTypeSO.GetBuildType(),MultiStoreBuild.BuildArchitecture.Seperated);
        }

    }

    private void ApplyBuildType()
    {
        // دسترسی به BuildTypeSO
        BuildTypeSO buildTypeSO = (BuildTypeSO)target;

        // تغییر سمبل‌های دیفاین برای بازار انتخاب‌شده
        DefineSymbolsHelper.SetSymbolsForMarket(NamedBuildTarget.Android, buildTypeSO.GetBuildType());
        Debug.Log($"✅ Applied define symbols for {buildTypeSO.GetBuildType()}");
    }

    // private void BuildForMarket(BuildTypeSO.BuildType market)
    // {
    //     var bundleId = Application.identifier;
    //
    //     // تنظیمات کی‌استور و پسوردها (این‌ها باید از قبل تنظیم شوند)
    //     PlayerSettings.Android.keystorePass = "13811381";
    //     PlayerSettings.Android.keyaliasPass = "13811381";
    //     PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, bundleId);
    //
    //     // اعمال قالب‌های Gradle برای بازار انتخابی
    //     GradleManager.ApplyGradleTemplates(market);
    //
    //     // مسیر خروجی بیلد
    //     string path = $"Builds/{market}/MyApp-{market}.apk";
    //     if (!System.IO.Directory.Exists($"Builds/{market}"))
    //         System.IO.Directory.CreateDirectory($"Builds/{market}");
    //
    //     // اجرای بیلد
    //     BuildPipeline.BuildPlayer(
    //         EditorBuildSettings.scenes,
    //         path,
    //         BuildTarget.Android,
    //         BuildOptions.None
    //     );
    //
    //     Debug.Log($"✅ Build for {market} completed → {path}");
    // }
}
