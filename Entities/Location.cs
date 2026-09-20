namespace RouteTracker.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using RouteTracker.Models;

    /// <summary>
    /// Backend API Controller for telemetry data ingestion, saving and querying.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : ControllerBase
    {
        private readonly ITelemetryService _telemetryService;

        public TelemetryController(ITelemetryService telemetryService)
        {
            _telemetryService = telemetryService;
        }
        [HttpPost("journeys/start")]
        public async Task<ActionResult<JourneySession>> StartJourney([FromBody] StartJourneyDto dto)
        {
            var session = await _telemetryService.CreateJourneyAsync(dto.UserId, dto.UserName);
            return Ok(session);
        }
        [HttpPost("coordinates")]
        public async Task<IActionResult> SaveCoordinate([FromBody] CoordinatePoint point)
        {
            await _telemetryService.SaveCoordinateAsync(point);
            return StatusCode(201);
        }

        /// <summary>
        /// 3. Ingest a batch of coordinates for high-throughput buffering.
        /// POST /api/telemetry/coordinates/batch
        /// </summary>
        [HttpPost("coordinates/batch")]
        public async Task<IActionResult> SaveCoordinateBatch([FromBody] List<CoordinatePoint> points)
        {
            await _telemetryService.SaveBatchAsync(points);
            return Ok(new { insertedCount = points.Count });
        }

        /// <summary>
        /// 4. Complete an active journey session and compute aggregates.
        /// PUT /api/telemetry/journeys/{journeyId}/complete
        /// </summary>
        [HttpPut("journeys/{journeyId:guid}/complete")]
        public async Task<IActionResult> CompleteJourney(Guid journeyId, [FromBody] CompleteJourneyDto dto)
        {
            var result = await _telemetryService.CompleteJourneyAsync(journeyId, dto);
            return Ok(result);
        }

        /// <summary>
        /// 5. Save a custom marker tied to a specific user.
        /// POST /api/telemetry/markers
        /// </summary>
        [HttpPost("markers")]
        public async Task<ActionResult<CustomMarker>> CreateMarker([FromBody] CustomMarker marker)
        {
            var saved = await _telemetryService.SaveMarkerAsync(marker);
            return CreatedAtAction(nameof(GetMarkersByUser), new { userId = marker.UserId }, saved);
        }

        /// <summary>
        /// 6. Query coordinate trail for a specific user and journey.
        /// GET /api/telemetry/coordinates?userId={userId}&journeyId={journeyId}&routeId={routeId}
        /// </summary>
        [HttpGet("coordinates")]
        public async Task<ActionResult<List<CoordinatePoint>>> GetCoordinates(
            [FromQuery] string userId,
            [FromQuery] Guid? journeyId,
            [FromQuery] string? routeId)
        {
            var points = await _telemetryService.GetCoordinatesAsync(userId, journeyId, routeId);
            return Ok(points);
        }

        /// <summary>
        /// 7. Retrieve all custom markers created by a user.
        /// GET /api/telemetry/markers?userId={userId}
        /// </summary>
        [HttpGet("markers")]
        public async Task<ActionResult<List<CustomMarker>>> GetMarkersByUser([FromQuery] string userId)
        {
            var markers = await _telemetryService.GetMarkersByUserAsync(userId);
            return Ok(markers);
        }
    }
}