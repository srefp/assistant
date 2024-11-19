﻿using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.Test.Dataset;
using BetterGenshinImpact.Test.Simple;
using BetterGenshinImpact.Test.Simple.AllMap;
using BetterGenshinImpact.Test.Simple.Track;
using BetterGenshinImpact.Test.View;
using System.Windows;

namespace BetterGenshinImpact.Test;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Global.StartUpPath = @"D:\HuiPrograming\Projects\CSharp\MiHoYo\BetterGenshinImpact\BetterGenshinImpact\bin\x64\Debug\net8.0-windows10.0.22621.0";
    }

    private void ShowCameraRecWindow(object sender, System.Windows.RoutedEventArgs e)
    {
        new CameraRecWindow().Show();
    }

    private void ShowHsvTestWindow(object sender, System.Windows.RoutedEventArgs e)
    {
        new HsvTestWindow().Run();
    }

    private void DoMapPuzzle(object sender, System.Windows.RoutedEventArgs e)
    {
        MapPuzzle.Put();
    }

    private void DoOcrTest(object sender, System.Windows.RoutedEventArgs e)
    {
        OcrTest.TestYap();
    }

    private void DoMatchTemplateTest(object sender, System.Windows.RoutedEventArgs e)
    {
        MatchTemplateTest.Test();
    }

    private void DoMatchTest(object sender, System.Windows.RoutedEventArgs e)
    {
        // KeyPointMatchTest.Test();
        // EntireMapTest.Test();
        EntireMapTest.Storage();
        // BigMapMatchTest.Test();

        // FeatureTransfer.Transfer();
    }

    private void MapDrawTeleportPoint(object sender, RoutedEventArgs e)
    {
        MapTeleportPointDraw.Draw();
    }

    private void GenAvatarData(object sender, RoutedEventArgs e)
    {
        AvatarClassifyGen.GenAll();
    }

    private void AutoCookTestCase(object sender, RoutedEventArgs e)
    {
        AutoCookTest.Test();
    }

    private void MapPathView(object sender, RoutedEventArgs e)
    {
        MapPathTest.Test();
    }

    private void ZoomOut(object sender, RoutedEventArgs e)
    {
        ScaleTest.ZoomOutTest();
    }

    private void GenAvatarDataT(object sender, RoutedEventArgs e)
    {
        AvatarClassifyTransparentGen.GenAll();
    }
}
