using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    // �������ơ�Blog.Core�����Զ��壬����һ�¼���
    c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
    {
        Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�\"",
        Name = "Authorization", // jwtĬ�ϵĲ�������
        In = "header", // jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
        Type = "apiKey"
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7150";
        //options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
