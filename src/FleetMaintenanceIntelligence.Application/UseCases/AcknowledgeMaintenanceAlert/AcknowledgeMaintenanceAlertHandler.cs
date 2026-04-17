using FleetMaintenanceIntelligence.Application.Abstractions;
using FleetMaintenanceIntelligence.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetMaintenanceIntelligence.Application.UseCases.AcknowledgeMaintenanceAlert
{
    public sealed class AcknowledgeMaintenanceAlertHandler
    {
        private readonly IMaintenanceAlertRepository _alertRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AcknowledgeMaintenanceAlertHandler(
            IMaintenanceAlertRepository alertRepository,
            IUnitOfWork unitOfWork)
        {
            _alertRepository = alertRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(Guid alertId, CancellationToken cancellationToken = default)
        {
            var alert = await _alertRepository.GetByIdAsync(alertId, cancellationToken);
            if (alert is null)
                throw new DomainException("Alert not found.");

            alert.Acknowledge();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
