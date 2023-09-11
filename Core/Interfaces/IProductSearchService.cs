using Elasticsearch.Net;
using Nest;
using System;
using System.Threading.Tasks;
using Product_API.Core.Models;

namespace Product_API.Core.Interfaces;

public interface IProductSearchService
{
    Task<ISearchResponse<Product>> SearchAsync(string query);
}