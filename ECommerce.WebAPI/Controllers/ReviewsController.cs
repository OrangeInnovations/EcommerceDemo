using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Core.Data;
using ECommerce.Core.Entities;
using ECommerce.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebAPI.Controllers
{
    /// <summary>Product catelog APIs
    /// 
    /// </summary>
    [Route("api/products/{productId}/reviews")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewsController : AuthController
    {
        private readonly IECommerceRepository _repository;
        private readonly ILogger<ReviewsController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;
        private readonly ITrackReviewService _trackReviewService;

        public ReviewsController(IECommerceRepository repository, ILogger<ReviewsController> logger, 
            IMapper mapper, UserManager<StoreUser> userManager, ITrackReviewService trackReviewService)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _trackReviewService = trackReviewService;
        }

        [HttpGet(Name = "GetProductViews")]
        public async Task<IActionResult> GetAllProductReviews(int productId)
        {
            try
            {

                var results =await _repository.GetReviewsAsync(productId);

                var data = _mapper.Map<IEnumerable<Review>, IEnumerable<ReviewViewModel>>(results);

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get reviews: {ex}");
                return BadRequest("Failed to get reviews");
            }
        }

        [HttpGet("{id}",Name = "GetReview")]
        public async Task<IActionResult> Get(int productId,string id)
        {
            try
            {
                var review = await _repository.GetReviewAsync(id);

                if (review != null)
                {
                    return Ok(_mapper.Map<Review, ReviewViewModel>(review));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get review: {ex}");
                return BadRequest("Failed to get review");
            }
        }


        [HttpPost(Name = "CreateReview")]
        public async Task<IActionResult> Post(int productId,[FromBody]ReviewCreateModel model)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    var review = _mapper.Map<ReviewCreateModel, Review>(model);
                    review.ProductId = productId;

                    //var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    //review.User = currentUser;

                    review.UserId = CurrentUserId;
                    _repository.AddReview(review);
                    if (await _repository.SaveAllAsync())
                    {
                        var result = await _repository.GetReviewAsync(review.Id);

                        await _trackReviewService.ProcessReview(result);

                        var dto = _mapper.Map<Review, ReviewViewModel>(result);

                        return CreatedAtRoute("GetReview", new {productId = dto.ProductId, id = dto.Id}, dto);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new review: {ex}");
            }

            return BadRequest("Failed to save new review");
        }


        [HttpPut("{id}",Name = "EditReview")]
        public async Task<IActionResult> Put(int productId, string id,[FromBody] ReviewCreateModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var review = await _repository.GetReviewAsync(id);
                    //var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

                    if (review.UserId != CurrentUserId)
                    {
                        return Unauthorized();
                    }

                    review.ReviewContent = model.ReviewContent;
                    review.CreateDateTimeOffset= DateTimeOffset.UtcNow;

                    if (await _repository.SaveAllAsync())
                    {
                        await _trackReviewService.ProcessReview(review);

                        var dto = _mapper.Map<Review, ReviewViewModel>(review);

                        return CreatedAtRoute("GetReview", new { productId = dto.ProductId, id = dto.Id }, dto);
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update a review: {ex}");
            }

            return BadRequest("Failed to update a review");
        }

    }
}
