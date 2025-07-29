using System;
using UnityEngine;

namespace Game.Scripts
{
    /// <summary>
    /// Assign this script to a GameObject in the scene to capture screenshots.
    /// </summary>
    public class Capturer : MonoBehaviour
    {
        public KeyCode captureKey = KeyCode.P;
        public RandomNameType randomNameType = RandomNameType.Guid;

        // TODO: Make this only show in the inspector if the randomNameType is Numeric
        [SerializeField] private int startNumber = 0;
        [SerializeField] private int endNumber = 1000;

        public string starNameString = "acc";

        public void Update()
        {
            if (Input.GetKeyDown(captureKey))
            {
                var namefile = starNameString;
                switch (randomNameType)
                {
                    case RandomNameType.Guid:
                        var guid = Guid.NewGuid();
                        namefile += guid.ToString();
                        break;
                    case RandomNameType.Numeric:
                        namefile += UnityEngine.Random.Range(startNumber, endNumber).ToString();
                        break;
                    case RandomNameType.DateTime:
                        namefile += DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                        break;
                }

                namefile += ".png";
                ScreenCapture.CaptureScreenshot(namefile);
                Debug.Log("CAPTURED PICTURE: " + namefile);
            }
        }
    }

    public enum RandomNameType
    {
        Guid,
        Numeric,
        DateTime,
    }
}