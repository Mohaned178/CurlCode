using CurlCode.Domain.Entities.Identity;

namespace CurlCode.Application.Contracts.Infrastructure;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}






