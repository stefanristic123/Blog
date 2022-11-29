using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Blog.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IWriterRepository _writerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public PostController(
            IPostRepository postRepository,
            IWriterRepository writerRepository,
            ICategoryRepository categoryRepository,
            IPhotoService photoService,
            IMapper mapper
            )
        {
            _photoService = photoService;
            _postRepository = postRepository;
            _writerRepository = writerRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPosts()
        {
            var x = _mapper.Map<List<PostDto>>(_postRepository.GetPosts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpGet("{postId}")] 
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        public IActionResult GetPost(int postId)
        {
            //if (!_postRepository.PokemonExists(pokeId))
            //    return NotFound();

            var x = _mapper.Map<PostDto>(_postRepository.GetPost(postId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePost(
            [FromQuery] int writerId,
            [FromQuery] int categoryId,
            [FromBody] PostDto postCreate
            )
        {
            if (postCreate == null)
                return BadRequest(ModelState);

            var post = _postRepository.GetPostTrimToUpper(postCreate);
            if (post != null)
            {
                ModelState.AddModelError("", "Post already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var map = _mapper.Map<Post>(postCreate);

            map.Writer = _writerRepository.GetWriter(writerId);
            //x.Category = _categoryRepository.GetCategory(categoryId);

            if (!_postRepository.CreatePost(categoryId, map))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePost([FromBody] Post update)
        {
            if (update == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var map = _mapper.Map<Post>(update);

            if (!_postRepository.UpdatePost(map))
            {
                ModelState.AddModelError("", "Something went wrong updating post");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Photo>> AddPhoto(IFormFile file)
        {
            // var post = _mapper.Map<PostDto>(_postRepository.GetPost(1));

            // if (post == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            return photo; 

            // if (post.Photos.Count == 0) photo.IsMain = true;

            // post.Photos.Add(photo);

            // if (await _postRepository.SaveAllAsync())  return _mapper.Map<Photo>(photo);
            
            // return BadRequest("Problem");

        }
    }

}
