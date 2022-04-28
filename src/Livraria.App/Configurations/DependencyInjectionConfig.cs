using Livraria.App.Extensions;
using Livraria.Business.Interfaces;
using Livraria.Business.Notifications;
using Livraria.Business.Services;
using Livraria.Data.Context;
using Livraria.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace Livraria.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<LivrariaDbContext>();
            services.AddScoped<ILivroRepository, LivroRepository>();         
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IAutorRepository, AutorRepository>();

            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IAutorService, AutorService>();

            return services;
        }
    }
}
