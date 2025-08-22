using BudgetManager.Models.Entities;
using BudgetManager.Models.ViewModels;
using BudgetManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace BudgetManager.Controllers
{
    public class AccountTypesController: Controller
    {
        private readonly IAccountTypesRepository _accountTypesRepository;
        private readonly IUserService _userService;
        public AccountTypesController(IAccountTypesRepository accountTypesRepository, IUserService userService)
        {
            _accountTypesRepository = accountTypesRepository;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userService.GetUserId();
            AccountType? accountType = await _accountTypesRepository.GetById(id, userId);

            if (accountType is null) 
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = _userService.GetUserId();
            IEnumerable<AccountType> accountTypes = await _accountTypesRepository.GetAll(userId);

            IndexAccountTypeViewModel indexAccountTypeViewModel = new()
            {
                AccountTypes = accountTypes,
                ConfirmationModal = GetConfirmationModalViewModel()
            };

            return View(indexAccountTypeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if(!ModelState.IsValid)
            {
                return View(accountType);
            }

            accountType.UserId = _userService.GetUserId();

            bool accountAlreadyExists = await _accountTypesRepository.ExistByNameAndUser
                (accountType.Name, accountType.UserId);

            if (accountAlreadyExists) 
            {
                ModelState.AddModelError(nameof(accountType.Name), 
                                    $"El nombre {accountType.Name} ya existe.");

                return View(accountType);
            }

            await _accountTypesRepository.Save(accountType);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType)
        {
            if (!ModelState.IsValid)
            {
                return View(accountType);
            }

            var userId = _userService.GetUserId();

            AccountType? previousAccountType = await _accountTypesRepository.GetById(accountType.Id, userId);

            if (previousAccountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountTypesRepository.Update(accountType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistAccount(string name, int id)
        {
            int userId = _userService.GetUserId();
            bool accountAlreadyExists = await _accountTypesRepository.ExistByNameAndUser(name, userId, id);

            if (accountAlreadyExists) {
                return Json($"El nombre {name} ya existe");
            }

            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userService.GetUserId();
            var accountType = _accountTypesRepository.GetById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _accountTypesRepository.DeleteById(id, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sort([FromBody] int[] ids)
        {
            var userId = _userService.GetUserId();
            var accountTypes = await _accountTypesRepository.GetAll(userId);
            var accountTypesIds = accountTypes.Select(x => x.Id);

            var accountTypesIdsDoNotBelongToUser = accountTypesIds.Except(accountTypesIds).ToList();

            if(accountTypesIdsDoNotBelongToUser.Count > 0)
            {
                return Forbid();
            }

            var sortedAccountTypes = ids.Select((value, index) => new AccountType()
            {
                Id = value,
                Order = index + 1
            }).AsEnumerable();

            await _accountTypesRepository.Sort(sortedAccountTypes);

            return Ok();
        }

        //Auxiliar methods
        private static ConfirmationModalViewModel GetConfirmationModalViewModel()
        {
            return new ConfirmationModalViewModel
            {
                ModalTitle = "Eliminar Registro",
                ModalTextBody = "¿Está seguro que desea borrar el tipo cuenta ",
                ActionTarget = "Delete",
                ControllerTarget = "AccountTypes"
            };
        }
    }
}
