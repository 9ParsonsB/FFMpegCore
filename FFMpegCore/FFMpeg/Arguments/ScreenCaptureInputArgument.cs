using System.Threading;
using System.Threading.Tasks;

namespace FFMpegCore.Arguments
{
    public class ScreenCaptureInputArgument : IInputArgument
    {
        public ScreenCaptureInputArgument(ScreenCaptureMethod method ,
            string? videoSize = null,
            int? xOffset = null,
            int? yOffset = null,
            bool showRegion = false,
            int captureFramerate = 30,
            bool showCursor = true,
            string? captureTarget = null
        )
        {
            ScreenCaptureMethod = method;
            VideoSize = videoSize;
            XOffset = xOffset;
            YOffset = yOffset;
            ShowRegion = showRegion;
            CaptureFramerate = captureFramerate;
            ShowCursor = showCursor;
            CaptureTarget = captureTarget;
        }

        /// <summary>
        /// Note: the -framerate argument and -r are not the same!
        /// </summary>
        public int CaptureFramerate { get; set; } = 30;

        /// <summary>
        /// Can be gdigrab or dshow 
        /// </summary>
        public ScreenCaptureMethod ScreenCaptureMethod { get; set; }
        //-f gdigrab -framerate 30 -i desktop output.mkv
        // -offset_x 10 -offset_y 20 -video_size 640x480 -show_region 1

        public string Text =>
            $"-f {ScreenCaptureMethod.ToString("G").ToLower()} " + $"-framerate {CaptureFramerate} " + $"{xOffsetArgument} "
          + $"{yOffsetArgument} " + $"{VideoSizeArgument} " + $"{ShowRegionArgument} -i {input} ";

        /// <summary>
        /// The target device to capture.
        /// If using DirectShow (dshow), this can be multiple devices (i.e. Video & Audio)
        /// see https://trac.ffmpeg.org/wiki/Capture/Desktop for more info
        /// </summary>
        public string? CaptureTarget { get; set; } = null;
        
        private string input => ScreenCaptureMethod == ScreenCaptureMethod.GdiGrab ? (CaptureTarget ?? "desktop") : 
        dShowInput;

        /// <summary>
        /// Can also handle Audio if you append `:audio="Microphone"`
        /// or any other audio device (including Stereo Mix)
        /// </summary>
        private string dShowInput => CaptureTarget ?? $"video=\"screen-capture-recorder\"";

        public string? VideoSize = null;
        private string VideoSizeArgument => VideoSize != null ? $"-video_size {VideoSize}" : "";


        public int? XOffset = null;
        private string xOffsetArgument => XOffset.HasValue ? $"-offset_x {XOffset.Value}" : string.Empty;


        public int? YOffset = null;
        public string yOffsetArgument => YOffset.HasValue ? $"-offset_y {YOffset.Value}" : string.Empty;

        public bool ShowRegion = false;
        private string ShowRegionArgument => $"-show_region {(ShowRegion ? "1" : "0")}";

        public bool ShowCursor = false;
        private string ShowCursorArgument => ScreenCaptureMethod == ScreenCaptureMethod.GdiGrab ? $"-draw_mouse {(ShowCursor?"1":"0")}" : "";

        public void Pre()
        { }

        public Task During(CancellationToken? cancellationToken = null) => Task.CompletedTask;

        public void Post()
        { }
    }
}