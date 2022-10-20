namespace RealtySale.Api.Data.IRepositories;

public interface IUnitOfWork
{
    ICityRepository CityRepository { get; }
    IUserRepository UserRepository { get; }
    Task<bool> SaveAsync();
}