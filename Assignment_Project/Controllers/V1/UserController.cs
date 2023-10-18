using Asp.Versioning;
using Assignment_Project.Data;
using Assignment_Project.Dtos;
using Assignment_Project.Models;
using Assignment_Project.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Assignment_Project.Controllers
{
    [Route("api/v{version:apiVersion}/User")]
    [ApiController]
    [ApiVersion(1.0)]
    public class UserController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUserRepository _dbUser;
        private readonly IMapper _mapper;

        public UserController(IUserRepository dbUser, IMapper mapper)
        {
            this._dbUser = dbUser;
            this._mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> users = await _dbUser.GetAllAsync();
                _response.Result = _mapper.Map<IEnumerable<UserDTO>>(users);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString()};
            }
            return _response;
        }

        [HttpGet("{id}", Name = "GetUser")]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUser(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var user = await _dbUser.GetAsync(u => u.UserId == id);

                if (user == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<IEnumerable<UserDTO>>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUser([FromBody] UserDTO newUser)
        {
            try 
            { 
                if (newUser == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                User userEntity = _mapper.Map<User>(newUser);
                await _dbUser.CreateAsync(userEntity);

                _response.Result = _mapper.Map<UserDTO>(userEntity);
                _response.StatusCode =HttpStatusCode.Created;

                return CreatedAtRoute("GetUser", new { id = userEntity.UserId }, _response);

            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteUser(int id)
        {
            try
            {

            
                if (id <= 0)
                {
                    return BadRequest();
                }

                var user = await _dbUser.GetAsync(u => u.UserId == id);

                if (user == null)
                {
                    return NotFound();
                }

                await _dbUser.RemoveAsync(user);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            try 
            { 
                if (userDTO == null || id != userDTO.UserId)
                {
                    return BadRequest();
                }

                var userEntity = await _dbUser.GetAsync(u => u.UserId == id);

                if (userEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(userDTO, userEntity);
                await _dbUser.UpdateAsync(userEntity);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialUser")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialUser(int id, [FromBody] JsonPatchDocument<UserDTO> patchDTO)
        {
            if (patchDTO == null || id <= 0)
            {
                return BadRequest();
            }

            var userEntity = await _dbUser.GetAsync(u => u.UserId == id);

            if (userEntity == null)
            {
                return NotFound();
            }

            var userDTO = _mapper.Map<UserDTO>(userEntity);
            patchDTO.ApplyTo(userDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(userDTO, userEntity);
            await _dbUser.UpdateAsync(userEntity);

            return NoContent();
        }
    }
}
