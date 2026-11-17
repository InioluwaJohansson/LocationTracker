using Home_Security.RealTimeServices;
using LocationTracker.Entities;
using LocationTracker.Interfaces.Repositories;
using LocationTracker.Interfaces.Services;
using LocationTracker.Models.DTOs;

namespace LocationTracker.Implementations.Services;
public class ActionService : IActionService
{
    IActionRepo _actionRepo;
    IRealtimeNotificationService _realtime;
    public ActionService(IActionRepo actionRepo, IRealtimeNotificationService realtime)
    {
        _actionRepo = actionRepo;
        _realtime = realtime;
    }

    public async Task<BaseResponse> ActivateDeactivateAction(int actionId)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse> CreateAction(CreateActionDto createActionDto, int personId)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse> DeleteAction(int actionId, int personId)
    {
        throw new NotImplementedException();
    }

    public async Task<ActionsResponseModel> GetAllActions()
    {
        throw new NotImplementedException();
    }

    public async Task<ActionResponseModel> GetById(int actionId)
    {
        throw new NotImplementedException();
    }

    public async Task RunAllReccuringActions(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse> UpdateAction(UpdateActionDto updateActionDto, int personId)
    {
        throw new NotImplementedException();
    }
}