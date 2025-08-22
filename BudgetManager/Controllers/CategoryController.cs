using AutoMapper;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;
using BudgetManager.Models.ViewModels;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BudgetManager.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IUserService userService,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var userId = _userService.GetUserId();

            category.userId = userId;
            await _categoryRepository.Create(category);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Index(PaginationViewModel viewModel)
        {
            var filter = _mapper.Map<PaginationFilter>(viewModel);
            var userId = _userService.GetUserId();
            var totalCategories = await _categoryRepository.Count(userId);
            IEnumerable<Category> categories = await _categoryRepository.GetAll(userId, filter);

            var response = new PaginationResponseWithItemsViewModel<Category>()
            {
                Page = viewModel.Page,
                RowsPerPage = viewModel.RowsPerPage,
                Total = totalCategories,
                BaseURL = Url.Action(),
                Rows = categories
            };

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(id, userId);

            if (category is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = _userService.GetUserId();
            var foundCategory = await _categoryRepository.GetById(category.Id,userId);

            if (foundCategory is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _categoryRepository.Update(category);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var category = await _categoryRepository.GetById(id, userId);

            if( category is null )
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _categoryRepository.Delete(id, userId);
            return RedirectToAction("Index");
        }

    }
}
