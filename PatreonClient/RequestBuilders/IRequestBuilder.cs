﻿using PatreonClient.Models;
using PatreonClient.Requests;
using PatreonClient.Responses;

namespace PatreonClient.RequestBuilders
{
    public interface IRequestBuilder<TResponse, TAttributes, TRelationships>
        where TResponse : PatreonResponseBase<TAttributes, TRelationships>
        where TRelationships : IRelationship
    {
        IPatreonRequest<TResponse, TAttributes, TRelationships> Build();
    }
}