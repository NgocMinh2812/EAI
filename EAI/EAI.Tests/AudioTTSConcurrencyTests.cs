using System;
using System.Threading;
using BUS.Services;
using Xunit;

#pragma warning disable CA1416 // Validate platform compatibility

namespace EAI.Tests
{
    public class AudioTTSConcurrencyTests
    {
        [Fact]
        public void NotifyMediaPlayerStarting_StopsAllSpeechAndCancelsActiveTTS()
        {
            var coordinator = new AudioPlaybackCoordinator();
            coordinator.IsSpeaking = true;
            int initialCancelCount = coordinator.SpeechCancelCount;

            coordinator.NotifyMediaPlayerStarting();

            Assert.True(coordinator.IsMediaPlayerPlaying, "MediaPlayer should be marked as playing.");
            Assert.False(coordinator.IsSpeaking, "TTS should be marked as stopped.");
            Assert.True(coordinator.SpeechCancelCount > initialCancelCount, "Speech cancel count must increment on NotifyMediaPlayerStarting.");
        }

        [Fact]
        public void SpeakAsync_PausesRegisteredMediaPlayerAndEnforcesMutualExclusion()
        {
            var coordinator = new AudioPlaybackCoordinator();
            bool mediaPlayerPaused = false;

            Action pauser = () =>
            {
                mediaPlayerPaused = true;
            };

            coordinator.RegisterActiveMediaPlayer(pauser);
            coordinator.NotifyMediaPlayerStarting();
            Assert.True(coordinator.IsMediaPlayerPlaying);
            Assert.False(mediaPlayerPaused);

            // Starting TTS speech must automatically pause running MediaPlayer
            coordinator.SpeakAsync("Testing mutual exclusion between audio player and synthetic speech.", speed: 1.0, volume: 100);

            Assert.True(mediaPlayerPaused, "MediaPlayer pause callback must be invoked when TTS starts speaking.");
            Assert.False(coordinator.IsMediaPlayerPlaying, "IsMediaPlayerPlaying must be false once TTS takes over.");
            Assert.True(coordinator.MediaPlayerPauseCount >= 1);
        }

        [Fact]
        public void SpeakAsync_RapidCalls_CancelsPreviousSpeechToPreventOverlap()
        {
            var coordinator = new AudioPlaybackCoordinator();

            coordinator.SpeakAsync("First rapid utterance.", speed: 1.0, volume: 100);
            int cancelCountAfterFirst = coordinator.SpeechCancelCount;

            // Second call in rapid succession
            coordinator.SpeakAsync("Second rapid utterance canceling first.", speed: 1.0, volume: 100);
            int cancelCountAfterSecond = coordinator.SpeechCancelCount;

            Assert.True(cancelCountAfterSecond > cancelCountAfterFirst,
                "Subsequent SpeakAsync call must invoke StopAllSpeech() to cancel previous utterances, avoiding audio cacophony.");
        }

        [Fact]
        public void UnregisterActiveMediaPlayer_CleansUpPauserCallback()
        {
            var coordinator = new AudioPlaybackCoordinator();
            bool mediaPlayerPaused = false;
            Action pauser = () => { mediaPlayerPaused = true; };

            coordinator.RegisterActiveMediaPlayer(pauser);
            coordinator.UnregisterActiveMediaPlayer(pauser);

            coordinator.SpeakAsync("Testing unregistered state.", speed: 1.0, volume: 100);

            Assert.False(mediaPlayerPaused, "Unregistered pauser must not be called.");
        }

        [Fact]
        public void StopAllSpeech_CanBeCalledRepeatedlyWithoutException()
        {
            var coordinator = new AudioPlaybackCoordinator();
            var ex = Record.Exception(() =>
            {
                coordinator.StopAllSpeech();
                coordinator.StopAllSpeech();
                coordinator.StopAllSpeech();
            });

            Assert.Null(ex);
            Assert.False(coordinator.IsSpeaking);
        }
    }
}
