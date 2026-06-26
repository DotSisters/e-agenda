using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Aplicacao;
using EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao.Views;

namespace EAgenda.WebApp.Modulos.ModuloCompromisso.Apresentacao;

public class CompromissoProfile : Profile
{
    public CompromissoProfile()
    {
        CreateMap<OpcaoContatoDto, OpcaoContatoViewModel>();
        CreateMap<ListarCompromissosDto, ListarCompromissosViewModel>();
        CreateMap<CadastrarCompromissoViewModel, CadastrarCompromissoDto>();
        CreateMap<EditarCompromissoViewModel, EditarCompromissoDto>();

        CreateMap<DetalhesCompromissoDto, EditarCompromissoViewModel>()
            .ForCtorParam("ContatoId", opt => opt.MapFrom(src => src.ContatoId))
            .ForCtorParam("Contatos", opt => opt.MapFrom(_ => new List<OpcaoContatoViewModel>()));

        //      CreateMap<DetalhesCompromissoDto, ExcluirCompromissoViewModel>()
        // .ForMember(dest => dest.ContatoNome, opt => opt.MapFrom(src => src.ContatoNome));
    }
}
