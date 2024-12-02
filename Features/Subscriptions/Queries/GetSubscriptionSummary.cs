using FluentValidation;
using MediatR;
using SubscriptionApi.Common;
using SubscriptionApi.Infrastructure.Data;
using SubscriptionApi.Models;
using SubscriptionApi.Services;

namespace SubscriptionApi.Features.Subscriptions.Queries;

public record GetSubscriptionSummaryQuery(string CustomerPhoneNumber) 
    : IRequest<Result<SubscriptionSummary>>;

public class GetSubscriptionSummaryValidator : AbstractValidator<GetSubscriptionSummaryQuery>
{
    public GetSubscriptionSummaryValidator()
    {
        RuleFor(x => x.CustomerPhoneNumber)
            .NotEmpty()
            .Matches(@"^\+[1-9]\d{1,14}$")
            .WithMessage("Phone number must be in E.164 format");
    }
}

public class GetSubscriptionSummaryHandler 
    : IRequestHandler<GetSubscriptionSummaryQuery, Result<SubscriptionSummary>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDiscountService _discountService;

    public GetSubscriptionSummaryHandler(
        IUnitOfWork unitOfWork,
        IDiscountService discountService)
    {
        _unitOfWork = unitOfWork;
        _discountService = discountService;
    }

    public async Task<Result<SubscriptionSummary>> Handle(
        GetSubscriptionSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var subscriptions = await _unitOfWork.Subscriptions
            .FindAllAsync(s => s.CustomerPhoneNumber == request.CustomerPhoneNumber);

        var summary = new SubscriptionSummary
        {
            CustomerPhoneNumber = request.CustomerPhoneNumber,
            Subscriptions = subscriptions.Select(s => new SubscriptionDetail
            {
                ServiceName = s.Service!.Name,
                DurationMonths = s.DurationMonths,
                MonthlyPrice = s.Service.MonthlyPrice
            }).ToList()
        };

        var discounts = _discountService.CalculateDiscounts(subscriptions.ToList());
        
        summary.TotalCostBeforeDiscounts = subscriptions
            .Sum(s => s.Service!.MonthlyPrice * s.DurationMonths);
        summary.AppliedDiscounts = discounts;
        summary.TotalDiscounts = discounts.Sum(d => d.Amount);
        summary.FinalCost = summary.TotalCostBeforeDiscounts - summary.TotalDiscounts;

        return Result<SubscriptionSummary>.Success(summary);
    }
}