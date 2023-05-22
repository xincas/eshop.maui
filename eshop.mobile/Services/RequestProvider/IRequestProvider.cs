using System.Net;

namespace Eshop.Mobile.Services.RequestProvider;

public interface IRequestProvider
{
    Task<TResult?> GetAsync<TResult>(string uri, string token = "", string header = "") where TResult: class;
    Task<TResult?> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "") where TResult: class;
    Task<TResult?> PostAsync<TResult>(string uri, string data, string token = "", string header = "", CookieContainer? cookies = null) where TResult: class;
    Task<TResult?> PostAsync<TData, TResult>(string uri, TData data, string token = "", string header = "", CookieContainer? cookies = null) where TResult: class;
    Task<TResult?> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "") where TResult: class;
    Task DeleteAsync<TResult>(string uri, string token = "", string header = "");
}