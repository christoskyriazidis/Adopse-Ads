using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerAds.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        //[DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        //public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}
