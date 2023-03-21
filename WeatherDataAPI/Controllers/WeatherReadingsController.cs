using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WeatherDataAPI.Attributes;
using WeatherDataAPI.Models;
using WeatherDataAPI.Models.AppUser;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models.WeatherReadings.DTO;
using WeatherDataAPI.Repository;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace WeatherDataAPI.Controllers
{

    [ApiController]
    [EnableCors]
    [Route("[controller]")]


    public class WeatherReadingsController : ControllerBase
    {
        private readonly IReadingRepository _readingRepository;
        private IValidator<WeatherReadingUpdateDTO> _updateValidator;
        private IValidator<List<WeatherReadingUpdateManyDTO>> _updateManyValidator;
        private IValidator<WeatherReadingFilter> _filterValidator;
        private IValidator<List<WeatherReadingFilter>> _filterManyValidator;
        private IValidator<WeatherReadingBase> _weatherReadingBaseValidator;
        private IValidator<List<WeatherReadingBase>> _weatherReadingBaseManyValidator;
        private IValidator<WeatherReading> _weatherReadingValidator;
        private IValidator<List<WeatherReading>> _weatherReadingManyValidator;


        public WeatherReadingsController(IValidator<WeatherReadingFilter> filterValidator, IValidator<List<WeatherReadingFilter>> filterManyValidator, IValidator<WeatherReadingBase> weatherReadingBaseValidator, IValidator<List<WeatherReadingBase>> weatherReadingBaseManyValidator, IValidator<WeatherReading> weatherReadingValidator, IValidator<List<WeatherReading>> weatherReadingManyValidator, IValidator<WeatherReadingUpdateDTO> updateValidator, IValidator<List<WeatherReadingUpdateManyDTO>> updateManyValidator, IReadingRepository readingRepository)
        {
            _readingRepository = readingRepository;
            _weatherReadingBaseValidator = weatherReadingBaseValidator;
            _weatherReadingBaseManyValidator = weatherReadingBaseManyValidator;
            _weatherReadingValidator = weatherReadingValidator;
            _weatherReadingManyValidator = weatherReadingManyValidator;
            _updateValidator = updateValidator;
            _updateManyValidator = updateManyValidator;
            _filterValidator = filterValidator;
            _filterManyValidator = filterManyValidator;

        }
        /// <summary>
        ///  Gets WeatherReadings that match the filter
        /// </summary>
        // GET: api/<WeatherReadingController>
        [HttpGet]
        [ApiKey(nameof(Permissions.GetWeatherReadings))]

        public async Task<ActionResult<List<WeatherReading>>> weatherReadingsAsync([FromQuery] WeatherReadingFilter? filter)
        {
            ValidationResult validationResult = await _filterValidator.ValidateAsync(filter);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }
            return _readingRepository.GetAll(filter);
        }
        /// <summary>
        ///  Gets a specific WeatherReading
        /// </summary>
        /// <param name="id">The WeatherReading id</param>
        // GET: api/<WeatherReadingController>/5
        [HttpGet("{id}")]
        [ApiKey(nameof(Permissions.GetWeatherReading))]

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherReading))]
        public ActionResult<WeatherReading> weatherReading(string id)
        {

            if (ObjectId.TryParse(id, out _))
            {
                var result = _readingRepository.GetById(id);
                //check that result contains something
                if (result != null)
                {
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest("id must be a valid id");
            }
            return NotFound();



        }
        /// <summary>
        ///  Creates a new WeatherReading
        /// </summary>
        // POST api/<WeatherReadingController>
        [HttpPost]
        [HttpPut]
        [ApiKey(nameof(Permissions.PostWeatherReading), nameof(Permissions.PutWeatherReading))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDTO))]
        public IActionResult Post([FromBody] WeatherReadingBase weatherReading)
        {
            ValidationResult validationResult = _weatherReadingBaseValidator.Validate(weatherReading);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }
            try
            {
                var result = _readingRepository.CreateWeatherReading(weatherReading);
                return CreatedAtAction(nameof(weatherReading), new CreateDTO { id = result.Id }, new { id = result.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Creates new WeatherReadings
        /// </summary>
        //POST api/<WeatherReadingController>/Many
        [ApiKey(nameof(Permissions.PostWeatherReadings))]
        [HttpPost("Many")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateManyDTO))]

        public async Task<IActionResult> PostManyAsync([FromBody] List<WeatherReadingBase> weatherReadings)
        {
            ValidationResult validationResult = await _weatherReadingBaseManyValidator.ValidateAsync(weatherReadings);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }
            try
            {
                var result = _readingRepository.CreateWeatherReadings(weatherReadings);
                //return a list of all ids
                return CreatedAtAction(nameof(weatherReadings), new { ids = result.Select(x => x.Id) }, new CreateManyDTO { ids = result.Select(x => x.Id) });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Deletes a specific WeatherReading
        /// </summary>
        /// <param name="id">The WeatherReading id</param>
        //DELETE api/<WeatherReadingController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.DeleteWeatherReading))]

        public IActionResult Delete(string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    var result = _readingRepository.DeleteWeatherReading(id);
                    if (result)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("id must be a valid id");
                }


            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Delete WeatherReadings that match the filter
        /// </summary>
        //DELETE api/<WeatherReadingController>/Many
        [HttpDelete("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        [ApiKey(nameof(Permissions.DeleteWeatherReadings))]


        public async Task<IActionResult> DeleteManyAsync([FromQuery] WeatherReadingFilter filter)
        {
            ValidationResult validationResult = await _filterValidator.ValidateAsync(filter);
            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }
            try
            {
                var result = _readingRepository.DeleteWeatherReadings(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Replace a specific WeatherReading with a new WeatherReading
        /// </summary>
        /// <param name="id">The WeatherReading id</param>
        //PUT api/<WeatherReadingController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.PutWeatherReading))]

        public async Task<IActionResult> ReplaceAsync([FromBody] WeatherReadingBase weatherReading, string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    ValidationResult validationResult = await _weatherReadingBaseValidator.ValidateAsync(weatherReading);
                    if (!validationResult.IsValid)
                    {
                        return new BadRequestObjectResult(validationResult.Errors);
                    }
                    var newWeatherReading = weatherReading.ToReading();
                    newWeatherReading.Id = id;
                    var result = _readingRepository.ReplaceWeatherReading(newWeatherReading);
                    if (result)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("id must be a valid id");
                }

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Replace WeatherReadings with provided new WeatherReadings
        /// </summary>
        //PUT api/<WeatherReadingController>/Many
        [HttpPut("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        [ApiKey(nameof(Permissions.PutWeatherReadings))]
        public async Task<IActionResult> ReplaceManyAsync([FromBody] List<WeatherReading> weatherReadings)
        {
            try
            {
                ValidationResult validationResult = await _weatherReadingManyValidator.ValidateAsync(weatherReadings);
                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(validationResult.Errors);
                }
                var result = _readingRepository.ReplaceWeatherReadings(weatherReadings);
                if (result > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Update information for a specific WeatherReading
        /// </summary>
        /// <param name="id">The WeatherReading id</param>
        //PATCH api/<WeatherReadingController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.PatchWeatherReading))]

        public async Task<IActionResult> UpdateAsync([FromBody] WeatherReadingUpdateDTO weatherReading, string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    ValidationResult validationResult = await _updateValidator.ValidateAsync(weatherReading, options =>
                    {
                        options.IncludeRuleSets("Update");
                    });
                    if (!validationResult.IsValid)
                    {
                        return new BadRequestObjectResult(validationResult.Errors);
                    }
                    var result = _readingRepository.UpdateWeatherReading(weatherReading, id);
                    if (result == 1)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("id must be a valid id");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Update information for many WeatherReadings
        /// </summary>
        //PATCH api/<WeatherReadingController>/Many
        [HttpPatch("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        [ApiKey(nameof(Permissions.PatchWeatherReadings))]

        public async Task<IActionResult> UpdateManyAsync([FromBody] List<WeatherReadingUpdateManyDTO> weatherReadings)
        {
            try
            {
                ValidationResult validationResult = await _updateManyValidator.ValidateAsync(weatherReadings, options =>
                {
                    options.IncludeRuleSets("Update");
                });
                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(validationResult.Errors);
                }
                var result = _readingRepository.UpdateWeatherReadings(weatherReadings);
                if (result >= 1)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }


        //Statistics stuff from here on out
        /// <summary>
        ///  Find the maximum precipitation reading for a specific device for the last five months
        /// </summary>
        /// <param name="deviceName">The device name</param>
        //GET api/<WeatherReadingController>/Statistics/MaxPrecipitationForLastFiveMonths
        [HttpGet("Statistics/MaxPrecipitationForLastFiveMonths/{deviceName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherReadingMaxPrecipitationDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.GetMaxPrecipitationForLastFiveMonths))]

        public IActionResult MaxPrecipitationForLastFiveMonths(string deviceName)
        {

            try
            {
                WeatherReadingFilter filter = new WeatherReadingFilter();
                filter.DeviceName = deviceName;
                filter.Limit = 1;
                filter.SortBy = WeatherReadingNames.Precipitation;
                filter.SortOrder = "desc";
                filter.StartTime = DateTime.Now.AddMonths(-5);
                filter.Projection = new WeatherReadingProjection
                {
                    DeviceName = true,
                    Time = true,
                    Precipitation = true
                };
                var result = _readingRepository.GetAll(filter);
                if (result != null && result.Count > 0)
                {
                    if (result.Count == 1)
                    {
                        return Ok(new WeatherReadingMaxPrecipitationDTO
                        {
                            Id = result[0].Id,
                            DeviceName = result[0].DeviceName,
                            Time = result[0].Time,
                            Precipitation = result[0].Precipitation
                        });
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Find all readings at a specific station for a specific hour
        /// </summary>
        /// <param name="deviceName">The device name</param>
        /// <param name="hour">The desired hour. Will only be precise to the hour and will ignore any minutes, seconds, etc...</param>
        //GET api/<WeatherReadingController>/Statistics/ReadingsAtSpecificHourForStation{deviceName}
        [HttpGet("Statistics/ReadingsAtSpecificHourForStation/{deviceName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WeatherReadingHourDTO>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.GetReadingsAtSpecificHourForStation))]

        public IActionResult ReadingsAtSpecificHourForStation(string deviceName, [FromQuery, Required] DateTime hour)
        {
            try
            {
                var result = _readingRepository.ReadingsAtSpecificHourForStation(deviceName, hour);
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Find the maximum recorded temperature for all devices between two times
        /// </summary>
        /// <param name="startTime">The oldest time allowed</param>
        /// <param name="endTime">The newest time allowed</param>
        //GET api/<WeatherReadingController>/Statistics/MaxTempBetWeenTimes
        [HttpGet("Statistics/MaxTempBetWeenTimes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<WeatherReadingMaxTempDTO>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ApiKey(nameof(Permissions.GetMaxTempBetweenTimes))]
        public IActionResult MaxTempBetweenTimes([FromQuery, Required] DateTime startTime, [FromQuery, Required] DateTime endTime)
        {
            try
            {
                var result = _readingRepository.MaxTempBetweenTimes(startTime, endTime);
                if (result != null && result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }


        }

    }
}
