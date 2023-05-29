using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eshop.Mobile.Models;
using Eshop.Mobile.Pages;
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

        State = CatalogState.Default;
    }

    public override async Task InitializeAsync()
    {
        _nodes.Clear();

        var categories = await _catalogService.GetCategoriesAsync();

        foreach (var category in categories)
        {
            var node = Node.FromCategory(category, category.SubCategories);
            _nodes.Add(node);
        }
    }

    [RelayCommand]
    private async void SearchNavigation(SearhResult searhResult)
    {
        if (searhResult.Type == CategoryType.Category)
        {
            var category = await _catalogService.GetCategoryByIdAsync(searhResult.Id);
            await NavigationService.NavigateToAsync(nameof(CategoryPageT), new Dictionary<string, object>()
            {
                { "category", category }
            });
        }
        else
        {
            var subCategory = await _catalogService.GetSubCategoryByIdAsync(searhResult.Id);
            await NavigationService.NavigateToAsync(nameof(CategoryPageT), new Dictionary<string, object>()
            {
                { "subcategory", subCategory }
            });
        }
    }

    [RelayCommand]
    private async void DefaultNavigation(Node node)
    {
        if (node.Type == CategoryType.Category)
        {
            var category = await _catalogService.GetCategoryByIdAsync(node.Id);
            await NavigationService.NavigateToAsync(nameof(CategoryPageT), new Dictionary<string, object>()
            {
                { "category", category }
            });
        }
        else
        {
            var subCategory = await _catalogService.GetSubCategoryByIdAsync(node.Id);
            await NavigationService.NavigateToAsync(nameof(CategoryPageT), new Dictionary<string, object>()
            {
                { "subcategory", subCategory }
            });
        }
    }

    [RelayCommand]
    private async void PerformSearch(string name)
    {
        if (IsBusy || string.IsNullOrEmpty(name)) return;

        await IsBusyFor(async () =>
        {
            _searchResults.Clear();

            var categories = await _catalogService.FindCategoriesByNameAsync(name);
            var subCategories = await _catalogService.FindSubCategoriesByNameAsync(name);

            foreach (var category in categories)
                _searchResults.Add(new SearhResult(category.Title, category.Id, CategoryType.Category));

            foreach (var subCategory in subCategories)
                _searchResults.Add(new SearhResult(subCategory.Title, subCategory.Id, CategoryType.SubCategory));
        });
    }

    [RelayCommand]
    private void StartSearching() => State = CatalogState.Searching;

    [RelayCommand]
    private void EndSearching()
    {
        _searchResults.Clear();
        State = CatalogState.Default;
    }
}

public enum CategoryType
{
    Category = 0,
    SubCategory
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
    public virtual bool IsLeaf { get; set; }
    public virtual bool IsExpanded { get; set; }

    public CategoryType Type { get; set; }
    public long Id { get; set; }

    static public Node FromCategory(Category category, IEnumerable<SubCategory> subCategories)
    {
        var node = new Node(category.Title)
            { Type = CategoryType.Category, Id = category.Id, IsLeaf = false, IsExpanded = true };

        foreach (var subCategory in subCategories)
            node.Children.Add(new Node(subCategory.Title)
                { Type = CategoryType.SubCategory, Id = subCategory.Id, IsLeaf = true, IsExpanded = false });

        return node;
    }
}

public static class CatalogState
{
    public const string Default = nameof(Default);
    public const string Searching = nameof(Searching);
}