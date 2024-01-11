using ServerPesentation.Extensions;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.AddServerDbContext();
            builder.ConfigureJWT();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCustomizedSwagger();
            builder.Services.AddJwtService();
            builder.Services.AddHashService();
            builder.Services.AddUnitOfWork();
            builder.Services.AddValidationService();
            builder.Services.AddCustomCORS();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}