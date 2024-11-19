using BetterGenshinImpact.ViewModel.Pages;
using System.Windows.Controls;

namespace BetterGenshinImpact.View.Pages;

public partial class AutoTpPage
{
    private AutoTpPageViewModel ViewModel { get; }
    public AutoTpPage(AutoTpPageViewModel viewModel, HotKeyPageViewModel hotKeyPageViewModel)
    {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
        
        // hotKeyPageViewModel 放在这里是为了在首页就初始化热键
    }
}
