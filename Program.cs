using Microsoft.EntityFrameworkCore;
using Pembayaran_Siakad.Data;
using Pembayaran_Siakad.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SIAKAD - API Keuangan",
        Version = "v1",
        Description = "API Keuangan Sistem Informasi Akademik (SIAKAD). " +
                      "Melakukan sinkronisasi data mahasiswa dari API eksternal dan " +
                      "mengelola data keuangan (UKT, tagihan, dan riwayat pembayaran)."
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddHttpClient<SyncService>();

builder.Services.AddScoped<SyncService>();
builder.Services.AddScoped<KeuanganService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SIAKAD Keuangan API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();