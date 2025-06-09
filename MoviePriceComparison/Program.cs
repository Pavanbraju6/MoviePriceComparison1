using MoviePriceComparison.Interface;
using MoviePriceComparison.Middleware;
using MoviePriceComparison.Services;

var builder = WebApplication.CreateBuilder(args);
// Bind config
//builder.Services.Configure<MovieApiOptions>(
//    builder.Configuration.GetSection("MovieApi"));

//// Register HttpClient and service
//builder.Services.AddHttpClient<ICinemaworldService, CinemaworldService>();
// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddScoped<ICinemaWorldService, CinemaWorldService>();
builder.Services.AddScoped<IFilmWorldService, FilmWorldService>();
builder.Services.AddScoped<IMovieComparisonService, MovieComparisonService>();


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Use the middleware **before** other middleware
//app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);


app.UseAuthorization();

app.MapControllers();

app.Run();
