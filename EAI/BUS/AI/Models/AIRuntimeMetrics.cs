using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BUS.AI.Models
{
    public class AIRuntimeMetrics
    {
        private long _totalRequests;
        private long _successfulRequests;
        private long _failedRequests;
        private long _totalLatencyMs;
        private readonly ConcurrentQueue<AIRequestLog> _recentLogs = new();
        private const int MaxLogRetention = 50;

        public long TotalRequests => Interlocked.Read(ref _totalRequests);
        public long SuccessfulRequests => Interlocked.Read(ref _successfulRequests);
        public long FailedRequests => Interlocked.Read(ref _failedRequests);

        public double AverageLatencyMs
        {
            get
            {
                long total = TotalRequests;
                if (total == 0) return 0;
                return (double)Interlocked.Read(ref _totalLatencyMs) / total;
            }
        }

        public double ErrorRatePercent
        {
            get
            {
                long total = TotalRequests;
                if (total == 0) return 0.0;
                return (double)FailedRequests / total * 100.0;
            }
        }

        public void Record(AIRequestLog log)
        {
            Interlocked.Increment(ref _totalRequests);
            if (log.Success)
            {
                Interlocked.Increment(ref _successfulRequests);
            }
            else
            {
                Interlocked.Increment(ref _failedRequests);
            }

            Interlocked.Add(ref _totalLatencyMs, log.LatencyMs);

            _recentLogs.Enqueue(log);
            while (_recentLogs.Count > MaxLogRetention && _recentLogs.TryDequeue(out _)) { }
        }

        public List<AIRequestLog> GetRecentLogs()
        {
            return _recentLogs.Reverse().ToList();
        }
    }
}
