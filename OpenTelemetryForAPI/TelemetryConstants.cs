﻿using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace OpenTelemetryForAPI
{
    public static class TelemetryConstants
    {
        /// <summary>
        /// The name of the <see cref="ActivitySource"/> that is going to produce our traces and
        /// the <see cref="Meter"/> that is going to produce our metrics.
        /// </summary>
        public const string MyAppSource = "OpenTelemetry API";

        public static readonly ActivitySource DemoTracer = new ActivitySource(MyAppSource);

        public static readonly Meter OTmeter = new Meter(MyAppSource);

        public static readonly Counter<long> HitsCounter =
            OTmeter.CreateCounter<long>("IndexHits", "hits", "number of hits to API");
        



    }
}
