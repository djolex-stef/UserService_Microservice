using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using UserService.Data;
using UserService.Models.Country;
using UserService.Entities;
using UserService.Exceptions;
using UserService.Services.Countries;

namespace UserService.Controllers
{
    /// <summary>
    /// Kontroler za pribavljanje, kreiranje, update-ovanje i brisanje drzava
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api/countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly ICountryService _countriesService;

        public CountryController(ICountryRepository countryRepository, IMapper mapper, LinkGenerator linkGenerator, ICountryService countriesService)
        {
            this.countryRepository = countryRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            _countriesService = countriesService;
        }

        /// <summary>
        /// Vraca listu svih drzava
        /// </summary>
        /// <param name="countryName">Name of the country</param>
        /// <returns>List of cities</returns>
        /// <response code="200">Returns the list</response>
        /// <response code="204">List is empty</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<CountryDto>> GetCountries(string countryName)
        {
            try
            {
                var countries = _countriesService.GetCountries(countryName);
                if (countries == null || countries.Count == 0)
                {
                    return NoContent();
                }
                return Ok(mapper.Map<List<CountryDto>>(countries));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Vraca zemlju sa odredjenim ID-om
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <returns>Country with countryId</returns>
        ///<response code="200">Returns the country</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Country with countryId is not found</response>
        /// <response code="500">Error on the server while fetching cities</response>
        [AllowAnonymous]
        [HttpGet("{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CountryDto> GetCountryById(Guid countryId)
        {
            try
            {
                var country = _countriesService.GetCountryByCountryId(countryId);
                if (country == null)
                {
                    return NotFound();
                }
                return Ok(mapper.Map<CountryDto>(country));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Kreira novu zemlju
        /// </summary>
        /// <param name="country">Model of country</param>
        /// <returns>Confirmation of the creating a country</returns>
        /// <response code="200">Returns the created country</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">There was an error on the server</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public ActionResult<CountryCreatingConfirmationDto> CreateCountry([FromBody] CountryCreatingDto country)
        {
            try
            {
                Country newCountry = mapper.Map<Country>(country);
                CountryCreatingConfirmation createdCountry = _countriesService.CreateCountry(newCountry);
                var location = linkGenerator.GetPathByAction("GetCountrById", "Country", new { countryId = createdCountry.CountryId });
                return Created(location, mapper.Map<CountryCreatingConfirmationDto>(createdCountry));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Update-ovanje drzave
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <returns>Confirmation of update</returns>
        /// <response code="200">Returns updated country</response>
        /// <response code="400">Country with countryId is not found</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="500">Error on the server while updating</response>
        [HttpPut("{countryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<CountryDto> UpdateCountry([FromBody] CountryUpdateDto countryUpdate, Guid countryId)
        {
            try
            {
                Country countryWithId = _countriesService.GetCountryByCountryId(countryId);
                if (countryWithId == null)
                {
                    return NotFound();
                }
                Country updatedCountry = mapper.Map<Country>(countryUpdate);
                updatedCountry.CountryId= countryId;
                mapper.Map(updatedCountry, countryWithId);
                countryRepository.SaveChanges();
                return Ok(mapper.Map<CountryDto>(countryWithId));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");

            }
        }

        /// <summary>
        /// Brisanje zemlje sa odredjenim ID-om
        /// </summary>
        /// <param name="countryId">Country Id</param>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Country is deleted</response>
        /// <response code="401">Unauthorized user</response>
        /// <response code="404">Country with specified countryId is not found</response>
        /// <response code="409">Country is referenced in another table</response>
        /// <response code="500">Error on the server while deleting</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpDelete("{countryId}")]
        public IActionResult DeleteCountry(Guid countryId)
        {
            try
            {
                var country = _countriesService.GetCountryByCountryId(countryId);
                if (country == null)
                {
                    return NotFound();
                }
                _countriesService.DeleteCountry(countryId);
                return NoContent();
            }
            catch(Exception ex)
            {
                if (ex.GetBaseException().GetType() == typeof(ReferentialConstraintViolationException))
                {
                    return StatusCode(StatusCodes.Status409Conflict, ex.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }

    }
}
