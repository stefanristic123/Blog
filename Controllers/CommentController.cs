using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentController(
            IPostRepository postRepository,
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
        public IActionResult GetComments()
        {
            var x = _mapper.Map<List<Comment>>(_commentRepository.GetComments());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }


        [HttpGet("{commentId}")]
        [ProducesResponseType(200, Type = typeof(Comment))]
        [ProducesResponseType(400)]
        public IActionResult GetComment(int id)
        {
            if (!_commentRepository.CommentExists(id))
                return NotFound();

            var x = _mapper.Map<Comment>(_commentRepository.GetComment(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateComment(
            [FromQuery] int userId,
            [FromQuery] int postId,
            [FromBody] Comment create)
        {
            if (create == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var map = _mapper.Map<Comment>(create);

            map.User = _userRepository.GetUser(userId);
            map.Post = _postRepository.GetPost(postId);

            if (!_commentRepository.CreateComment(map))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateComment(int id, [FromBody] Comment update)
        {
            if (update == null)
                return BadRequest(ModelState);

            if (id != update.Id)
                return BadRequest(ModelState);

            if (!_commentRepository.CommentExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var x = _mapper.Map<Comment>(update);

            if (!_commentRepository.UpdateComment(x))
            {
                ModelState.AddModelError("", "Something went wrong updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteComment(int id)
        {
            if (!_commentRepository.CommentExists(id))
            {
                return NotFound();
            }

            var x = _commentRepository.GetComment(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_commentRepository.DeleteComment(x))
            {
                ModelState.AddModelError("", "Something went wrong deleting");
            }

            return NoContent();
        }

        [HttpGet("post/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        [ProducesResponseType(400)]
        public IActionResult GetCommentsForPost(int id)
        {
            var pokemons = _mapper.Map<List<PostDto>>(
                _commentRepository.GetCommentsForPost(id));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(pokemons);
        }

    }
}

