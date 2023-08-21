public static class VaultInizializer
{
    public static WebApplication InitializeVault(this WebApplication webApp)
    {
        using (var scope = webApp.Services.CreateScope())
        {
            if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "vault")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "vault"));
            }

            if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "data")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "data"));
            }
        }
        return webApp;
    }
}