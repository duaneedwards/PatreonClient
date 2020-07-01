﻿using PatreonClient.Models;
using PatreonClient.Requests;

namespace PatreonClient.RequestBuilders
{
    public interface IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : IPatreonResponse<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IPatreonRequest<TResponse, TAttributes, TRelationships> Build();
    }
}