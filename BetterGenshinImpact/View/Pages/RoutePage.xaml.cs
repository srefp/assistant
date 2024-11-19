using BetterGenshinImpact.ViewModel.Pages;
using System.Windows.Controls;

namespace BetterGenshinImpact.View.Pages;

public partial class RoutePage : UserControl
{
    private RoutePageViewModel ViewModel { get; }
    public RoutePage(RoutePageViewModel viewModel)
    {
        DataContext = ViewModel = viewModel;
        InitializeComponent();
    }
}
