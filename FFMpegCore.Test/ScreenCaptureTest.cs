using System;
using System.Threading;
using System.Threading.Tasks;
using FFMpegCore.Arguments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FFMpegCore.Test
{
    [TestClass]
    public class ScreenCaptureTest
    {
        [TestMethod]
        public void CaptureScreenTest()
        {
            var testFilePath = @"C:\Users\benp\test.mp4";
            //var args = FFMpegArguments.FromScreenCapture(ScreenCaptureMethod.DShow, captureTarget: @"video=""UScreenCapture"":audio=""Stereo Mix""");
            var args = FFMpegArguments.FromScreenCapture(ScreenCaptureMethod.GdiGrab);
            var processor = args.OutputToFile(testFilePath);

            var task = processor.CancellableThrough(out var cancelEvent);
            
            Task.Run(() => Thread.Sleep(TimeSpan.FromSeconds(5))).ContinueWith((t) => cancelEvent.Invoke());

            var result = task.ProcessSynchronously();

            var x = FFProbe.Analyse(testFilePath);
            
            Assert.IsTrue(result, "Failed to record");
            Assert.IsTrue(x.Duration > TimeSpan.FromSeconds(1), "Resulting File too short");
        }
    }
}