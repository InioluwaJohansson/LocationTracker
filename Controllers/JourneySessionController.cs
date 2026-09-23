using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
[Route("LocationTracker/[controller]")]
[ApiController]
public class JourneySessionController : Controller
{
    IJourneySessionService _journeySessionService;
    IJWTAuthentication _jwtAuthentication;
    public JourneySessionController(IJourneySessionService journeySessionService, IJWTAuthentication jwtAuthentication)
    {
        _journeySessionService = journeySessionService;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateJourneySession")]
    public async Task<IActionResult> CreateJourneySession([FromBody] CreateJourneySessionDto createJourneySessionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        createJourneySessionDto.UserId = user.Id;
        var journeySession = await _journeySessionService.CreateJourneySession(createJourneySessionDto);
        return Ok(journeySession);
    }
    [Authorize]
    [HttpPost("UpdateJourneySession")]
    public async Task<IActionResult> UpdateJourneySession(UpdateJourneySessionDto updateJourneySessionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var journeySession = await _journeySessionService.UpdateJourneySession(updateJourneySessionDto);
        return Ok(journeySession);
    }
    [Authorize]
    [HttpGet("CancelJourneySession")]
    public async Task<IActionResult> CancelJourneySession(int journeyId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var journeySession = await _journeySessionService.CancelJourneySession(journeyId);
        return Ok(journeySession);
    }
    [Authorize]
    [HttpGet("GetAllJourneySessions")]
    public async Task<IActionResult> GetAllJourneySessions()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var journeySession = await _journeySessionService.GetAllJourneySessions();
        return Ok(journeySession);
    }
    [Authorize]
    [HttpGet("GetAllJourneySessionsByDate")]
    public async Task<IActionResult> GetAllJourneySessionsByDate(DateTime? date)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var journeySession = await _journeySessionService.GetAllJourneySessions();
        return Ok(journeySession);
    }
}
