using PlayerX.Models;

namespace PlayerX.Views;

public partial class VideoPlayer : ContentPage
{
    string folderPath;

    public VideoPlayer(VideoModel folder)
	{
		InitializeComponent();

        DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;
        if (folder != null)
        {
            MediaVideo.Source = folder.Location;
        }
    }

    private void Current_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
    {
        if (DeviceDisplay.Current.MainDisplayInfo.Rotation == DisplayRotation.Rotation90)
        {
            MediaVideo.HeightRequest = 400;
        }
        if (DeviceDisplay.Current.MainDisplayInfo.Rotation == DisplayRotation.Rotation0)
        {
            MediaVideo.ScaleY = 1.0f;
            MediaVideo.ScaleX = 1.0f;
        }
    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        MediaVideo.Stop();
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {

    }
}