using System.Data;
public interface IGrupoEquipoRepository
{
    void Crear(CrearGrupoEquipoComando comando);
    DataTable? ObtenerPorId(int id);
    DataTable ObtenerPorNombreYCategoria(string? nombre, string? categoria);
    void Actualizar(ActualizarGrupoEquipoComando comando);
    void Eliminar(int id);
    DataTable ObtenerTodos();
    DataTable ObtenerTodosFavoritos();
    DataTable ObtenerFavoritosPorGrupoEquipoId(int grupoEquipoId);
    void AgregarAFavoritosPorGrupoEquipo(int grupoEquipoId,bool favorito);
}