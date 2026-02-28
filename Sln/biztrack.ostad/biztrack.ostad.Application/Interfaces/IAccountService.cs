using biztrack.ostad.Application.Common;
using biztrack.ostad.Contract.Request;
using biztrack.ostad.Domain;

namespace biztrack.ostad.Application.Interfaces;

public interface IAccountService
{
    Task<Result<ApplicationUser>> RegisterAsync(RegisterModel model, CancellationToken cancellationToken = default);
}
