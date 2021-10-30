using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using UserService.Models.Users;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Services.Users;

namespace UserService.Controllers
{
    /// <summary>
    /// Kontroler za dobavljanje, kreiranje, updajtovanje i brisanje korporativnih naloga
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/corporationUsers")]
    public class CorporationUserController : ControllerBase
    {
        private readonly ICorporationUsersService _corporationUsersService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public CorporationUserController(IMapper mapper, 
            LinkGenerator linkGenerator, ICorporationUsersService corporationUsersService)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _corporationUsersService = corporationUsersService;
        }

        /// <summary>
        /// Vraca listu svih korporativnih naloga u sistemu
        /// </summary>
        /// <param name="country">Name of the country</param>
        /// <param name="username">User username</param>
        /// <returns>List of corporation user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<CorporationUserDto>> GetUsers(string country, string username)
        {
            try {
                var corporationUsers = _corporationUsersService.GetUsers(country, username);
                if (corporationUsers == null || corporationUsers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<CorporationUserDto>>(corporationUsers));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

        /// <summary>
        /// Vraca korporativni nalog sa odredjenim ID-om
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Corporation user with userId</returns>
        ///<response code="200">Returns the user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{userId}")]
        public ActionResult<CorporationUserDto> GetUserById(Guid userId)
        {
            try
            {
                var corporationUser = _corporationUsersService.GetUserByUserId(userId);
                if (corporationUser == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<CorporationUserDto>(corporationUser));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kreira novi korporativni nalog
        /// </summary>
        /// <param name="corporationUser">Model of corporation user</param>
        /// <returns>Confirmation of the creating of corporation user</returns>
        /// <response code="200">Returns the created corporation user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CorporationUserCreatingConfirmationDto> CreateUser([FromBody] CorporationUserCreatingDto corporationUser)
        {
            try
            {
                
                CorporationUser userEntity = _mapper.Map<CorporationUser>(corporationUser);
                CorporationUserCreatingConfirmation userCreated = _corporationUsersService.CreateUser(userEntity, corporationUser.Password);
               

                string location = _linkGenerator.GetPathByAction("GetUserById", "CorporationUser", new { userId = userCreated.UserId });
                return Created(location, _mapper.Map<CorporationUserCreatingConfirmationDto>(userCreated));
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintViolationException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                if (ex.GetType().IsAssignableFrom(typeof(UniqueValueViolationException)))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);

                }
                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                    switch (ErrorCode)
                    {
                        case 2627: 
                            break;
                        case 547:
                            return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                        case 2601: 
                            return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update-ovanje korporativnog naloga
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Confirmation for updating Corporation user</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="404">Corporation user with userId is not found</response>
        ///<response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">Error on the server while updating</response>

        [HttpPut("{userId}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CorporationUserDto> UpdateUser([FromBody] CorporationUserUpdateDto corporationUser, Guid userId)
        {
            try
            {
                var userWithId = _corporationUsersService.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                CorporationUser CorporationUser = _mapper.Map<CorporationUser>(corporationUser);
                _corporationUsersService.UpdateUser(CorporationUser, userWithId);

                return Ok(_mapper.Map<CorporationUserDto>(userWithId));
            }
            catch (Exception ex)
            {
                if (ex.GetType().IsAssignableFrom(typeof(ForeignKeyConstraintViolationException)))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                if (ex.GetType().IsAssignableFrom(typeof(UniqueValueViolationException)))
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
                }
                if (ex.GetBaseException().GetType() == typeof(SqlException))
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;
                    switch (ErrorCode)
                    {
                        case 2627: 
                            break;
                        case 547: 
                            return StatusCode(StatusCodes.Status422UnprocessableEntity);
                        case 2601: 
                            return StatusCode(StatusCodes.Status409Conflict);
                        default:
                            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    }
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Brisanje korporacijskog naloga sa odredjenim ID-om
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">User succesfully deleted</response>
        ///<response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with this is userId not found</response>
        /// <response code="500">Error on the server while updating</response>

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                var corporationUser = _corporationUsersService.GetUserByUserId(userId);
                if (corporationUser == null)
                {
                    return NotFound();
                }
                _corporationUsersService.DeleteUser(userId);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }
    }
}
