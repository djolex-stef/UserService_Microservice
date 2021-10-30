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
    /// Kontroler za pribavljanje, kreiranje, update-ovanje i brisanje personalnih naloga
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/personalUsers")]
    public class PersonalUserController : ControllerBase
    {
        private readonly IPersonalUsersService _personalUsersService;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public PersonalUserController(IMapper mapper, 
            LinkGenerator linkGenerator, IPersonalUsersService personalUsersService)
        {
            _mapper = mapper;
            _linkGenerator = linkGenerator;
            _personalUsersService = personalUsersService;
        }

        /// <summary>
        /// Vraca listu svih personalnih naloga
        /// </summary>
        /// <param name="country">Name of the country</param>
        /// <param name="username">User username</param>
        /// <returns>List of personal user accounts</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">No user accounts are found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<PersonalUserDto>> GetUsers(string country, string username)
        {
            try{
                var personalUsers = _personalUsersService.GetUsers(country, username);
                if (personalUsers == null || personalUsers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(_mapper.Map<List<PersonalUserDto>>(personalUsers));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Vraca personalni nalog sa odredjenim ID-om
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Personal user with userId</returns>
        ///<response code="200">Returns personal user</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">User with userId is not found</response>
        /// <response code="500">There was an error on the server</response>
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}")]
        public ActionResult<PersonalUserDto> GetUserById(Guid userId)
        {
            try
            {
                var personalUser = _personalUsersService.GetUserByUserId(userId);
                if (personalUser == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<PersonalUserDto>(personalUser));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
           
        }


        /// <summary>
        /// Kreiramo novi personalni nalog
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatingConfirmationDto> CreateUser([FromBody] PersonalUserCreatingDto personalUser)
        {
            try
            {
                PersonalUser persUserEntity = _mapper.Map<PersonalUser>(personalUser);

                PersonalUserCreatingConfirmation persUserCreated = _personalUsersService.CreateUser(persUserEntity, personalUser.Password);

                string location = _linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = persUserCreated.UserId });

                return Created(location, _mapper.Map<PersonalUserCreatingConfirmationDto>(persUserCreated));
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
        /// Update-ovanje personalnog naloga
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated user</response>
        /// <response code="404">Personal user with userId is not found</response>
        /// <response code="500">Error on the server while updating</response>
        /// <response code="400">User doesn't own the resource</response>
        /// <response code="401">Unauthorized user</response>
        ///<response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserDto> UpdateUser([FromBody] PersonalUserUpdateDto personalUser, Guid userId)
        {
            try
            {
                var userWithId = _personalUsersService.GetUserByUserId(userId);
                if (userWithId == null)
                {
                    return NotFound();
                }
                PersonalUser personaluser = _mapper.Map<PersonalUser>(personalUser);
                _personalUsersService.UpdateUser(personaluser, userWithId);

                return Ok(_mapper.Map<PersonalUserDto>(userWithId));
            }
            catch(Exception ex)
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
        /// Brisanje personalnog naloga sa odredjenim ID-om
        /// </summary>
        /// <param name="userId">User's Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">User succesfully deleted</response>
        /// <response code="400">User doesn't own the resource</response>
        /// <response code="404">User with userId not found</response>
        /// <response code="500">Error on the server while deleting</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                var personalUser = _personalUsersService.GetUserByUserId(userId);
                if (personalUser == null)
                {
                    return NotFound();
                }
                _personalUsersService.DeleteUser(userId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Kreiranje novog personalnog naloga, Uloga-Admin
        /// </summary>
        /// <param name="personalUser">Model of personal user</param>
        /// <returns>Confirmation of the creation of personal user</returns>
        /// <response code="200">Returns the created personal user</response>
        /// <response code="401">Unauthorize user</response>
        /// <response code="409">Unique value violation</response>
        /// <response code="422">Constraint violation</response>
        /// <response code="500">There was an error on the server</response>
       // [Authorize(Roles="Admin")]
        [HttpPost("admins")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonalUserCreatingConfirmationDto> CreateAdmin([FromBody] PersonalUserCreatingDto personalUser)
        {
            try
            {
                PersonalUser personalUserEntity = _mapper.Map<PersonalUser>(personalUser);

                PersonalUserCreatingConfirmation personalUserCreated = _personalUsersService.CreateAdmin(personalUserEntity, personalUser.Username);

                string location = _linkGenerator.GetPathByAction("GetUserById", "PersonalUser", new { userId = personalUserCreated.UserId });
                return Created(location, _mapper.Map<PersonalUserCreatingConfirmationDto>(personalUserCreated));
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
    }
}
