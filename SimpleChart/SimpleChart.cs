namespace SimpleChart
{
    using System.Threading;
    using System.Windows.Threading;

    public static class SimpleChart
    {
        /// <summary>
        /// Launches a new chart window in a separate thread. The thread finishes when the chart window is closed.
        /// </summary>
        public static ISimpleChart LaunchInNewThread()
        {
            SimpleChartImpl chartWindow = null;

            var chartThread = new Thread(() =>
            {
                chartWindow = new SimpleChartImpl();
                chartWindow.Closed += (s, e) => chartWindow.Dispatcher.InvokeShutdown();
                chartWindow.Show();
                Dispatcher.Run();
            });

            chartThread.SetApartmentState(ApartmentState.STA);
            chartThread.IsBackground = true;
            chartThread.Start();

            while (chartWindow == null) Thread.Sleep(1);

            return chartWindow;
        }
    }
}