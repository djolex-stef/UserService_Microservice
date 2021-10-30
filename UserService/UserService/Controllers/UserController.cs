using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using UserService.Data;
using UserService.Models.Users;
using UserService.Entities;
using UserService.Services;
using UserService.Services.Users;

namespace UserService.Controllers
{
    /// <summary>
    /// Kontroler za pribavljanje korisnickih naloga
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/users")]
   // [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IPersonalUsersService _personalUsersService;
        private readonly ICorporationUsersService _corporationUsersService;
        private readonly IMapper _mapper;

        public UserController(IPersonalUsersService personalUsersService, ICorporationUsersService corporationUsersService,
            IMapper mapper)
        {
            _corporationUsersService = corporationUsersService;
            _personalUsersService = personalUsersService;
            _mapper = mapper;
        }

        /// <summary>
        /// Vraca listu svih korisnicih naloga u sistemu
        /// </summary>
        /// <param name="country">Name of the country</param>
        /// <param name="userType">Name of the account type 
        /// (personalUser or corporationUser)</param>
        /// <returns>List of user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<UserInfoDto>> GetUsers(string userType, string country, string username)
        {
            try
            {
                if (string.IsNullOrEmpty(userType))
                {
                    List<CorporationUser> corporationUsers = _corporationUsersService.GetUsers(country, username);
                    List<PersonalUser> personalUsers = _personalUsersService.GetUsers(country, username);
                    List<UserInfoDto> users = new List<UserInfoDto>();
                    users.AddRange(_mapper.Map<List<UserInfoDto>>(personalUsers));
                    users.AddRange(_mapper.Map<List<UserInfoDto>>(corporationUsers));
                    if (users.Count == 0)
                    {
                        return NoContent();
                    }
                    return Ok(users);
                }
                else
                {
                    if (string.Equals(userType, "personalUser"))
                    {
                        List<PersonalUser> personalUsers = _personalUsersService.GetUsers(country, username);
                        if (personalUsers == null || personalUsers.Count == 0)
                        {
                            return NoContent();
                        }
                        return Ok(_mapper.Map<List<UserInfoDto>>(personalUsers));
                    }
                    else if (string.Equals(userType, "corporationUser"))
                    {
                        List<CorporationUser> corporationUsers = _corporationUsersService.GetUsers(country, username);
                        if (corporationUsers == null || corporationUsers.Count == 0)
                        {
                            return NoContent();
                        }
                        return Ok(_mapper.Map<List<UserInfoDto>>(corporationUsers));
                    }
                    else
                    {
                        return NoContent();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Vraca korisnike sa odredjenim ID-om
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns> User with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="500">Error on the server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId}")]
        public ActionResult<UserInfoDto> GetUserById(Guid userId)
        {
            try
            {
                var corporationUser = _corporationUsersService.GetUserByUserId(userId);
                if (corporationUser != null)
                {
                    return Ok(_mapper.Map<UserInfoDto>(corporationUser));
                }
                var personalUser = _personalUsersService.GetUserByUserId(userId);
                if (personalUser != null)
                {
                    return Ok(_mapper.Map<UserInfoDto>(personalUser));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        } 
        
    }
}
