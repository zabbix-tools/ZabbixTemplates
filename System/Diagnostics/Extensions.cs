namespace System.Diagnostics
{
    public static class Extensions
    {
        public static bool IsValidForExport(this PerformanceCounter pdhCounter)
        {
            return !(
                pdhCounter.CounterName == pdhCounter.CategoryName
                || pdhCounter.CounterName == "No name"
                || pdhCounter.CounterName == "Not displayed");            
        }
    }
}
