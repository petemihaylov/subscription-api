using FluentValidation;
using MediatR;
using SubscriptionApi.Common;
using SubscriptionApi.Common.Exceptions;
using SubscriptionApi.Infrastructure.Data;

namespace SubscriptionApi.Features.Subscriptions.Commands;

public record UnsubscribeCommand(
    string CustomerPhoneNumber,
    int ServiceId) : IRequest<Result<Unit>>;

public class UnsubscribeValidator : AbstractValidator<UnsubscribeCommand>
{
    public UnsubscribeValidator()
    {
        RuleFor(x => x.CustomerPhoneNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in E.164 format");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0);
    }
}

public class UnsubscribeHandler : IRequestHandler<UnsubscribeCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnsubscribeHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(
        UnsubscribeCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services.GetByIdAsync(request.ServiceId)
            ?? throw new ServiceNotFoundException(request.ServiceId);

        var subscription = await _unitOfWork.Subscriptions
            .FindAsync(s => 
                s.CustomerPhoneNumber == request.CustomerPhoneNumber && 
                s.ServiceId == request.ServiceId)
            ?? throw new SubscriptionNotFoundException(request.CustomerPhoneNumber, service.Name);

        _unitOfWork.Subscriptions.Remove(subscription);
        await _unitOfWork.SaveChangesAsync();

        return Result<Unit>.Success(Unit.Value);
    }
}