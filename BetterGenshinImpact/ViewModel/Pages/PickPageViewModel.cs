using BetterGenshinImpact.Core.Config;
using BetterGenshinImpact.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;

namespace BetterGenshinImpact.ViewModel.Pages;

public partial class PickPageViewModel : ObservableObject, IViewModel
{
    private readonly ILogger<RoutePageViewModel> _logger;
    public AllConfig Config { get; set; }


    public PickPageViewModel(IConfigService configService)
    {
     
    }

}