



using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace INZ_API_POSTGRES.Schemas
{
    public class Users : IdentityUser
    {
        public string ?ApiKey { get; set; }

    }
}
