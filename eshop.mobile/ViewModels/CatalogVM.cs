using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Services.Catalog;
using Eshop.Mobile.Services.Navigation;
using Eshop.Mobile.ViewModels.Base;

namespace Eshop.Mobile.ViewModels;

public partial class CatalogVM : ViewModelBase
{
    private readonly ICatalogService _catalogService;

    private ObservableCollection<Node> _nodes;
    private ObservableCollection<SearhResult> _searchResults;

    public IReadOnlyList<Node> Nodes => _nodes;
    public IReadOnlyList<SearhResult> SearchResults => _searchResults;

    [ObservableProperty] private string _state;

    public CatalogVM(ICatalogService catalog, INavigationService navigationService) : base(navigationService)
    {
        _catalogService = catalog;

        _nodes = new ObservableCollection<Node>();
        _searchResults = new ObservableCollection<SearhResult>();

        State = "Default";
    }

    public override async Task InitializeAsync()
    {
        //await IsBusyFor(async () =>
        //{
        _nodes.Clear();

        var categories = await _catalogService.GetCategoriesAsync();
        //var subCategories = await _catalogService.GetSubCategoriesAsync();

        foreach (var category in categories)
        {
            //var subs = subCategories.Where(it => it.Categories.Contains(category.Id));
            var node = Node.FromCategory(category, category.SubCategories);
            _nodes.Add(node);
        }

        //});
    }

    [RelayCommand]
    public async void PerformSearch(string name)
    {
        if (IsBusy || string.IsNullOrEmpty(name)) return;

        await IsBusyFor(async () =>
        {
            _searchResults.Clear();

            var categories = await _catalogService.FindCategoriesByNameAsync(name);
            var subCategories = await _catalogService.FindSubCategoriesByNameAsync(name);

            foreach (var category in categories)
            {
                _searchResults.Add(new SearhResult(category.Title, category.Id, SearhResult.CategoryType.Category));
            }

            foreach (var subCategory in subCategories)
            {
                _searchResults.Add(new SearhResult(subCategory.Title, subCategory.Id,
                    SearhResult.CategoryType.SubCategory));
            }
        });
    }

    [RelayCommand]
    public void StartSearching() => State = "Searching";

    [RelayCommand]
    public void EndSearching()
    {
        _searchResults.Clear();
        State = "Default";
    }
}

public class SearhResult
{
    public SearhResult()
    {
    }

    public SearhResult(string title, long id, CategoryType type)
    {
        Title = title;
        Id = id;
        Type = type;
    }

    public string Title { get; set; }
    public long Id { get; set; }
    public CategoryType Type { get; set; }

    public enum CategoryType
    {
        Category = 0,
        SubCategory
    }
}

public class Node
{
    public Node()
    {
    }

    public Node(string name)
    {
        Name = name;
    }

    public virtual string Name { get; set; }
    public virtual IList<Node> Children { get; set; } = new ObservableCollection<Node>();

    static public Node FromCategory(Category category, IEnumerable<SubCategory> subCategories)
    {
        var node = new Node(category.Title);

        foreach (var subCategory in subCategories)
        {
            node.Children.Add(new Node(subCategory.Title));
        }

        return node;
    }
}