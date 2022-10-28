using System.Diagnostics;

namespace OTService
{
    public class JustDelay
    {
        public void DoSomething()
        {
            using var activity = MyActivitySource.Instance.StartActivity("OTServiceCall");
            activity?.AddTag("Success", "True");
            Task.Delay(1000).Wait();
            activity?.Stop();
        }

    }

    public class MyActivitySource
    {
        public static string name = nameof(MyActivitySource);
        public static ActivitySource Instance = new ActivitySource(name);
    }
}