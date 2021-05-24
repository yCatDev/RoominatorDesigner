#if UNITY_WEBGL && !UNITY_EDITOR
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Logic
{
    public class AppLoader : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void AppReady();

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            AppReady();
        }
    }
}
#endif