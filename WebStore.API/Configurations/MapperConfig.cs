using AutoMapper;
using WebStore.API.Data;
using WebStore.API.Models.Author;
using WebStore.API.Models.Book;

namespace WebStore.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
            CreateMap<AuthorReadOnlyDTO, Author>().ReverseMap();

            CreateMap<BookUpdateDTO, Book>().ReverseMap();
            CreateMap<BookCreateDTO, Book>().ReverseMap();
            CreateMap<BookReadOnlyDTO, Book>().ReverseMap();
            CreateMap<Book, BookReadOnlyDTO>()
                .ForMember(c => c.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} - {map.Author.LastName}"))
                .ReverseMap();
            CreateMap<Book, BookDetailsDTO>()
               .ForMember(c => c.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} - {map.Author.LastName}"))
               .ReverseMap();
        }
    }
}
