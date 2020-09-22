using System;
using System.Collections.Generic;
using MEC;

namespace Utilities.Helpers
{
    public static class CallTiming
    {
        public static void DelayInvoke(this Action action, float delay)
        {
            Timing.RunCoroutine(DelayCallInvoke(delay));

            IEnumerator<float> DelayCallInvoke(float d)
            {
                yield return Timing.WaitForSeconds(d);
                action?.Invoke();
            }
        }
        
        public static void DelayInvoke(this Action action, float delay, string tag)
        {
            Timing.RunCoroutine(DelayCallInvoke(delay), tag);

            IEnumerator<float> DelayCallInvoke(float d)
            {
                yield return Timing.WaitForSeconds(d);
                action?.Invoke();
            }
        }
    }
}