using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloDespesa.Aplicacao;

namespace EAgenda.WebApp.Modulos.ModuloDespesa.Apresentacao;

public class DespesaProfile : Profile
{
    public DespesaProfile()
    {
        CreateMap<ListarDespesasDto, ListarDespesasViewModels>();

        CreateMap<CadastrarDespesaViewModels, CadastrarDespesaDto>();
        // CreateMap<EditarDespesaViewModels, EditarDespesaDto>();
        // CreateMap<DetalhesDespesaDto, EditarDespesaViewModels>();
        // CreateMap<DetalhesDespesaDto, ExcluirDespesaViewModels>();
    }
}

