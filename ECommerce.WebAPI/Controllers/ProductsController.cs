using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Core.Data;
using ECommerce.WebAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebAPI.Controllers
{
    /// <summary>Product catelog APIs
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IECommerceRepository _repository;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;

        public ProductsController(IECommerceRepository repository, ILogger<ProductsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>Get all products. In a practive enviroment should be imlement paging
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll(bool includeReviews=false)
        {
            try
            {
                var list = _repository.GetAllProducts(includeReviews);

                var data = _mapper.Map<IEnumerable<ProductViewModel>>(list);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get products: {ex}");
                return BadRequest("Failed to get products");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id,bool includeReviews = true)
        {
            try
            {
                var product = _repository.GetProductById(id,includeReviews);
                if (product == null)
                {
                    return NotFound();
                }

                var data = _mapper.Map<ProductViewModel>(product);
                data.ReviewList=data.ReviewList.OrderByDescending(c => c.CreateDateTimeOffset).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get product: {ex}");
                return BadRequest("Failed to get product");
            }
        }
    }
}
