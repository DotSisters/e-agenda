using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        // CreateMap<OpcaoDespesaDto, OpcaoDespesaViewModels>();
        CreateMap<ListarCategoriasDto, ListarCategoriasViewModels>();
        CreateMap<CadastrarCategoriaViewModels, CadastrarCategoriaDto>();
        CreateMap<EditarCategoriaViewModels, EditarCategoriaDto>();

        CreateMap<DetalhesCategoriasDto, EditarCategoriaViewModels>();
        // .ForCtorParam("DespesaId", opt => opt.MapFrom(src => src.DespesaId))
        // .ForCtorParam("Despesas", opt => opt.MapFrom(_ => new List<OpcaoDespesaViewModels>()));
    }

}
