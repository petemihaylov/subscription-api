using FluentValidation;
using MediatR;
using SubscriptionApi.Common;
using SubscriptionApi.Common.Exceptions;
using SubscriptionApi.Infrastructure.Data;
using SubscriptionApi.Infrastructure.Factories;
using SubscriptionApi.Models;

namespace SubscriptionApi.Features.Subscriptions.Commands;

public record CreateSubscriptionCommand(
    string CustomerPhoneNumber,
    int ServiceId,
    int DurationMonths) : IRequest<Result<Subscription>>;

public class CreateSubscriptionValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionValidator()
    {
        RuleFor(x => x.CustomerPhoneNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in E.164 format");

        RuleFor(x => x.ServiceId)
            .GreaterThan(0);

        RuleFor(x => x.DurationMonths)
            .GreaterThan(0)
            .LessThanOrEqualTo(12);
    }
}

public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionCommand, Result<Subscription>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISubscriptionFactory _subscriptionFactory;

    public CreateSubscriptionHandler(
        IUnitOfWork unitOfWork,
        ISubscriptionFactory subscriptionFactory)
    {
        _unitOfWork = unitOfWork;
        _subscriptionFactory = subscriptionFactory;
    }

    public async Task<Result<Subscription>> Handle(
        CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _unitOfWork.Services.GetByIdAsync(request.ServiceId)
            ?? throw new ServiceNotFoundException(request.ServiceId);

        var existingSubscription = await _unitOfWork.Subscriptions
            .FindAsync(s => 
                s.CustomerPhoneNumber == request.CustomerPhoneNumber && 
                s.ServiceId == request.ServiceId);

        if (existingSubscription != null)
            throw new DuplicateSubscriptionException(request.CustomerPhoneNumber, service.Name);

        var subscription = _subscriptionFactory.Create(
            request.CustomerPhoneNumber,
            service,
            request.DurationMonths);

        await _unitOfWork.Subscriptions.AddAsync(subscription);
        await _unitOfWork.SaveChangesAsync();

        return Result<Subscription>.Success(subscription);
    }
}