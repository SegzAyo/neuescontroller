using System.IO;
using Microsoft.AspNetCore.Mvc;
using Neues.Core.Models;
using Neues.Infrastructure;
using Neues.Interface;
using Neues.Model;
using Neues.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;
using Neues.Core.DTO;
using System.Data;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Hosting;

namespace NeuesWebApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/post")]
    public class NeuesPostController : ControllerBase
    {
        private readonly IPost _postSqlService;

        //private readonly BlobServiceClient _blobServiceClient;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public NeuesPostController(ILogger<NeuesPostController> logger, IPost postSqlService, IWebHostEnvironment webHostEnvironment)
        {
            _postSqlService = postSqlService;
            _webHostEnvironment = webHostEnvironment;
            //_blobServiceClient = blobServiceClient;

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/post/all-posts/{param}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<Post>> GetPosts(string param)
        {
            try
            {
                var posts = await _postSqlService.GetAllPost(param);
                return posts;

            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }



        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/post/home")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IEnumerable<Post>> GetPostsHome()
        {
            try
            {
                var posts = await _postSqlService.PostHome();
                return posts;

            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }



        }
        //    var posts = await _postSqlService.GetAllPost();
        //    if (posts != null)
        //    {
        //        foreach(var post in posts)
        //        {
        //            if (post.Image != null && post.ThumbNail != null)
        //            {

        //                Stream ThumbNailStream = new MemoryStream(post.ThumbNail);
        //                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
        //                string FilePath = Path.Combine(path, post.ThumbNailFileName);
        //                post.ThumbNailFilePath = FilePath;
        //                if (!System.IO.File.Exists(FilePath))
        //                {
        //                    using (var stream = new FileStream(FilePath, FileMode.Create))
        //                    {
        //                        ThumbNailStream.CopyTo(stream);
        //                    }
        //                }

        //                Stream ImageStream = new MemoryStream(post.Image);
        //                string Imagepath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadimage");
        //                string ImageFilePath = Path.Combine(Imagepath, post.ImageFileName);
        //                post.ImageFilePath = ImageFilePath;
        //                if (!System.IO.File.Exists(ImageFilePath))
        //                {
        //                    using (var stream = new FileStream(ImageFilePath, FileMode.Create))
        //                    {
        //                        ImageStream.CopyTo(stream);
        //                    }
        //                }

        //            }
        //            else if (post.Video != null && post.ThumbNail != null)
        //            {
        //                Stream ThumbNailStream = new MemoryStream(post.ThumbNail);
        //                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
        //                string FilePath = Path.Combine(path, post.ThumbNailFileName);
        //                post.ThumbNailFilePath = FilePath;
        //                if (!System.IO.File.Exists(FilePath))
        //                {
        //                    using (var stream = new FileStream(FilePath, FileMode.Create))
        //                    {
        //                        ThumbNailStream.CopyTo(stream);
        //                    }
        //                }

        //                Stream VideoStream = new MemoryStream(post.Video);
        //                string Videopath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadvideo");
        //                string VideoFilePath = Path.Combine(Videopath, post.VideoFileName);
        //                post.VideoFilePath = VideoFilePath;
        //                if (!System.IO.File.Exists(VideoFilePath))
        //                {
        //                    using (var stream = new FileStream(VideoFilePath, FileMode.Create))
        //                    {
        //                        VideoStream.CopyTo(stream);
        //                    }
        //                }
        //            }
        //            else if (post.ThumbNail != null)
        //            {
        //                Stream ThumbNailStream = new MemoryStream(post.ThumbNail);
        //                string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
        //                string FilePath = Path.Combine(path, post.ThumbNailFileName);
        //                post.ThumbNailFilePath = FilePath;
        //                if (!System.IO.File.Exists(FilePath))
        //                {
        //                    using (var stream = new FileStream(FilePath, FileMode.Create))
        //                    {
        //                        ThumbNailStream.CopyTo(stream);
        //                    }
        //                }



        [AllowAnonymous]
        [HttpGet]
        [Route("~/post/post-detail/{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task <Post> GetPost(Guid id)
        {

            try
            {
                var post = await _postSqlService.GetPost(id);
                //if (post.Image != null && post.ThumbNail != null)
                //{
                //    string base64String = Convert.ToBase64String(post.Video, 0, post.Video.Length)

                //    Stream ThumbNailStream = new MemoryStream(post.ThumbNail);
                //    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
                //    string FilePath = Path.Combine(path, post.ThumbNailFileName);
                //    post.ThumbNailFilePath = FilePath;
                //    if (!System.IO.File.Exists(FilePath))
                //    {
                //        using (var stream = new FileStream(FilePath, FileMode.Create))
                //        {
                //            ThumbNailStream.CopyTo(stream);
                //        }
                //    }

                //    Stream ImageStream = new MemoryStream(post.Image);
                //    string Imagepath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadimage");
                //    string ImageFilePath = Path.Combine(Imagepath, post.ImageFileName);
                //    post.ImageFilePath = ImageFilePath;
                //    if (!System.IO.File.Exists(ImageFilePath))
                //    {
                //        using (var stream = new FileStream(ImageFilePath, FileMode.Create))
                //        {
                //            ImageStream.CopyTo(stream);
                //        }
                //    }

                //    //post.ThumbNail = this.GetImage(Convert.ToBase64String(pImageost.ThumbNail));
                //    //post.Image = this.GetImage(Convert.ToBase64String(post.Image));
                //}
                //else if (post.Video != null && post.ThumbNail != null)
                //{
                //    Stream ThumbNailStream = new MemoryStream(post.ThumbNail);
                //    string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
                //    string FilePath = Path.Combine(path, post.ThumbNailFileName);
                //    post.ThumbNailFilePath = FilePath;
                //    if (!System.IO.File.Exists(FilePath))
                //    {
                //        using (var stream = new FileStream(FilePath, FileMode.Create))
                //        {
                //            ThumbNailStream.CopyTo(stream);
                //        }
                //    }

                //    Stream VideoStream = new MemoryStream(post.Video);
                //    string Videopath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadvideo");
                //    string VideoFilePath = Path.Combine(Videopath, post.VideoFileName);
                //    post.VideoFilePath = VideoFilePath;
                //    if (!System.IO.File.Exists(VideoFilePath))
                //    {
                //        using (var stream = new FileStream(VideoFilePath, FileMode.Create))
                //        {
                //            VideoStream.CopyTo(stream);
                //        }
                //    }
                //    //post.ThumbNail = this.GetImage(Convert.ToBase64String(post.ThumbNail));
                //    //post.Video = this.GetImage(Convert.ToBase64String(post.Video));
                //}

                return post;
            }
            catch (Exception e)
            {
                throw new Exception($"Conflict, {e.Message}");
            }

        }


        [HttpPost]
        [Route("~/post/create-post")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<Post> AddPost([FromForm] PostDTO postDTO)
        {
            try
            {
                var post = new Post();
                post.Tags = postDTO.Tags;
                post.Body = postDTO.Body;
                post.Category = postDTO.Category;
                post.DateModified = DateTime.Now;
                post.DateCreated = DateTime.Now;
                post.Youtube = postDTO.Youtube;
                post.Description = postDTO.Description;
                post.Title = postDTO.Title;

                if (postDTO.ThumbNail != null)
                {
                    using (var thumbNailMemoryStream = new MemoryStream())
                    {
                        postDTO.ThumbNail.CopyTo(thumbNailMemoryStream);

                        post.ThumbNail = thumbNailMemoryStream.ToArray();
                        //post.ThumbNailFileName = postDTO.ThumbNail.FileName;
                        //string path = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadthumbnail");
                        //string FilePath = Path.Combine(path, post.ThumbNailFileName);
                        //post.ThumbNailFilePath = FilePath;
                    }

                    if (postDTO.Image != null)
                    {

                        using (var imageMemoryStream = new MemoryStream())
                        {
                            postDTO.Image.CopyTo(imageMemoryStream);

                            post.Image = imageMemoryStream.ToArray();
                            //post.ImageFileName = postDTO.Image.FileName;
                            //string Imagepath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadimage");
                            //string ImageFilePath = Path.Combine(Imagepath, post.ImageFileName);
                            //post.ImageFilePath = ImageFilePath;
                        }
                    }
                    else if (postDTO.Video != null)
                    {
                        using (var videoMemoryStream = new MemoryStream())
                        {
                            postDTO.Video.CopyTo(videoMemoryStream);

                            post.Video = videoMemoryStream.ToArray();
                            //post.VideoFileName = postDTO.Video.FileName;
                            //string Videopath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadvideo");
                            //string VideoFilePath = Path.Combine(Videopath, post.VideoFileName);
                            //post.VideoFilePath = VideoFilePath;
                        }
                    }
                }
                await _postSqlService.CreatePost(post);
                return post;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }       
        }


        [HttpPut]
        //[Authorize(Roles ="Admin")]
        [Route("~/post/update-post")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePost([FromForm]PutDTO postDTO)
        {
            try
            {
                var post = new Post();
                post.Tags = postDTO.Tags;
                post.Body = postDTO.Body;
                post.Category = postDTO.Category;
                post.DateModified = DateTime.Now;
                post.DateCreated = DateTime.Now;
                post.Description = postDTO.Description;
                post.Title = postDTO.Title;
                post.Id = postDTO.Id;

                if (postDTO.ThumbNail != null)
                {
                    using (var thumbNailMemoryStream = new MemoryStream())
                    {
                        postDTO.ThumbNail.CopyTo(thumbNailMemoryStream);

                        post.ThumbNail = thumbNailMemoryStream.ToArray();
                        //post.ThumbNailFileName = postDTO.ThumbNail.FileName;
                    }
                }

                    if (postDTO.Image != null)
                    {

                        using (var imageMemoryStream = new MemoryStream())
                        {
                            postDTO.Image.CopyTo(imageMemoryStream);

                            post.Image = imageMemoryStream.ToArray();
                            //post.ImageFileName = postDTO.Image.FileName;
                        }
                    }
                    else if (postDTO.Video != null)
                    {
                        using (var videoMemoryStream = new MemoryStream())
                        {
                            postDTO.Video.CopyTo(videoMemoryStream);

                            post.Video = videoMemoryStream.ToArray();
                            //post.VideoFileName = postDTO.Video.FileName;
                        }
                    }

                await _postSqlService.UpdatePost(post);
                return Ok(post);
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        [HttpDelete]
        [Route("~/post/delete-post/{Id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(Guid Id)
        {
            try
            {
                await _postSqlService.DeletePost(Id);
                return Ok(Id);
            }
            catch (Exception e)
            {

                throw new Exception($"Post not found, {e.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/post/related-post")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<Post>> RelatedPost([FromQuery]string relatedPost)
        {
            try
            {
                //var post = new Post();
                //post.Tags = relatedPostDto;

                var posts= await _postSqlService.RelatedPost(relatedPost);
                return posts;
            }
            catch
            {
                throw new Exception("No related posts");
            }
        }



        private byte[] GetImage(string Base64Image)
        {
            byte[] imageBytes = null;
            if (!string.IsNullOrEmpty(Base64Image))
            {
                imageBytes = Convert.FromBase64String(Base64Image);
            }
            return imageBytes;
        }
    }
 }   



