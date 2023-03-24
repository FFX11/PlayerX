using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlayerX.Models;
using PlayerX.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerX.ViewModel;

public partial class FolderVideosViewModel : ObservableObject
{
    public string FolderName;
    VideoModel selectedSong;

    [ObservableProperty]
    ObservableCollection<VideoModel> videoList = new();
    public AsyncRelayCommand<VideoModel> SelectedCommand { get; }

    public FolderVideosViewModel()
    {
        RootDirectory();

        SelectedCommand = new AsyncRelayCommand<VideoModel>(Selected);
    }

    public VideoModel SelectedSong
    {
        get => selectedSong;
        set => SetProperty(ref selectedSong, value);
    }

    async Task Selected(VideoModel video)
    {
        if (video == null)
        {
            return;
        }
        await Shell.Current.Navigation.PushModalAsync(new VideoPlayer(video));
    }


    private void RootDirectory()
    {
        //await CheckAndRequestLocationPermission();

        if (App.VideoModel != null)
        {
            FolderName = App.VideoModel.FolderPath;
        }

        if (FolderName != null)
        {
            foreach (string FilePath in Directory.EnumerateFiles(FolderName, "*.*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(".mp4") || x.EndsWith(".wmv")))
            {
                VideoModel selectedFolder = new();

                selectedFolder.VideoName = FilePath.Substring(FilePath.LastIndexOf('/') + 1);
                
                selectedFolder.Location = FilePath;

                VideoList.Add(selectedFolder);
            }
        }
    }
}
