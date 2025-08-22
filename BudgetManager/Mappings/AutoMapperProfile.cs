using AutoMapper;
using BudgetManager.Models.Entities;
using BudgetManager.Models.Filters;
using BudgetManager.Models.ViewModels;

namespace BudgetManager.Mappings
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Account, AccountCreateViewModel>();
            CreateMap<Transaction, TransactionCreateViewModel>();
            CreateMap<Transaction, TransactionEditViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, User>();
            CreateMap<PaginationViewModel, PaginationFilter>();
        }
    }
}
