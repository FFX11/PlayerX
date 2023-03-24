using CommunityToolkit.Maui.Views;
using PlayerX.Models;
using PlayerX.ViewModel;

namespace PlayerX.Views;

public partial class SongPage : ContentPage
{
	
    public SongPage()
	{
		InitializeComponent();
		BindingContext = new SongsViewModel();
    }
}
