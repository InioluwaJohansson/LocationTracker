using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
[Route("LocationTracker/[controller]")]
[ApiController]
public class RouteLogController : Controller
{
    IRouteLogService _routeLogService;
    IJWTAuthentication _jwtAuthentication;
    public RouteLogController(IRouteLogService routeLogService, IJWTAuthentication jwtAuthentication)
    {
        _routeLogService = routeLogService;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateRouteLog")]
    public async Task<IActionResult> CreateRouteLog([FromBody] CreateRouteLogDto createRouteLogDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        createRouteLogDto.UserId = user.Id;
        var routeLog = await _routeLogService.CreateRouteLog(createRouteLogDto);
        return Ok(routeLog);
    }
    [Authorize]
    [HttpPost("UpdateRouteLog")]
    public async Task<IActionResult> UpdateRouteLog(UpdateRouteLogDto updateRouteLogDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var routeLog = await _routeLogService.UpdateRouteLog(updateRouteLogDto);
        return Ok(routeLog);
    }
    [Authorize]
    [HttpGet("GetAllRouteLogs")]
    public async Task<IActionResult> GetAllRouteLogs()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var routeLog = await _routeLogService.GetAllRouteLogs();
        return Ok(routeLog);
    }
    [Authorize]
    [HttpGet("DeleteRouteLog")]
    public async Task<IActionResult> DeleteRouteLog(int routeLogId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var routeLog = await _routeLogService.DeleteRouteLog(routeLogId);
        return Ok(routeLog);
    }
    [Authorize]
    [HttpGet("GetRouteLogsByDate")]
    public async Task<IActionResult> GetRouteLogsByDate(DateTime? date)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var routeLog = await _routeLogService.GetAllRouteLogs();
        return Ok(routeLog);
    }
}
