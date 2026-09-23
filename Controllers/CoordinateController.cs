using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
[Route("LocationTracker/[controller]")]
[ApiController]
public class CoordinateController : Controller
{
    ICoordinateService _coordinateService;
    IJWTAuthentication _jwtAuthentication;
    public CoordinateController(ICoordinateService coordinateService, IJWTAuthentication jwtAuthentication)
    {
        _coordinateService = coordinateService;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateCoordinate")]
    public async Task<IActionResult> CreateCoordinate([FromBody] CreateCoordinateDto createCoordinateDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        createCoordinateDto.UserId = user.Id;
        var coordinate = await _coordinateService.CreateCoordinate(createCoordinateDto);
        return Ok(coordinate);
    }
    [Authorize]
    [HttpPost("CreateBatchCoordinates")]
    public async Task<IActionResult> CreateBatchCoordinates(List<CreateCoordinateDto> createCoordinateDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var coordinate = await _coordinateService.CreateBatchCoordinates(createCoordinateDto);
        return Ok(coordinate);
    }
    [Authorize]
    [HttpGet("GetCoordinatesByJourneyId")]
    public async Task<IActionResult> GetCoordinatesByJourneyId(int journeyId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var coordinate = await _coordinateService.GetCoordinatesByJourneyId(journeyId);
        return Ok(coordinate);
    }
    [Authorize]
    [HttpGet("GetCoordinatesByRouteId")]
    public async Task<IActionResult> GetCoordinatesByRouteId(int journeyId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var coordinate = await _coordinateService.GetCoordinatesByRouteId(journeyId);
        return Ok(coordinate);
    }
    [Authorize]
    [HttpGet("GetCoordinatesByDate")]
    public async Task<IActionResult> GetCoordinatesByDate(DateTime? date)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var coordinate = await _coordinateService.GetCoordinatesByRouteId(1);
        return Ok(coordinate);
    }
}
