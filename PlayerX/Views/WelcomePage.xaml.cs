using PlayerX.ViewModel;
namespace PlayerX.Views;

public partial class WelcomePage : ContentPage
{

    string currStat;
	public WelcomePage()
	{
		InitializeComponent();

        CheckPermission();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
       await CheckPermission();

        if (currStat == "Granted")
        {
            IsVisible = false;
        }
    }

    public  async Task<PermissionStatus> CheckPermission()
    {
        string stats;
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        currStat = status.ToString();

        if (currStat == "Granted")
        {
            IsVisible = false;

            App.FolderSongsViewModel = new FolderSongsViewModel();
            App.VideoModel = new VideoViewModel();
        }
            

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.StorageRead>();

        stats= status.ToString();

        currStat = stats;
        
        return status;
    }
}