using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlayerX.Models;
using PlayerX.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerX.ViewModel;

[QueryProperty("FolderPath" ,"FolderPath")]
public partial class FolderSongsViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> folders = new();

    [ObservableProperty]
    ObservableCollection<string> files = new();

    [ObservableProperty]
    string folderPath;

    public MusicInfo CurrSong = new();

    [ObservableProperty]
    public ObservableCollection<MusicInfo> songsList = new();

    public ObservableCollection<string> SongsFiles = new();

    [ObservableProperty]
    List<string> folderSongs = new();

    [ObservableProperty]
    string folderName;

    public AsyncRelayCommand<MusicInfo> SelectedCommand { get; }

    [ObservableProperty]
    private IMediaElement _mediaElements;
    public FolderSongsViewModel()
    {
        RootDirectory();

        SelectedCommand = new AsyncRelayCommand<MusicInfo>(Selected);
    }

    MusicInfo selectedSong;

    public MusicInfo SelectedSong
    {
        get => selectedSong;
        set => SetProperty(ref selectedSong, value);
    }

    async Task Selected(MusicInfo song)
    {
        if (song == null)
        {
            return;
        }
        App.FolderSongsViewModel.CurrSong = song;
        await Shell.Current.Navigation.PushModalAsync(new MainPage());
    }

    private void RootDirectory()
    {
        //await CheckAndRequestLocationPermission();

        if (App.FolderSongsViewModel != null)
        {
            FolderName = App.FolderSongsViewModel.FolderPath;
        }

        if (FolderName != null)
        {
            foreach (string musicFilePath in Directory.EnumerateFiles(FolderName, "*.mp3", SearchOption.AllDirectories))
            {
                MusicInfo selectedSong = new();

                var songName = Path.GetFileName(musicFilePath); //musicFilePath.Substring(musicFilePath.LastIndexOf('/') + 1);
                selectedSong.SongName = songName;

                selectedSong.Artist = "unknown";
                selectedSong.Location = musicFilePath;

                SongsList.Add(selectedSong);
            }
        }
    }
}
