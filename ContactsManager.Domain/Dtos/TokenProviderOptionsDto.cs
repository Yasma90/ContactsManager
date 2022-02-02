using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Domain.Dtos
{
    public class TokenProviderOptionsDto
    {
        public string JwtKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
        public string ApiSecret { get; set; }
    }
}
