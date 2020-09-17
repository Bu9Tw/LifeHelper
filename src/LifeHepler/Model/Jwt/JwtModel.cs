using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Jwt
{
    public class JwtModel
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public string Pwd { get; set; }
    }
}
