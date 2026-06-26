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

    }

}
