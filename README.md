# SimpleChart
A simple single series ready-made WPF chart window which launches in a separate thread. Built on top of [LiveCharts](https://lvcharts.net/).

![simple chart img](https://github.com/Vladimir-Pavelka/SimpleChart/blob/master/chart.png "Simple chart")

## Usage
1. Install the [SimpleChart](https://www.nuget.org/packages/SimpleChart/) nuget package.

2. Create an instance of the chart, which will run as a standalone window in a separate thread.
```csharp
ISimpleChart chart = SimpleChart.LaunchInNewThread();
```
3. Start adding `double` value points to the chart continuously,
```csharp
chart.AddPoint(4.2);
chart.AddPoint(-3.7);
...
```
   or set the whole point series at once
```csharp
chart.SetSeries(new [] { 7, 4.2, -1 });
```

For customization options, explore the `ISimpleChart` interface. 
This is a simple 'ready to go' chart, if you need more customization, please use [LiveCharts](https://lvcharts.net/) directly.
