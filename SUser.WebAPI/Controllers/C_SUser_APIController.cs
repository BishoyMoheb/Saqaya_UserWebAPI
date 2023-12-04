using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SUser.CLDataAccess.EFContext;
using SUser.CLDataAccess.RepositoryPattern;
using SUser.CLDataAccess.ViewModels;
using SUser.CLDomain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class C_SUser_APIController : ControllerBase
    {
        private readonly IUserRespository _userRepositoryI;
        private readonly UserManager<IdUserExtension> _userM_ID;
        private readonly SignInManager<IdUserExtension> _signInM_ID;
        private readonly IConfiguration _configI;

        // Injecting the built-in UserManager, SignInManager service
        public C_SUser_APIController(IUserRespository userRepositoryI,
                                     UserManager<IdUserExtension> userM_ID,
                                     SignInManager<IdUserExtension> signInM_ID,
                                     IConfiguration configI)
        {
            this._userRepositoryI = userRepositoryI;
            this._userM_ID = userM_ID;
            this._signInM_ID = signInM_ID;
            this._configI = configI;
        }


        // GET : api/c_suser_api/
        [HttpGet]
        public ActionResult GetActionResult()
        {
            return Ok("The Saqaya API Core Project is Loaded");
        }


        // POST : api/c_suser_api/
        [HttpPost]
        public async Task<IActionResult> Do_AddUserAsync([FromBody] VM_MUser vmMUser)
        {
            var NewUser = new IdUserExtension
            {
                UserName = vmMUser.FirstName + vmMUser.LastName,
                Email = vmMUser.Email,
                IsMarketingConsent = vmMUser.IsMarketingConsent
            };
            var IsCreated = await _userM_ID.CreateAsync(NewUser, "NEWpa$$2023");
            MUser UserAdded = new MUser();
            string SAccessToken = string.Empty;
            if (IsCreated.Succeeded)
            {
                string NewId = await _signInM_ID.UserManager.GetUserIdAsync(NewUser);
                MUser mUser = new MUser
                {
                    UId = NewId,
                    FirstName = vmMUser.FirstName,
                    LastName = vmMUser.LastName,
                    Email = vmMUser.Email,
                    IsMarketingConsent = vmMUser.IsMarketingConsent
                };
                UserAdded = await _userRepositoryI.AddUserAsync(mUser);
                var SSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configI["Jwt:Key"]));
                var SignCredentials = new SigningCredentials(SSecurityKey, SecurityAlgorithms.HmacSha256);
                var JSToken = new JwtSecurityToken(_configI["Jwt:Issuer"],
                                                   _configI["Jwt:Issuer"],
                                                   null,
                                                   expires: DateTime.Now.AddMinutes(120),
                                                   signingCredentials: SignCredentials);
                SAccessToken = new JwtSecurityTokenHandler().WriteToken(JSToken);
            }
            return Ok(new { Id = UserAdded.UId, AccessToken = SAccessToken });
        }


        // GET : api/c_suser_api/Do_GetUserAsync/id={Id}&accessToken={AccessToken}
        [HttpGet]
        [Route("Do_GetUserAsync/id={Id}&accessToken={AccessToken}")]
        public async Task<IActionResult> Do_GetUserAsync(string Id, string AccessToken)
        {
            MUser UserToGet = await _userRepositoryI.GetUserAsync(Id);
            if (UserToGet == null)
                return NotFound(Id);
            if (UserToGet.IsMarketingConsent)
                return Ok(UserToGet);
            VM_MUserWithoutMarket vmMUserWithoutMarket = new VM_MUserWithoutMarket();
            vmMUserWithoutMarket.UId = UserToGet.UId;
            vmMUserWithoutMarket.FirstName = UserToGet.FirstName;
            vmMUserWithoutMarket.LastName = UserToGet.LastName;
            vmMUserWithoutMarket.Email = UserToGet.Email;
            return Ok(vmMUserWithoutMarket);
        }
    }
}
