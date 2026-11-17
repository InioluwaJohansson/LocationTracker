using LocationTracker.Authentication;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
[Route("LocationTracker/[controller]")]
[ApiController]
public class ActionController : Controller
{
    IActionService _actionService;
    IJWTAuthentication _jwtAuthentication;
    public ActionController(IActionService ActionService, IJWTAuthentication jwtAuthentication)
    {
        _actionService = ActionService;
        _jwtAuthentication = jwtAuthentication;
    }
    [Authorize]
    [HttpPost("CreateAction")]
    public async Task<IActionResult> CreateAction(CreateActionDto createActionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var action = await _actionService.CreateAction(createActionDto, user.Id);
        return Ok(action);
    }
    [Authorize]
    [HttpPut("UpdateAction")]
    public async Task<IActionResult> UpdateAction(UpdateActionDto updateActionDto)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var action = await _actionService.UpdateAction(updateActionDto, user.Id);
        return Ok(action);
    }
    [Authorize]
    [HttpPost("ActivateAction")]
    public async Task<IActionResult> ActivateAction(int actionId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user == null)
            return Unauthorized();
        var action = await _actionService.ActivateAction(actionId, user.Id, token);
        return Ok(action);
    }
    [Authorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int actionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var action = await _actionService.GetById(actionId);
        return Ok(action);
    }
    [Authorize]
    [HttpGet("GetAllActions")]
    public async Task<IActionResult> GetAllActions()
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var action = await _actionService.GetAllActions();
        return Ok(action);
    }
    [Authorize]
    [HttpGet("GetAllActionNames")]
    public async Task<IActionResult> GetAllActionNames()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var user = _jwtAuthentication.GetUserFromToken(token);
        if (user == null)
            return Unauthorized();
        var action = await _actionService.GetActionNames(token);
        return Ok(Content(action.GetRawText(),"application/json"));
    }
    [Authorize]
    [HttpDelete("DeleteAction")]
    public async Task<IActionResult> DeleteAction(int actionId)
    {
        var user = _jwtAuthentication.GetUserFromToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
        if (user == null)
            return Unauthorized();
        var action = await _actionService.DeleteAction(actionId, user.Id);
        return Ok(action);
    }
}
