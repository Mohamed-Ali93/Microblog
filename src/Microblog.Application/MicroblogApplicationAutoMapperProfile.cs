using AutoMapper;
using Microblog.Books;

namespace Microblog;

public class MicroblogApplicationAutoMapperProfile : Profile
{
    public MicroblogApplicationAutoMapperProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<CreateUpdateBookDto, Book>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
