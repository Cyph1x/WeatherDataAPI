using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WeatherDataAPI.Attributes;
using WeatherDataAPI.Models.AppUser;
using WeatherDataAPI.Models.User;
using WeatherDataAPI.Repository;
using Swashbuckle.AspNetCore;
using MongoDB.Bson;
using WeatherDataAPI.Models.WeatherReadings;
using WeatherDataAPI.Models;
using WeatherDataAPI.Models.WeatherReadings.Validation;
using WeatherDataAPI.Models.User.DTO;
using WeatherDataAPI.Models.User.Validation;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;
using Microsoft.AspNetCore.Cors;

namespace WeatherDataAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiKey(nameof(Permissions.Admin))]
    [EnableCors]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private IValidator<AppUserFilter> _filterValidator;
        private IValidator<AppUserBase> _appUserBaseValidator;
        private IValidator<List<AppUserBase>> _appUserBaseManyValidator;
        private IValidator<List<AppUserCreateManyDTO>> _appUserCreateManyValidator;
        private IValidator<List<AppUserUpdateManyDTO>> _appUserUpdateManyValidator;

        public UsersController(IValidator<AppUserFilter> filterValidator, IValidator<AppUserBase> appUserValidator, IValidator<List<AppUserBase>> appUserBaseManyValidator, IValidator<List<AppUserCreateManyDTO>> appUserCreateManyValidator, IValidator<List<AppUserUpdateManyDTO>> appUserUpdateManyValidator, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _filterValidator = filterValidator;
            _appUserBaseValidator = appUserValidator;
            _appUserBaseManyValidator = appUserBaseManyValidator;
            _appUserCreateManyValidator = appUserCreateManyValidator;
            _appUserUpdateManyValidator = appUserUpdateManyValidator;
        }

        


        /// <summary>
        ///  Gets Users that match the filter
        /// </summary>
        //GET: api/<UsersController>/
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AppUser>))]
        public ActionResult GetUsers([FromQuery] AppUserFilter filter)
        {
            ValidationResult validationResult = _filterValidator.Validate(filter);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var users = _userRepository.GetUsers(filter);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        /// <summary>
        ///  Creates a new user
        /// </summary>
        //POST: api/<UsersController>
        //PUT: api/<UsersController>
        [HttpPost]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateDTO))]
        public ActionResult CreateUser([FromBody] AppUserBase user) 
        {
            ValidationResult validationResult = _appUserBaseValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = _userRepository.CreateUser(user);
            if (result != null)
            {
                return CreatedAtRoute("", new { id = result }, result);
            }
            else
            { 
                return BadRequest("User Already Exists");
            }
        }

        /// <summary>
        ///  Gets a specific user
        /// </summary>
        /// <param name="id">The user id</param>
        //GET: api/<UsersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AppUser))]
        public ActionResult GetUser(string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    var user = _userRepository.GetUser(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    return Ok(user);
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
        ///  Delete a specific user
        /// </summary>
        /// <param name="id">The user id</param>
        //DELETE: api/<UsersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult DeleteUser(string id)
        {
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    var user = _userRepository.GetUser(id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    _userRepository.DeleteUser(id);
                    return NoContent();
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
        ///  Replace a specific user with a new user
        /// </summary>
        /// <param name="id">The user id</param>
        //PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult Replace([FromBody] AppUserBase user, string id)
        {
            ValidationResult validationResult = _appUserBaseValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    var newUser = user.ToAppUser();
                    newUser.Id = id;
                    var result = _userRepository.ReplaceUser(newUser);
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
        ///  Update information for a specific user
        /// </summary>
        /// <param name="id">The user id</param>
        //PATCH api/<UsersController>/5
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public ActionResult Update([FromBody] AppUserBase user, string id)
        {
            ValidationResult validationResult = _appUserBaseValidator.Validate(user, options =>
            {
                options.IncludeRuleSets("Update");
            });
            //ValidationResult validationResult = _appUserBaseValidator.Validate(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                if (ObjectId.TryParse(id, out _))
                {
                    var result = _userRepository.UpdateUser(user, id);
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
        ///  Update information for many users
        /// </summary>
        //PATCH api/<UsersController>/Many
        [HttpPatch("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public ActionResult UpdateMany([FromBody] List<AppUserUpdateManyDTO> users)
        {
            ValidationResult validationResult = _appUserUpdateManyValidator.Validate(users, options =>
            {
                options.IncludeRuleSets("Update");
            });
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                
                var result = _userRepository.UpdateUsers(users);
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
        /// <summary>
        ///  Replace Users with provided new users
        /// </summary>
        //PUT api/<UsersController>/Many
        [HttpPut("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public ActionResult ReplaceMany([FromBody] List<AppUserCreateManyDTO> users)
        {
            ValidationResult validationResult = _appUserCreateManyValidator.Validate(users);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {

                var result = _userRepository.ReplaceUsers(users);
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
        ///  Delete Users that match the filter
        /// </summary>
        //DELETE: api/<UsersController>/Many
        [HttpDelete("Many")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public ActionResult DeleteUsers([FromQuery] AppUserFilter filter)
        {
            ValidationResult validationResult = _filterValidator.Validate(filter);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var result = _userRepository.DeleteUsers(filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        ///  Creates new users
        /// </summary>
        //POST: api/<UsersController>/
        [HttpPost("Many")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateManyDTO))]
        public ActionResult CreateUsers([FromBody] List<AppUserBase> users)
        {
            ValidationResult validationResult = _appUserBaseManyValidator.Validate(users);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            try
            {
                var result = _userRepository.CreateUsers(users);
                //return a list of all ids
                return CreatedAtRoute("", new { ids = result.Select(x => x.Id) }, new CreateManyDTO { ids = result.Select(x => x.Id) });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }



    }
}
