using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
[Route("LocationTracker/[controller]")]
[ApiController]
public class CustomMarkerController : Controller
{
    ICustomMarkerService _customMarkerService;
    IJWTAuthentication _jwtAuthentication;
    public CustomMarkerController(ICustomMarkerService CustomMarkerService, IJWTAuthentication jwtAuthentication)
    {
        _customMarkerService = CustomMarkerService;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateCustomMarker")]
    public async Task<IActionResult> CreateCustomMarker([FromBody] CreateCustomMarkerDto createCustomMarkerDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        createCustomMarkerDto.UserId = user.Id;
        var customMarker = await _customMarkerService.CreateCustomMarker(createCustomMarkerDto);
        return Ok(customMarker);
    }
    [Authorize]
    [HttpPost("CreateBatchCustomMarkers")]
    public async Task<IActionResult> CreateBatchCustomMarkers(UpdateCustomMarkerDto updateCustomMarkerDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        updateCustomMarkerDto.UserId = user.Id;
        var customMarker = await _customMarkerService.UpdateCustomMarker(updateCustomMarkerDto);
        return Ok(customMarker);
    }
    [Authorize]
    [HttpGet("GetAllCustomMarkers")]
    public async Task<IActionResult> GetAllCustomMarkers()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var customMarker = await _customMarkerService.GetAllCustomMarkers();
        return Ok(customMarker);
    }
    [Authorize]
    [HttpDelete("DeleteCustomMarker")]
    public async Task<IActionResult> DeleteCustomMarker(int customMarkerId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var customMarker = await _customMarkerService.DeleteCustomMarker(customMarkerId);
        return Ok(customMarker);
    }
}
