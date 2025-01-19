namespace Application.Common
{
    public interface IViewModelMapper<TViewModel, TDomainModel>
    {
        TViewModel MapNaarViewModel(TDomainModel domainModel);
        TDomainModel MapNaarDomainModel(TViewModel viewModel);
    }
}
