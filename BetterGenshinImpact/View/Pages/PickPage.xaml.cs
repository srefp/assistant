using BetterGenshinImpact.ViewModel.Pages;
using System.Windows.Controls;

namespace BetterGenshinImpact.View.Pages;

public partial class PickPage : UserControl
{
    private PickPageViewModel ViewModel { get; }
    public PickPage(PickPageViewModel viewModel)
    {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
    }
}
