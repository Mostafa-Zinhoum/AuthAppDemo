using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using AuthAppDemoService.Basics.Interfaces;
public class ObjectMapper : IObjectMapper
{
    private readonly IMapper _mapper;

    public ObjectMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }

    public IQueryable<TDestination> ProjectTo<TDestination>(IQueryable source)
    {
        return _mapper.ProjectTo<TDestination>(source);
    }
}
