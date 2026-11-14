using System;

namespace GUI.Services
{
    /// <summary>
    /// Proxy/Wrapper sang BUS.Services.AudioPlaybackCoordinator nhằm đảm bảo tương thích ngược.
    /// </summary>
    public class AudioPlaybackCoordinator : BUS.Services.AudioPlaybackCoordinator
    {
        public new static AudioPlaybackCoordinator Instance { get; } = new AudioPlaybackCoordinator();
    }
}
