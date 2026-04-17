using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.ResolveMaintenanceAlert
{
    public sealed class ResolveMaintenanceAlertHandler
    {
        private readonly IMaintenanceAlertRepository _alertRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public ResolveMaintenanceAlertHandler(
            IMaintenanceAlertRepository alertRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _alertRepository = alertRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task HandleAsync(Guid alertId, CancellationToken cancellationToken = default)
        {
            var alert = await _alertRepository.GetByIdAsync(alertId, cancellationToken);
            if (alert is null)
                throw new DomainException("Alert not found.");

            alert.Resolve(_clock.UtcNow);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
