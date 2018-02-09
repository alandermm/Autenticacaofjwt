using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Autenticacaofjwt.Models;
using Autenticacaofjwt.Repositorio;
using Microsoft.AspNetCore.Mvc;
using modelobasicoefjwt.Models;
using modelobasicoefjwt.Repositorio;

namespace Autenticacaofjwt.Controllers
{
    [Route("api/[controller]")]
    public class LoginController:Controller
    {
        readonly AutenticacaoContext _contexto;
        public LoginController(AutenticacaoContext contexto){
            _contexto = contexto;
        }

        [HttpPost]
        public IActionResult Validar([FromBody] Usuario usuario,
                                    [FromServices] SigningConfigurations singningConfigurations,
                                    [FromServices] TokenConfigurations tokenConfigurations){
            Usuario user = _contexto.Usuarios.FirstOrDefault(c => c.Email == usuario.Email && c.Senha == usuario.Senha);

            if(user != null){
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.IdUsuario.ToString(), "Login"),
                    new [] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.IdUsuario.ToString()),
                        new Claim("Nome", user.Nome),
                        new Claim(ClaimTypes.Email, user.Email)
                    }
                );
            }
        }
    }
}