using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace voyantisServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueuesController : ControllerBase // Consider renaming to QueuesController
    {
        private static readonly ConcurrentDictionary<string, ConcurrentQueue<string>> _queues = new();

        [HttpPost("{queue_name}")] // POST to specific queue
        public IActionResult Post(string queue_name, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            if (!_queues.ContainsKey(queue_name))
            {
                _queues[queue_name] = new ConcurrentQueue<string>();
            }

            _queues[queue_name].Enqueue(message);
            Console.WriteLine($"Enqueued message: {message} to queue: {queue_name}");
            return Ok(); // Or CreatedAtRoute(...)
        }



        [HttpGet("{queue_name}")] // GET from specific queue with timeout
        public async Task<IActionResult> Get(string queue_name, int? timeout = 10000)
        {
            if (!_queues.ContainsKey(queue_name))
            {
                return NotFound($"Queue '{queue_name}' not found."); // 404 Not Found
            }

            var queue = _queues[queue_name];
            string message = null;

            await Task.Run(async () =>
            {
                var startTime = DateTime.Now;
                while (DateTime.Now - startTime < TimeSpan.FromMilliseconds(timeout.Value) && !queue.TryDequeue(out message))
                {
                    await Task.Delay(100); // Poll every 100ms
                }
            });


            if (message != null)
            {
                return Ok(message);
            }
            else
            {
                return NoContent(); // 204 No Content if timeout elapsed
            }

        }

        [HttpGet("list")] // Get list of queues and their sizes
        public IActionResult GetQueues() // New Method
        {
            var queueInfo = _queues.Select(kvp => new { Name = kvp.Key, Count = kvp.Value.Count });
            return Ok(queueInfo);
        }
    }
}