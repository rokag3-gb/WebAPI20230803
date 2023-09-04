using WebAPI;

namespace WebAPI20230803
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<DapperContext>();

            builder.WebHost.UseKestrel();
            builder.WebHost.UseUrls("http://*:8080");
            builder.WebHost.ConfigureKestrel(opt => {
                opt.ListenAnyIP(8080);
            });

            var app = builder.Build();

            // Minimal API test
            app.MapGet("/HelloWorld", (string? name) =>
                $"Hello World! {name ?? "담당자"}님. Minimal API는 이렇게 간단히 쓸 수 있어서 좋군요^^"
            );

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}