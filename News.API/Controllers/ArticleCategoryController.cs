using System.Collections.Generic;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News.API.Infrastructure;
using News.API.Infrastructure.Queries;
using News.API.Infrastructure.Repositories.ArticleCategoryRepository;
using News.API.Infrastructure.Validations;
using News.API.Models.Domain;

namespace News.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticleCategoryController : CustomBaseController
    {
        private readonly ILogger<ArticleCategoryController> _logger;
        private readonly IArticleCategoryRepository _articleCategoryRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IArticleCategoryQueries _queryRepository;
        private readonly NewsDbContext _dbContext;

        public ArticleCategoryController(
            ILogger<ArticleCategoryController> logger,
            IArticleCategoryQueries queryRepository,
            IArticleCategoryRepository articleCategoryRepository,
            NewsDbContext dbContext,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _articleCategoryRepository = articleCategoryRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetArticles([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_queryRepository.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_queryRepository.GetAll(filterModel));
            }

            return Ok(_queryRepository.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<ArticleCategoryDto>> FindById(int id)
        {
            var articleCategory = _queryRepository.FindById((int)id);

            if (articleCategory == null)
            {
                return NotFound();
            }

            return Ok(articleCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleCategoryDto articleCategory)
        {
            var validator = new ArticleCategoryValidator();
            var validateResult = await validator.ValidateAsync(articleCategory);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponseCheckValid = new ActionResponse<ArticleCategoryDto>();
            var name = articleCategory.Name.Trim();
            var nameAscii = articleCategory.ASCII.Trim();
            var checkName = _articleCategoryRepository.CheckExitName(name, 0);
            var checkNameAscii = _articleCategoryRepository.CheckExitNameAscii(nameAscii, 0);

            if (!checkName)
            {
                actionResponseCheckValid.AddError("Tên danh mục đã tồn tại ", nameof(articleCategory.Name));
                return BadRequest(actionResponseCheckValid);
            }

            if (!checkNameAscii)
            {
                actionResponseCheckValid.AddError("Mã ascii đã tồn tại", nameof(articleCategory.ASCII));
                return BadRequest(actionResponseCheckValid);
            }

            if (!articleCategory.ParentId.HasValue || articleCategory.ParentId <= 0)
            {
                articleCategory.TreePath = $"/{articleCategory.ASCII}/";
            }
            else
            {
                var parent = await _articleCategoryRepository.GetByIdAsync(articleCategory.ParentId.Value);
                articleCategory.TreePath = $"{parent.TreePath}{articleCategory.ASCII}/";
            }

            var actResponse = await _articleCategoryRepository.CreateAndSave(articleCategory);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("FindById", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_articleCategoryRepository.IsExisted(id)) return BadRequest();

            return Ok(_articleCategoryRepository.DeleteAndSave(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArticleCategoryDto articleCategory)
        {
            if (id != articleCategory.Id)
            {
                return BadRequest();
            }

            var validator = new ArticleCategoryValidator();
            var validateResult = await validator.ValidateAsync(articleCategory);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponseCheckValid = new ActionResponse<ArticleCategoryDto>();
            var name = articleCategory.Name.Trim();
            var nameAscii = articleCategory.ASCII.Trim();
            var checkName = _articleCategoryRepository.CheckExitName(name, articleCategory.Id);
            var checkNameAscii = _articleCategoryRepository.CheckExitNameAscii(nameAscii, articleCategory.Id);

            if (!checkName)
            {
                actionResponseCheckValid.AddError("Tên danh mục đã tồn tại ", nameof(articleCategory.Name));
                return BadRequest(actionResponseCheckValid);
            }
            if (!checkNameAscii)
            {
                actionResponseCheckValid.AddError("Mã ascii đã tồn tại", nameof(articleCategory.ASCII));
                return BadRequest(actionResponseCheckValid);
            }


            if (!articleCategory.ParentId.HasValue || articleCategory.ParentId <= 0)
            {
                articleCategory.TreePath = $"/{articleCategory.ASCII}/";
            }
            else
            {
                var parent = await _articleCategoryRepository.GetByIdAsync(articleCategory.ParentId.Value);
                articleCategory.TreePath = $"{parent.TreePath}{articleCategory.ASCII}/";
            }

            return Ok(await _articleCategoryRepository.UpdateAndSave(articleCategory));
        }
    }
}
