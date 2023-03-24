using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Timers;
using PlayerX.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace PlayerX.ViewModel;
[QueryProperty("Object1", "Object1")]
[QueryProperty("ImageUrl", "ImageUrl")]
[QueryProperty("Song", "Song")]
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string textLabel = string.Empty;

    readonly string pausePng = "pause.png";
    readonly string playPng = "play.png";

    [ObservableProperty]
    string imageUrl;

    [ObservableProperty]
    MusicInfo song;
    
    [ObservableProperty]
    string mp3Title = string.Empty;

    [ObservableProperty]
    ObservableCollection<string> mp3Files = new();

    [ObservableProperty]
    ObservableCollection<string> myFolders = new();

    MusicInfo musicInfo = new();

    string mp3File;
    string mp3 = string.Empty;
    int count = 0;
    bool isPlaying = false;
    double duration;
    bool SongDisposed;
    string processedFile;


    string folderPath;

    [ObservableProperty]
    private IMediaElement _mediaElements;
    public MainViewModel(IMediaElement mediaEle)
    {
        MediaElements = mediaEle;
        SongDisposed = false;
        
        ImageUrl = playPng;
        RootDirectory();
    }
    
    private void RootDirectory()
    {
        folderPath = App.FolderSongsViewModel.FolderPath;

        if (folderPath != null)
        {
            foreach (string musicFilePath in Directory.EnumerateFiles(folderPath, "*.mp3", SearchOption.AllDirectories))
            {
                Mp3Files.Add(musicFilePath);
            }


            if (Mp3Files.Count() > 0)
            {
                mp3 = Mp3Files[0];
            }

            return;
        }

        var directories = Directory.GetDirectories("/storage/emulated/0/");

        foreach (var directory in directories.Where(x =>
        !x.Equals("/storage/emulated/0/Android") &&
        !x.Equals("/storage/emulated/0/Alarms") &&
        !x.Equals("/storage/emulated/0/Ringtones") &&
        !x.Equals("/storage/emulated/0/Notifications")))
        {

            foreach (string musicFilePath in Directory.EnumerateFiles(directory, "*.mp3", SearchOption.AllDirectories))
            {

                if (musicFilePath != null)
                {
                    musicInfo = new();

                    Mp3Files.Add(musicFilePath);
                    musicInfo.SongName = musicFilePath.Substring(musicFilePath.LastIndexOf('/') + 1);
                    musicInfo.Artist = "unknown";
                    musicInfo.Location = musicFilePath;
                    Mp3Files.Add(musicInfo.SongName);
                }
            }
        
        }

        
    }

    [RelayCommand]
    private void Play()
    {
        ImageUrl = string.Empty;
        ImageUrl = pausePng;

        if (isPlaying && MediaElements.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
        {
            ImageUrl = string.Empty;
            ImageUrl = playPng;

            MediaElements.Pause();
            isPlaying = false;
        }
        else
        {
            ImageUrl = string.Empty;
            ImageUrl = pausePng;

            if (MediaElements.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Paused)
            {
                MediaElements.Play();
                isPlaying = true;

                return;
            }

            if (Song != null)
            {
                processedFile = Song.Location;

                Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
                TextLabel = Mp3Title;
                MediaElements.Source = processedFile;

                MediaElements.Play();

                isPlaying = true;

                return;
            }

            processedFile = Mp3Files[count];

            Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
            TextLabel = Mp3Title;
            MediaElements.Source = processedFile;

            MediaElements.Play();

            isPlaying = true;
        }
    }


    [RelayCommand]
    private void Stop()
    {
        ImageUrl = playPng;
        MediaElements.Stop();
        isPlaying = false;
    }

    [RelayCommand]
    private void NextSong()
    {
        if (count < Mp3Files.Count() - 1)
        {
            if (isPlaying)
            {
                ImageUrl = pausePng;
                MediaElements.Stop();
                count++;
                processedFile = Mp3Files[count];
                Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
                TextLabel = Mp3Title;
                MediaElements.Source = processedFile;
                MediaElements.Play();

                isPlaying = true;
            }
            else
            {
                ImageUrl = pausePng;
                if (MediaElements != null)
                {
                    MediaElements.Stop();
                }
                count++;
                processedFile = Mp3Files[count];
                Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
                TextLabel = Mp3Title;
                MediaElements.Source = processedFile;

                MediaElements.Play();
                isPlaying = true;
            }
        }
    }

    [RelayCommand]
    private void PrevioustSong()
    {
        if (count > 0)
        {
            if (isPlaying)
            {
                ImageUrl = pausePng;
                MediaElements.Stop();
                count--;
                processedFile = Mp3Files[count];
                Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
                TextLabel = Mp3Title;
                MediaElements.Source = processedFile;

                MediaElements.Play();
                isPlaying = true;
            }
            else
            {
                ImageUrl = pausePng;
                MediaElements.Stop();
                count--;
                processedFile = Mp3Files[count];
                Mp3Title = processedFile.Substring(Mp3Title.LastIndexOf('/') + 1);
                TextLabel = Mp3Title;
                MediaElements.Source = processedFile;

                MediaElements.Play();
                isPlaying = true;
            }
        }
    }
}
