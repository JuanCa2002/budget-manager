using AutoMapper;
using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;
using BudgetManager.Models.ViewModels;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetManager.Controllers
{
    public class AccountController: Controller
    {
        private readonly IAccountTypesRepository _accountTypesRepository;
        private readonly IUserService _userService;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IReportService _reportService;
        public AccountController(IUserService userService, IAccountTypesRepository accountTypesRepository,
            IAccountRepository accountRepository, IReportService reportService 
            ,IMapper mapper)
        {
            _userService = userService;
            _accountTypesRepository = accountTypesRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id, int month, int year)
        {
            var userId = _userService.GetUserId();
            Account? account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            ViewBag.Account = account.Name;

            var model = await _reportService.GetReportDetailByAccount(userId, account.Id, month, year, ViewBag);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userService.GetUserId();
            IEnumerable<Account> accounts = await _accountRepository.GetAll(userId);
            
            var viewModels = accounts
                .GroupBy(x => x.AccountType)
                .Select(group => new IndexAccountViewModel
                {
                    AccountType = group.Key,
                    Accounts = group.AsEnumerable(),
                }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AccountCreateViewModel viewModel = new()
            {
                AccountTypes = await GetAccountTypes()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateViewModel account)
        {
            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes();
                return View(account);
            }

            var userId = _userService.GetUserId();
            var accountType = await _accountTypesRepository.GetById(account.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Save(account);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            AccountCreateViewModel viewModel = _mapper.Map<AccountCreateViewModel>(account);
            viewModel.AccountTypes = await GetAccountTypes();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountCreateViewModel account)
        {
            if (!ModelState.IsValid)
            {
                account.AccountTypes = await GetAccountTypes();
                return View(account);
            }

            var userId = _userService.GetUserId();
            var accountType = await _accountTypesRepository.GetById(account.AccountTypeId, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var foundAccount = await _accountRepository.GetById(account.Id, userId);

            if (foundAccount is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Update(account);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountRepository.Delete(id);
            return RedirectToAction("Index");
        }


        // Auxiliar Methods
        private async Task<IEnumerable<SelectListItem>> GetAccountTypes()
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _accountTypesRepository.GetAll(userId);

            return accountTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
