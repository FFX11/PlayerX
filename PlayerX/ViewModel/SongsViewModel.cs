using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlayerX.Models;
using PlayerX.Views;
using System.Collections.ObjectModel;

namespace PlayerX.ViewModel;

public partial class SongsViewModel : ObservableObject
{
	[ObservableProperty]
	public string mytext = string.Empty;

    [ObservableProperty]
    ObservableCollection<string> mySongs = new();

    public AsyncRelayCommand<MusicInfo> SelectedCommand { get; }

    [ObservableProperty]
    public ObservableCollection<MusicInfo> myMusicInfo = new();

    [ObservableProperty]
    private string mySelectedItem = string.Empty;

    MusicInfo musicInfo = new();

    
    private string playPng = "play.png";

    [ObservableProperty]
    IMediaElement _mediaElements;
    
    public SongsViewModel()
    {
        //MediaElement mediaElementX = new ();
        _mediaElements = new MediaElement();

        SelectedCommand = new AsyncRelayCommand<MusicInfo>(Selected);
        RootDirectory();
    }


    MusicInfo selectedSong;

    public MusicInfo SelectedSong
    {
        get  => selectedSong; 
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
        var directories = Directory.GetDirectories("/storage/emulated/0/");

        foreach (var directory in directories.Where(x =>
        !x.Equals("/storage/emulated/0/Android") &&
        !x.Equals("/storage/emulated/0/Alarms") &&
        !x.Equals("/storage/emulated/0/Ringtones") &&
        !x.Equals("/storage/emulated/0/Notifications")))
        {
            foreach (string musicFilePath in Directory.EnumerateFiles(directory, "*.mp3", SearchOption.AllDirectories))
            {
                musicInfo = new();
                //Log.Info("Music File", "Music file path is " + musicFilePath);

                MySongs.Add(musicFilePath);
                musicInfo.SongName = musicFilePath.Substring(musicFilePath.LastIndexOf('/') + 1);
                musicInfo.Artist = "unknown";
                musicInfo.Location = musicFilePath;
                MyMusicInfo.Add(musicInfo);
            }
        }

        App.FolderSongsViewModel.SongsList = MyMusicInfo;
    }
}
