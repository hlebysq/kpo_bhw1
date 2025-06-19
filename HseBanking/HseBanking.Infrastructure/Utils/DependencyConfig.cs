using HseBanking.HseBanking.Application.Factories;
using HseBanking.HseBanking.Application.ImportExport;
using HseBanking.HseBanking.Application.Services;
using HseBanking.HseBanking.Domain.Models;
using HseBanking.HseBanking.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HseBanking.HseBanking.Infrastructure.Utils;

public static class DependencyConfig
{
    public static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
            
        // Factories
        services.AddSingleton<FinancialFactory>();
            
        // Repositories with proxy caching
        services.AddSingleton<IRepository<BankAccount>>(provider => 
            new CachingRepositoryProxy<BankAccount>(
                new InMemoryAccountRepository()));
            
        services.AddSingleton<IRepository<Operation>>(provider => 
            new CachingRepositoryProxy<Operation>(
                new InMemoryOperationRepository()));
            
        // Services
        services.AddSingleton<IFinancialFacade, FinancialFacade>();
            
        // Import/Export
        services.AddSingleton<IDataExporter, JsonExporter>();
            
        return services.BuildServiceProvider();
    }
}