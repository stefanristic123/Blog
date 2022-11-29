using System;
using AutoMapper;
using Blog.Dto;
using Blog.Interfaces;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController] 
    public class WriterController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IWriterRepository _writerRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public WriterController(
            IPostRepository postRepository,
            IWriterRepository writerRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper
            )
        {
            _postRepository = postRepository;
            _writerRepository = writerRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Writer>))]
        public IActionResult GetWriters()
        {
            var x = _mapper.Map<List<Writer>>(_writerRepository.GetWriters());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }


        [HttpGet("{writerId}")]
        [ProducesResponseType(200, Type = typeof(Writer))]
        [ProducesResponseType(400)]
        public IActionResult GetWriter(int writerId)
        {
            //if (!_postRepository.PokemonExists(pokeId))
            //    return NotFound();

            var x = _mapper.Map<Writer>(_writerRepository.GetWriter(writerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(x);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateWriter(
            [FromBody] WriterDto writerCreate
            )
        {
            if (writerCreate == null)
                return BadRequest(ModelState);

            var writerExists = _writerRepository.GetWriters()
                .Where(c => c.Name.Trim().ToUpper() == writerCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (writerExists != null)
            {
                ModelState.AddModelError("", "Writer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var writer = _mapper.Map<Writer>(writerCreate);

            if (!_writerRepository.CreateWriter(writer))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }
    }
}

