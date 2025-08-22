using AutoMapper;
using BudgetManager.Models.Dtos;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Enums;
using BudgetManager.Models.Filters;
using BudgetManager.Models.ViewModels;
using BudgetManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace BudgetManager.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReportService _reportService;
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        public TransactionController(IUserService userService, ITransactionRepository transactionRepository,
            IAccountRepository accountRepository, ICategoryRepository categoryRepository,
            IReportService reportService, IExportService exportService, 
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userService = userService;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _reportService = reportService;
            _exportService = exportService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> WeeklyReport(int month, int year)
        {
            var userId = _userService.GetUserId();
            IEnumerable<TransactionByWeek> transactions = await _reportService.GetReportDetailByWeek(userId, month, year, ViewBag);

            var group = transactions
                        .GroupBy(x => x.Week)
                        .Select(x => new TransactionByWeek()
                        {
                            Week = x.Key,
                            IncomeBalance = x.Where(x => x.TransactionTypeId.Equals(TransactionType.Income))
                             .Select(x => x.Amount)
                             .FirstOrDefault(),
                            OutcomeBalance = x.Where(x => x.TransactionTypeId.Equals(TransactionType.Outcome))
                             .Select(x => x.Amount)
                             .FirstOrDefault(),

                        }).ToList();

            if(year == 0 || month == 0)
            {
                var today = DateTime.Today;
                year = today.Year;
                month = today.Month;
            }

            var referenceDate = new DateTime(year, month, 1);
            var monthDays = Enumerable.Range(1, referenceDate.AddMonths(1).AddDays(-1).Day);

            var segmentedDays = monthDays.Chunk(7).ToList();

            for ( int i = 0; i < segmentedDays.Count(); i++)
            {
                var week = i + 1;
                var initialDate = new DateTime(year, month, segmentedDays[i].First());
                var finalDate = new DateTime(year, month, segmentedDays[i].Last());
                var weekGroup = group.FirstOrDefault(x => x.Week == week);

                if(weekGroup is null)
                {
                    group.Add(new TransactionByWeek()
                    {
                        Week = week,
                        InitialDate = initialDate,
                        FinalDate = finalDate
                    });
                }
                else
                {
                    weekGroup.InitialDate = initialDate;
                    weekGroup.FinalDate = finalDate;
                }
            }

            group = group.OrderByDescending(x => x.Week).ToList();

            var viewModel = new WeeklyReportViewModel()
            {
                ReferenceDate = referenceDate,
                TransactionsByWeek = group
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> MonthlyReport(int year)
        {
            var userId = _userService.GetUserId();

            if(year == 0)
            {
                year = DateTime.Today.Year;
            }

            var transactionsByMonth = await _transactionRepository.GetByMonth(userId, year);

            var groupTransactions = transactionsByMonth
                .GroupBy(x => x.Month)
                .Select(group => new TransactionByMonth()
                {
                    Month = group.Key,
                    IncomeBalance = group
                        .Where(x => x.TransactionTypeId.Equals(TransactionType.Income))
                        .Select(x => x.Amount)
                        .FirstOrDefault(),
                    OutcomeBalance = group
                        .Where(x => x.TransactionTypeId.Equals(TransactionType.Outcome))
                        .Select(x => x.Amount)
                        .FirstOrDefault()
                }).ToList();

            for (int month = 1; month <= 12; month++)
            {
                var transaction = groupTransactions.FirstOrDefault(x => x.Month == month);
                var referenceDate = new DateTime(year, month, 1);
                if(transaction is null)
                {
                    groupTransactions.Add(new TransactionByMonth()
                    {
                        Month = month,
                        ReferenceDate = referenceDate
                    });
                }
                else
                {
                    transaction.ReferenceDate = referenceDate;
                }
            }

            groupTransactions = groupTransactions.OrderByDescending(x => x.Month).ToList();

            var model = new MonthlyReportViewModel()
            {
                Year = year,
                TransactionsByMonth = groupTransactions
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ExcelReport()
        {
            return View();
        }

        [HttpGet]
        public async Task<FileResult> ExportExcelByMonth(int month, int year)
        {
            var initialDate = new DateTime(year, month, 1);
            var finalDate = initialDate.AddMonths(1).AddDays(-1);
            var userId = _userService.GetUserId();

            var transactions = await _transactionRepository.GetByUser(new TransactionByUserFilter()
            {
                InitialDate = initialDate,
                FinalDate = finalDate,
                UserId = userId
            });

            var fileName = $"Budget Manager - {initialDate.ToString("MMM yyyy")}.xlsx";
            return _exportService.ExportReportAsExcel(fileName, transactions);
        }

        [HttpGet]
        public async Task<FileResult> ExportExcelByYear(int year)
        {
            var initialDate = new DateTime(year, 1, 1);
            var finalDate = initialDate.AddYears(1).AddDays(-1);
            var userId = _userService.GetUserId();

            var transactions = await _transactionRepository.GetByUser(new TransactionByUserFilter()
            {
                InitialDate = initialDate,
                FinalDate = finalDate,
                UserId = userId
            });

            var fileName = $"Budget Manager - {initialDate.ToString("yyyy")}.xlsx";
            return _exportService.ExportReportAsExcel(fileName, transactions);
        }

        [HttpGet]
        public async Task<FileResult> ExportExcelAll()
        {
            var initialDate = DateTime.Today.AddYears(-100);
            var finalDate = DateTime.Today.AddYears(1000);
            var userId = _userService.GetUserId();

            var transactions = await _transactionRepository.GetByUser(new TransactionByUserFilter()
            {
                InitialDate = initialDate,
                FinalDate = finalDate,
                UserId = userId
            });

            var fileName = $"Budget Manager - {DateTime.Today:dd-MM-yyyy)}.xlsx";
            return _exportService.ExportReportAsExcel(fileName, transactions);
        }

        [HttpGet]
        public IActionResult CalendarReport()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetTransactionsCalendar(
            DateTime start, DateTime end)
        {
            var userId = _userService.GetUserId();

            var transactions = await _transactionRepository.GetByUser(new TransactionByUserFilter()
            {
                UserId = userId,
                InitialDate = start,
                FinalDate = end
            });

            var calendarEvents = transactions.Select(transaction => new CalendarEvent()
            {
                Title = transaction.Amount.ToString("N"),
                Start = transaction.TransactionDate.ToString("yyyy-MM-dd"),
                End = transaction.TransactionDate.ToString("yyyy-MM-dd"),
                Color = transaction.TransactionTypeId.Equals(TransactionType.Income) ? "Green" : "Red"
            });

            return Json(calendarEvents);
        }

        [HttpGet]
        public async Task<JsonResult> GetTransactionsByDate(DateTime date)
        {
            var userId = _userService.GetUserId();

            var transactions = await _transactionRepository.GetByUser(new TransactionByUserFilter()
            {
                UserId = userId,
                InitialDate = date,
                FinalDate = date
            });

            return Json(transactions);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, string? urlCallBack = null)
        {
            var userId = _userService.GetUserId();
            var transaction = await _transactionRepository.GetById(id, userId);

            if(transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var model = _mapper.Map<TransactionEditViewModel>(transaction);
            model.PreviousAmount = model.Amount;

            if (model.TransactionTypeId.Equals(TransactionType.Outcome)) 
            {
                model.PreviousAmount = model.Amount * -1;
            }

            model.PreviousAccountId = transaction.AccountId;
            model.Categories = await GetCategories(transaction.TransactionTypeId);
            model.Accounts = await GetAccounts();
            model.UrlCallBack = urlCallBack;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransactionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = await GetCategories(viewModel.TransactionTypeId);
                viewModel.Accounts = await GetAccounts();
                return View(viewModel);
            }

            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(viewModel.AccountId, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _categoryRepository.GetById(viewModel.CategoryId, userId);

            if (category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var transaction = _mapper.Map<Transaction>(viewModel);

            transaction.UserId = userId;

            if (transaction.TransactionTypeId.Equals(TransactionType.Outcome))
            {
                transaction.Amount *= -1;
            }
            else
            {
                transaction.Amount *= 1;
            } 

            await _transactionRepository.Update(transaction, viewModel.PreviousAmount, viewModel.PreviousAccountId);

            if (string.IsNullOrEmpty(viewModel.UrlCallBack))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(viewModel.UrlCallBack);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, string? urlCallBack = null)
        {
            var userId = _userService.GetUserId();
            var transaction = await _transactionRepository.GetById(id, userId);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _transactionRepository.Delete(id, userId);

            if (string.IsNullOrEmpty(urlCallBack))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return LocalRedirect(urlCallBack);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            TransactionCreateViewModel viewModel = new()
            {
                Accounts = await GetAccounts(),
                Categories = await GetCategories(TransactionType.Income)
            };

            return View(viewModel);

        }

        public async Task<IActionResult> Index(int month, int year)
        {
            var userId = _userService.GetUserId();
            var model = await _reportService.GetReportDetailByUser(userId, month, year, ViewBag);
            return View(model);
        } 

        [HttpPost]
        public async Task<IActionResult> Create(TransactionCreateViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Accounts = await GetAccounts();
                viewModel.Categories = await GetCategories(viewModel.TransactionTypeId);
                return View(viewModel);
            }

            var userId = _userService.GetUserId();
            var account = await _accountRepository.GetById(viewModel.AccountId, userId);

            if(account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            var category = await _categoryRepository.GetById(viewModel.CategoryId, userId);

            if(category is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            viewModel.UserId = userId;

            if (viewModel.TransactionTypeId.Equals(TransactionType.Outcome)) 
            {
                viewModel.Amount *= -1;
            } else
            {
                viewModel.Amount *= 1;
            }

            await _transactionRepository.Save(viewModel);
            return RedirectToAction("Index");

        }

        // Auxiliar Methods

        [HttpPost]
        public async Task<IActionResult> GetAllowedCategories([FromBody] TransactionType transactionType)
        {
            var categories = await GetCategories(transactionType);
            return Ok(categories);
        }

        private async Task<IEnumerable<SelectListItem>> GetAccounts()
        {
            var userId = _userService.GetUserId();
            var accounts = await _accountRepository.GetAll(userId);

            return accounts.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }

        private async Task<IEnumerable<SelectListItem>> GetCategories(TransactionType transactionType)
        {
            var userId = _userService.GetUserId();
            
            var categories = await _categoryRepository.GetAllByTransactionType(userId, transactionType);

            List<SelectListItem> items = [
                new("--- Seleccione una categoría ---","0", true)
            ];

            items.AddRange(categories.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }));

            return items.AsEnumerable();
        }
    }
}
