using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloCategoria.Aplicacao;

namespace EAgenda.WebApp.Modulos.ModuloCategoria.Apresentacao;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        CreateMap<ListarCategoriasDto, ListarCategoriasViewModels>();
        CreateMap<CadastrarCategoriaViewModels, CadastrarCategoriaDto>();
        CreateMap<EditarCategoriaViewModels, EditarCategoriaDto>();

        CreateMap<DetalhesCategoriasDto, EditarCategoriaViewModels>();
        CreateMap<DetalhesCategoriasDto, ExcluirCategoriaViewModels>();
        CreateMap<DetalhesCategoriaComDespesasDto, CategoriaComDespesasViewModels>();
        CreateMap<DespesaPorCategoriaDto, DespesaPorCategoriaViewModels>();
    }

}
