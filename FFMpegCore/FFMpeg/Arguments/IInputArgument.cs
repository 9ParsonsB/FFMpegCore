namespace FFMpegCore.Arguments
{
    public interface IInputArgument : IInputOutputArgument
    { }

    public enum ScreenCaptureMethod
    {
        /// <summary>
        /// Windows Only
        /// </summary>
        GdiGrab,
        
        /// <summary>
        /// Windows Only (DirectShow)
        /// </summary>
        DShow,
        
        /// <summary>
        /// MacOS Only
        /// </summary>
        avfoundation,
        
        /// <summary>
        /// X11 Only
        /// </summary>
        x11grab 
    }
}