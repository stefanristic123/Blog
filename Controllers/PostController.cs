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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public PostController(
            IPostRepository postRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IPhotoService photoService,
            IMapper mapper
            )
        {
            _photoService = photoService;
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
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
            [FromQuery] int userId,
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

            // map.Writer = _writerRepository.GetWriter(writerId);
            // //x.Category = _categoryRepository.GetCategory(categoryId);

            // if (!_postRepository.CreatePost(categoryId, map))
            // {
            //     ModelState.AddModelError("", "Something went wrong while saving");
            //     return StatusCode(500, ModelState);
            // }

            // return Ok("Successfully created");

            var user = _userRepository.GetUser(userId);
            var userRole = user.UserRoles?.First().RoleId;
            if (userRole == 2){
                map.User = user;
                if (!_postRepository.CreatePost(categoryId, map))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }
                
            return Ok("Successfully created");
            } else {
                ModelState.AddModelError("", "You are not authorized"); 
                return StatusCode(401, ModelState);
            }
            
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

        [HttpPost("like/{postId}")]
        public async Task<ActionResult> LikePost(
            [FromQuery] int userId,
            [FromQuery] int postId,
            [FromBody] Like likePost
        ) 
        {
            if (likePost == null)
                return BadRequest(ModelState);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var map = _mapper.Map<Like>(likePost);

            map.User = _userRepository.GetUser(userId);
            map.Post = _postRepository.GetPost(postId);

            if (!_postRepository.LikePost(map))
            {
                ModelState.AddModelError("", "Something went wrong while trying to like the post");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully liked");

        }

        [HttpPost("{postId}")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(int postId, IFormFile file)
        {
            var post = _mapper.Map<Post>(_postRepository.GetPost(postId));

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (post.Photos.Count == 0) photo.IsMain = true;

            post.Photos.Add(photo);

            if (await _postRepository.SaveAllAsync()){
                return _mapper.Map<PhotoDto>(photo); 
            }  
            
                return BadRequest("Problem uploading photo");
        }

        [HttpPut("set-main-photo/{postId}")]
        public async Task<ActionResult> SetMainPhoto(int postId) 
        {
            var post = _mapper.Map<Post>(_postRepository.GetPost(postId));

            if (post == null) return NotFound();

            var photo = post.Photos.FirstOrDefault(x => x.Id == postId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("this is already your main photo");

            var currentMain = post.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _postRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");
        }

         [HttpDelete("delete-photo/{postId}/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId, int postId)
        {
            var post = _mapper.Map<Post>(_postRepository.GetPost(postId));

            var photo = post.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            post.Photos.Remove(photo);

            if (await _postRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem deleting photo");
        }
    }

}
