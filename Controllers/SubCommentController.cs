using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neues.Core.DTO;
using Neues.Core.Interface;
using Neues.Core.Models.DTO;
using Neues.Interface;
using Neues.Model;
using NeuesCore.Models;

namespace NeuesWebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/subcomment")]
    public class SubCommentController : Controller
    {
        private readonly ISubComment _subCommentSqlService;

        public SubCommentController(ILogger<SubCommentController> logger, ISubComment subCommentSqlService)
        {
            _subCommentSqlService = subCommentSqlService;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/subcomment/all-subcomments")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<SubComment>> GetSubComments([FromQuery]GetAllSubCommentDTO allSubCommentDTO)
        {
            try
            {
                MainComment mainComment = new MainComment();   
                mainComment.Id = allSubCommentDTO.MainCommentId;

                return await _subCommentSqlService.GetSubComments(mainComment);
            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }
        }


        [HttpPost]
        [Route("~/subcomment/create-subcomment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddSubCommnet([FromBody]SubCommentDTO subCommentDTO)
        {
            try
            {
                SubComment subComment = new SubComment();
                subComment.UserId = subCommentDTO.UserId;
                subComment.MainCommentId = subCommentDTO.MainCommentId;
                subComment.Message = subCommentDTO.Message;
               

                await _subCommentSqlService.CreateSubComment(subComment);
                return Ok(subComment);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpPut]
        [Route("~/subcomment/update-subcomment")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSubComment([FromBody] UpdateSubCommentDTO subCommentDTO)
        {
            try
            {
                SubComment subComment = new SubComment();
                subComment.Id = subCommentDTO.Id;
                subComment.Message = subCommentDTO.Message;
              
                await _subCommentSqlService.UpdateSubComment(subComment);
                return Ok(subComment);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpDelete]
        [Route("~/subcomment/delete-subcomment/{Id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSubcomment(Guid Id)
        {
            try
            {
                await _subCommentSqlService.DeleteSubComment(Id);
                return Ok(Id);
            }
            catch (Exception e)
            {

                throw new Exception($"Comment not found, {e.Message}");
            }
        }

    }
}

