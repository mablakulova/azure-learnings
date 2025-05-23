using AutoMapper;

namespace AzureBlobManager.Application.Common.Mapping;

public interface IMapFrom<TEntity>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(TEntity), GetType());
}