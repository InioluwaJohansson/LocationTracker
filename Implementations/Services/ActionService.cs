using System.Text;
using System.Text.Json;
using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;
using Newtonsoft.Json;

namespace LocationTracker.Implementations.Services;

public class ActionService : IActionService
{
    IActionRepo _actionRepo;
    IRealtimeNotificationService _realtime;
    public IConfiguration _baseUrl;
    public ActionService(IActionRepo actionRepo, IRealtimeNotificationService realtime, IConfiguration config)
    {
        _actionRepo = actionRepo;
        _realtime = realtime;
        _baseUrl = config.GetSection("BaseApp");
    }
    public async Task<BaseResponse> CreateAction(CreateActionDto createActionDto, int personId)
    {
        var getAction = await _actionRepo.Get(x => x.ActionName == createActionDto.ActionName && x.IsDeleted == false);
        if (getAction == null)
        {
            var action = new Actions()
            {
                ActionName = createActionDto.ActionName,
                DistanceToMarker = createActionDto.DistanceToMarker,
                CustomMarkerId = createActionDto.CustomMarkerId,
                IsActive = false,
                ActionIds = createActionDto.ActionIds,
                IsDeleted = false,
                CreatedBy = personId,
                LastModifiedBy = personId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };
            action = await _actionRepo.Create(action);
            await _realtime.NotifyAll("ActionCreated", GetDetails(action));
            return new BaseResponse()
            {
                Success = true,
                Message = "Action Added"
            };
        }
        return new BaseResponse()
        {
            Success = false,
            Message = "Failed to Add Action"
        };
    }
    public async Task<BaseResponse> UpdateAction(UpdateActionDto updateActionDto, int personId)
    {
        var action = await _actionRepo.Get(x => x.Id == updateActionDto.Id && x.IsDeleted == false);
        if (action != null)
        {
            action.ActionName = updateActionDto.ActionName ?? action.ActionName;
            action.DistanceToMarker = updateActionDto.DistanceToMarker;
            action.CustomMarkerId = updateActionDto.CustomMarkerId;
            action.ActionIds = updateActionDto.ActionIds;
            action.IsDeleted = false;
            action.LastModifiedBy = personId;
            action.LastModifiedOn = DateTime.UtcNow;
            action = await _actionRepo.Update(action);
            await _realtime.NotifyAll("ActionUpdated", GetDetails(action));
            return new BaseResponse()
            {
                Success = true,
                Message = "Action Updated"
            };
        }
        return new BaseResponse()
        {
            Success = false,
            Message = "Failed to Update Action"
        };
    }
    public async Task<BaseResponse> ActivateAction(int actionId, int personId, string token)
    {
        var action = await _actionRepo.Get(x => x.Id == actionId && x.IsDeleted == false);
        if (action != null)
        {
            action.LastModifiedBy = personId;
            action.LastModifiedOn = DateTime.UtcNow;
            if (action.IsActive == true) action.IsActive = false;
            else if (action.IsActive == false)
            {
                action.IsActive = true;
                await StartActions(token, action.ActionIds);
            }
            action = await _actionRepo.Update(action);
            await _realtime.NotifyAll("ActionActivated", new
            {
                Id = action.Id,
                Status = action.IsActive
            });
            return new BaseResponse()
            {
                Success = true,
                Message = "Action Updated"
            };
        }
        return new BaseResponse()
        {
            Success = false,
            Message = "Failed to Update Action"
        };
    }
    public async Task<ActionResponseModel> GetById(int actionId)
    {
        var action = await _actionRepo.Get(x => x.Id == actionId && x.IsDeleted == false);
        if (action != null)
        {
            return new ActionResponseModel()
            {
                Data = GetDetails(action),
                Success = true,
            };
        }
        return new ActionResponseModel()
        {
            Data = null,
            Success = false,
        };
    }
    public async Task<ActionsResponseModel> GetAllActions()
    {
        var actions = await _actionRepo.GetByExpression(x => x.IsDeleted == false);
        if (actions != null)
        {
            return new ActionsResponseModel()
            {
                Data = actions.Select(GetDetails).ToList(),
                Success = true,
            };
        }
        return new ActionsResponseModel()
        {
            Data = null,
            Success = false,
        };
    }
    public async Task<JsonElement> GetActionNames(string token)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);
        var response = await client.PostAsync($"{_baseUrl["BaseUrl"]}{_baseUrl["GetAppNamesIdList"]}", null);
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return new JsonElement();
        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        return document.RootElement.GetProperty("actionIdNames");
    }
    public async Task<BaseResponse> DeleteAction(int actionId, int personId)
    {
        var action = await _actionRepo.Get(x => x.Id == actionId && x.IsDeleted == false);
        if (action != null)
        {
            action.IsActive = false;
            action.IsDeleted = true;
            action.LastModifiedBy = personId;
            action.LastModifiedOn = DateTime.UtcNow;
            action = await _actionRepo.Update(action);
            await _realtime.NotifyAll("ActionDeleted",
            new
            {
                Id = action.Id
            });
            return new BaseResponse()
            {
                Success = true,
                Message = "Action Deleted"
            };
        }
        return new BaseResponse()
        {
            Success = false,
            Message = "Failed to Delete Action"
        };
    }
    private GetActionDto GetDetails(Actions action)
    {
        return new GetActionDto()
        {
            Id = action.Id,
            ActionName = action.ActionName,
            ActionIds = action.ActionIds,
            IsActive = action.IsActive,
            DistanceToMarker = action.DistanceToMarker,
            CustomMarkerId = action.CustomMarkerId
        };
    }
    private async Task StartActions(string token, List<int> ActionIds)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var url = $"{_baseUrl["BaseUrl"]}{_baseUrl["StartActions"]}";
        var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(ActionIds), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        Console.WriteLine($"Response from StartActions: {response.StatusCode}");
    }
}