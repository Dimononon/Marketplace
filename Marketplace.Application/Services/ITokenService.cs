using Marketplace.Domain.Entities;

namespace Marketplace.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}