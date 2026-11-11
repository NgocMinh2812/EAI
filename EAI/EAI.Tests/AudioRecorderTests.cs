using System;
using System.IO;
using BUS.AI;
using Xunit;

namespace EAI.Tests
{
    public class AudioRecorderTests
    {
        [Fact]
        public void MicrophoneRecorder_InstantiatesAndCleansUpSafely()
        {
            using var recorder = new MicrophoneRecorder();
            Assert.False(recorder.IsRecording);
            Assert.Null(recorder.CurrentTempFile);

            // Cleanup should not throw even if no temp file
            var ex = Record.Exception(() => recorder.CleanupTempFile());
            Assert.Null(ex);
        }
    }
}

