using AutoMapper;
using EAgenda.WebApp.Modulos.ModuloTarefa.Aplicacao;

namespace EAgenda.WebApp.Modulos.ModuloTarefa.Apresentacao;

public class TarefaProfile : Profile
{
  public TarefaProfile()
  {
    CreateMap<ListarTarefasDto, ListarTarefasViewModel>();
    CreateMap<CadastrarTarefaViewModel, CadastrarTarefaDto>();
    CreateMap<EditarTarefaViewModel, EditarTarefaDto>();
    CreateMap<DetalhesTarefaDto, EditarTarefaViewModel>();
    CreateMap<DetalhesTarefaDto, ExcluirTarefaViewModel>();
  }
}
