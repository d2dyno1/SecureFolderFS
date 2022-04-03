using System.Collections;
using System.Diagnostics;

namespace SecureFolderFS.Shared.Helpers
{
    public static class DebuggingHelpers
    {
        public static void PrintEnumerable(IEnumerable enumerable)
        {
#if !DEBUG
            return;
#endif
            if (!Constants.ENABLE_DEBUG_LOGGING)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                Debug.WriteLine(item);
            }
        }

        public static void PrintEnumerableInline(IEnumerable enumerable)
        {
#if !DEBUG
            return;
#endif
            if (!Constants.ENABLE_DEBUG_LOGGING)
            {
                return;
            }

            foreach (var item in enumerable)
            {
                Debug.Write($"{item} ");
            }
        }

        public static T OutputToDebugAndContinue<T>(this T target, string message)
        {
#if !DEBUG
            return;
#endif
            if (!Constants.ENABLE_DEBUG_LOGGING)
            {
                return target;
            }

            Debug.WriteLine(message);

            return target;
        }

        public static void OutputToDebug(string message)
        {
#if !DEBUG
            return;
#endif
            if (!Constants.ENABLE_DEBUG_LOGGING)
            {
                return;
            }

            Debug.WriteLine(message);

            return;
        }
    }
}
