namespace PetAdmin.Web.Extensions
{
    public class JwtSettings
    {
        // Chave de Criptografia do token
        // Pode ter o tamanho q quiser, qto mais complexo melhor
        public string Key { get; set; }

        public int MinutesToExpiration { get; set; }

        // A minha aplicação
        public string Issuer { get; set; }

        // Em quais urls esse token é válido
        public string Audience { get; set; }
    }
}
