﻿using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Web.WebView2.Wpf;
using Wpf.Ui.Controls;

namespace BetterGenshinImpact.View.Controls.Webview;

public class WebpageWindow : Window
{
    public WebpagePanel? Panel => Content as WebpagePanel;

    public WebView2 WebView => Panel!.WebView;

    public WebpageWindow()
    {
        WebpagePanel wp = new()
        {
            Margin = new(8, 8, 0, 8)
        };

        Content = wp;
        // Background = new SolidColorBrush(Color.FromRgb(0x20, 0x20, 0x20));
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        TryApplySystemBackdrop();
    }

    private void TryApplySystemBackdrop()
    {
        if (WindowBackdrop.IsSupported(WindowBackdropType.Mica))
        {
            WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
            return;
        }

        if (WindowBackdrop.IsSupported(WindowBackdropType.Tabbed))
        {
            WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Tabbed);
            return;
        }
    }

    public void NavigateToUri(Uri uri)
    {
        Panel?.NavigateToUri(uri);
    }

    public void NavigateToHtml(string html)
    {
        Panel?.NavigateToHtml(html);
    }

    public void NavigateToFile(string path)
    {
        Panel?.NavigateToFile(path);
    }
}
