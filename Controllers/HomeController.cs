using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Models;
using Shop.Services;
using Shop.Repositories;

namespace Shop.Controllers
{
    [Route("v1/account")]
    public class HomeController: ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);

            if(user == null)
                return NotFound(new { message = "Usuário ou senha invalidos" });

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("login/all")]
        [Authorize]
        public async Task<ActionResult<dynamic>> All(){
            var user = UserRepository.all();
            if(user == null)
                return NotFound(new { message = "Sem usuários cadastrados" });
            
            return user;
        }
        
        [HttpGet]
        [Route("login/user")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<dynamic>> Usuario(){
            var user = String.Format("Autenticado - {0}", User.Identity.Name);
           
            return user;
        }
    
    }
}