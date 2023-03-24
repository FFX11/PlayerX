using PlayerX.ViewModel;

namespace PlayerX.Views;

public partial class VideoPage : ContentPage
{
    public VideoPage()
	{
		InitializeComponent();

        BindingContext = new VideoViewModel();
    }
}