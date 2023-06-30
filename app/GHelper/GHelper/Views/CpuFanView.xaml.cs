using GHelper.Injection;
using GHelper.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Ninject;

namespace GHelper.Views
{
    public sealed partial class CpuFanView
    {
        public CpuFanViewModel ViewModel { get; private set; } = Services.ResolutionRoot.Get<CpuFanViewModel>();
        
        public CpuFanView()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
