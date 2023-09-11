using Nest;
using Product_API.Core.Interfaces;
using Product_API.Core.Models;
using Elasticsearch.Net;
using Nest;
using System;
using System.Threading.Tasks;


namespace Product_API.Core.Services;

public class ProductSearchService : IProductSearchService
{
    private readonly IElasticClient _elasticClient;

    public ProductSearchService(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<ISearchResponse<Product>> SearchAsync(string query)
    {
        var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Description)
                        .Field(p => p.Category.Name)
                    )
                    .Query(query)
                )
            )
        );

        return searchResponse;
    }
}