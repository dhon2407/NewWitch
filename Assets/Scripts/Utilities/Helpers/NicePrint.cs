namespace Utilities.Helpers
{
    public static class NicePrint
    {
        public static string NiceString(this int[] list)
        {
            var res = string.Empty;
            foreach (var i in list)
            {
                res += i.ToString() + " ";
            }

            return res;
        }
    }
}