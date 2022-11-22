using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News.API.Infrastructure;
using News.API.Infrastructure.Queries;
using News.API.Infrastructure.Repositories;
using News.API.Infrastructure.Repositories.ArticleArticleCategoryRepository;
using News.API.Infrastructure.Repositories.ArticleRepository;
using News.API.Infrastructure.Repositories.FileAttachmentRepository;
using News.API.Infrastructure.Validations;
using News.API.Models;
using News.API.Models.Domain;
using News.API.Models.RequestModels;
using News.API.Services.StaticResource;

namespace News.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IArticleRepository _articleRepository;
        private readonly IArticlesQueries _queryRepository;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IArticleArticleCategoryRepository _articleArticleCategoryRepository;
        private readonly IPictureQueries _pictureQueries;
        private readonly NewsDbContext _dbContext;

        public ArticlesController(
            ILogger<ArticlesController> logger,
            IArticlesQueries queryRepository,
            IArticleRepository articleRepository,
            IPictureRepository pictureRepository,
            IArticleArticleCategoryRepository articleArticleCategoryRepository,
            IStaticResourceService staticResourceService,
            IPictureQueries pictureQueries,
            NewsDbContext dbContext,
            IMapper mapper)
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _queryRepository = queryRepository;
            _pictureRepository = pictureRepository;
            _staticResourceService = staticResourceService;
            _articleArticleCategoryRepository = articleArticleCategoryRepository;
            _dbContext = dbContext;
            this._pictureQueries = pictureQueries;
            this._mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            var article = _queryRepository.Find((int)id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpGet("universal/{id}")]
        public IActionResult GetApplicationUserByUid(string id)
        {
            var article = _queryRepository.Find(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpGet]
        public IActionResult GetArticles([FromQuery] ArticleFilterModel filterModel)
        {
            return Ok(_queryRepository.GetList(filterModel));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleDto article)
        {
            var validator = new ArticleValidator();
            var validateResult = await validator.ValidateAsync(article);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            article.IdentityGuid = Guid.NewGuid();
            try
            {
                article.RawDescription = article.Description.GetPlainTextFromHtml();
                var actionResponse = await _articleRepository.CreateAndSave(article);
                if (article.ArticleCategories != null && article.ArticleCategories.Count > 0)
                {
                    foreach (var ac in article.ArticleCategories)
                    {
                        var acModel = new ArticleArticleCategory
                        {
                            ArticleId = actionResponse.Result.Id,
                            ArticleCategoryId = ac
                        };

                        await _articleArticleCategoryRepository.CreateAndSave(acModel);
                    }
                }

                if (actionResponse.IsSuccess)
                {
                    return Ok(actionResponse);
                }

                return BadRequest(actionResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _dbContext.RollbackTransaction();
                return BadRequest();
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        [HttpGet("ArticlePictures")]
        public IActionResult GetPictureArticles([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_pictureQueries.GetPagedList(filterModel));
        }


        [HttpPost("SavePictures")]
        public async Task<IActionResult> SaveArticlePicture([FromBody] FileAttachmentItem attachmentFile)
        {
            try
            {
                var persistentImage
                    = await _staticResourceService.PersistentImage(attachmentFile.TemporaryUrl);
                var saveImageResponse
                    = await _pictureRepository.CreateAndSave(persistentImage);
                if (saveImageResponse.IsSuccess)
                {
                    var pictureViewModel = _mapper.Map<PictureViewModel>(saveImageResponse.Result);
                    return Ok(pictureViewModel);
                }
                else
                {
                    return BadRequest(saveImageResponse);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ArticleDto article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }
            var validator = new ArticleValidator();
            var validateResult = await validator.ValidateAsync(article);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            try
            {
                article.RawDescription = article.Description.GetPlainTextFromHtml();
                var actionResponse = await _articleRepository.UpdateAndSave(article);

                await _articleArticleCategoryRepository.DeleteAllMapArticleArticleCategoryByArticleId(article.Id);
                if (article.ArticleCategories != null
                    && article.ArticleCategories.Count > 0)
                {
                    foreach (var t in article.ArticleCategories)
                    {
                        var aacModel = new ArticleArticleCategory
                        {
                            ArticleId = article.Id,
                            ArticleCategoryId = t
                        };

                        await _articleArticleCategoryRepository.CreateAndSave(aacModel);
                    }
                }

                //await _dbContext.CommitTransactionAsync(dbTransaction);
                if (actionResponse.IsSuccess)
                {
                    return Ok(actionResponse);
                }

                return BadRequest(actionResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _dbContext.RollbackTransaction();
                return BadRequest();
            }
            finally
            {
                _dbContext.Dispose();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_articleRepository.IsExisted(id)) return BadRequest();

            return Ok(_articleRepository.DeleteAndSave(id));
        }

    }
}
