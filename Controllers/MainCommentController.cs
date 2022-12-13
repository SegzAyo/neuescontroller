using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neues.Core.DTO;
using Neues.Core.Models.DTO;
using Neues.Interface;
using Neues.Model;
using NeuesCore.Models;

namespace NeuesWebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/maincomment")]
    public class MainCommentController : ControllerBase
    {
        private readonly IMainComment _mainSqlCommentService;

        public MainCommentController(ILogger<MainCommentController> logger, IMainComment mainSqlCommentService)
        {
            _mainSqlCommentService = mainSqlCommentService;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/maincomment/all-maincomments")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<MainComment>> GetMainComments([FromQuery] GetAllMainCommentDTO getAllMainCommentDTO)
        {

            try
            {
                Post post = new Post();

                post.Id = getAllMainCommentDTO.PostId;



                return await _mainSqlCommentService.GetAllMainComment(post);
            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }

        }


        [HttpPost]
        [Route("~/main-comment/create-maincomment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMainCommnet([FromBody] MainCommentDTO mainCommentDto)
        {
            try
            {
                MainComment mainComment = new MainComment();
                mainComment.Message = mainCommentDto.Message;
                //mainComment.UserName = mainCommentDto.UserName;
                mainComment.PostId = mainCommentDto.PostId;
                mainComment.UserId = mainCommentDto.UserId;

                await _mainSqlCommentService.CreateMainComment(mainComment);
                return Ok(mainComment);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        [Route("~/main-comment/update-maincomment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMainComment([FromBody] UdateMainCommentDTO mainCommentDTO)
        {
            try
            {
                MainComment mainComment = new MainComment();
                mainComment.Id = mainCommentDTO.Id;
                mainComment.Message = mainCommentDTO.Message;

                await _mainSqlCommentService.UpdateMainComment(mainComment);
                return Ok(mainComment);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpDelete]
        [Route("~/main-comment/delete-maincomment/{Id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            try
            {
                await _mainSqlCommentService.DeleteMainComment(Id);
                return Ok(Id);
            }
            catch (Exception e)
            {

                throw new Exception($"Post not found, {e.Message}");
            }
        }

    }
 }
