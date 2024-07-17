using System.IO;
using UnityEngine;

namespace Gemserk.Utilities
{
    public static class ScreenshotsUtilities
    {
        private const string ScreenshotsRootFolder = "screenshots";
        
        public static string GenerateScreenshotName(int width, int height)
        {
            var date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return $"screenshot_{width}x{height}_{date}.png";
        }
        
        public static string TakeScreenshot()
        {
            var rootFolder = Path.Combine(Application.dataPath, "..", ScreenshotsRootFolder);
        
            if (!Directory.Exists(rootFolder))
            {
                Directory.CreateDirectory(rootFolder);
            }
            
            var screenshotName = GenerateScreenshotName(Screen.width, Screen.height);
            var screenshotFileName = Path.Combine(rootFolder, screenshotName);
            ScreenCapture.CaptureScreenshot(screenshotFileName);

            return screenshotFileName;
        }        
    }
}