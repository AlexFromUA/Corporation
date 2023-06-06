namespace Corporation.Services;

public class BaseService 
{
    private readonly IConfiguration _configuration;

    public readonly string connectionString ;

    public BaseService(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("TestDB");
    }
}