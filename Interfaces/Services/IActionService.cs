using System.Text.Json;
using LocationTracker.Entities;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Interfaces.Services;
public interface IActionService
{
    public Task<BaseResponse> CreateAction(CreateActionDto createActionDto, int personId);
    public Task<BaseResponse> UpdateAction(UpdateActionDto updateActionDto, int personId);
    public Task<ActionResponseModel> GetById(int actionId);
    public Task<ActionsResponseModel> GetAllActions();
    public Task<JsonElement> GetActionNames(string token);
    public Task<BaseResponse> ActivateAction(int actionId, int personId, string token);
    public Task<BaseResponse> DeleteAction(int actionId, int personId);
}