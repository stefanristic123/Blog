using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IWriterRepository _writerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(
            IPostRepository postRepository,
            IWriterRepository writerRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _writerRepository = writerRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers()
        {
            var x = _mapper.Map<List<User>>(_userRepository.GetUsers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var x = _mapper.Map<User>(_userRepository.GetUser(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] User userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var xx = _userRepository.GetUsers()
                .FirstOrDefault();

            if (xx != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var x = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(x))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int id, [FromBody] User update)
        {
            if (update == null)
                return BadRequest(ModelState);

            if (id != update.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var map = _mapper.Map<User>(update);

            if (!_userRepository.UpdateUser(map))
            {
                ModelState.AddModelError("", "Something went wrong updating user");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int id)
        {
            if (!_userRepository.UserExists(id))
            {
                return NotFound();
            }

            var x = _userRepository.GetUser(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(x))
            {
                ModelState.AddModelError("", "Something went wrong deleting user");
            }

            return NoContent();
        }
    }
}

