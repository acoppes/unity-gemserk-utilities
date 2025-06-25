using UnityEngine;

namespace Game
{
    public static class CameraExtensions
    {
        public static Bounds GetBounds(this Camera camera)
        {
            var screenAspect = Screen.width / (float) Screen.height;
            var cameraHeight = camera.orthographicSize * 2;
            var bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }
        
        // this one uses the target texture directly...
        // private Bounds GetBounds2(Camera camera)
        // {
        //     // var screenAspect = camera.targetTexture.width / (float) camera.targetTexture.height;
        //     // var widthFactor = Screen.width / (float) camera.targetTexture.width;
        //     
        //     var heightFactor = Screen.height / (float) camera.targetTexture.height;
        //
        //     // var widthFactor = 2;
        //     // var heightFactor = 2;
        //     
        //     // var zoom = camera.orthographicSize * 2 * heightFactor;
        //     // var zoom = 16f * heightFactor;
        //     var dpi = 16f;
        //     
        //     var bounds = new Bounds(
        //         camera.transform.position,
        //         new Vector3(camera.targetTexture.width / dpi, camera.targetTexture.height / dpi, 0));
        //     return bounds;
        // }
    }
}