using System.Data;
using Npgsql;

public class GrupoEquipoRepository : IGrupoEquipoRepository
{
    private readonly IExecuteQuery _ejecutarConsulta;
    public GrupoEquipoRepository(IExecuteQuery ejecutarConsulta) => _ejecutarConsulta = ejecutarConsulta;
    public void Crear(CrearGrupoEquipoComando comando)
    {
        const string sql = @"CALL public.insertar_grupo_equipo(@nombre,@modelo,@marca,@descripcion,@nombreCategoria,@urlDataSheet,@urlImagen)";
        var parametros = new Dictionary<string, object?>
        {
            ["nombre"] = comando.Nombre,
            ["modelo"] = comando.Modelo ?? (object)DBNull.Value,
            ["marca"] = comando.Marca ?? (object)DBNull.Value,
            ["descripcion"] = comando.Descripcion ?? (object)DBNull.Value,
            ["nombreCategoria"] = comando.NombreCategoria ?? (object)DBNull.Value,
            ["urlDataSheet"] = comando.UrlDataSheet ?? (object)DBNull.Value,
            ["urlImagen"] = comando.UrlImagen ?? (object)DBNull.Value
        };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al crear grupo de equipo: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al crear grupo de equipo: {ex.Message}", ex); }
    }
    public DataTable? ObtenerPorId(int id)
    {
        const string sql = @"SELECT * from public.obtener_grupo_equipo_especifico_por_id(@id)";
        var parametros = new Dictionary<string, object?> { ["id"] = id };
        try {
            var dt = _ejecutarConsulta.EjecutarFuncion(sql, parametros);
            if (dt.Rows.Count == 0) return null;
            return dt;
        } catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener grupo de equipo: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener grupo de equipo: {ex.Message}", ex); }
    }
    public DataTable ObtenerPorNombreYCategoria(string? nombre, string? categoria)
    {
        const string sql = @"SELECT * from public.obtener_grupos_equipos_por_nombre_y_categoria(@nombre,@categoria)";
        try {
            return _ejecutarConsulta.EjecutarFuncion(sql, new Dictionary<string, object?>
            {
                ["nombre"] = nombre ?? (object)DBNull.Value,
                ["categoria"] = categoria ?? (object)DBNull.Value
            });
        } catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener grupos de equipos: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener grupos de equipos: {ex.Message}", ex); }
    }
    public void Actualizar(ActualizarGrupoEquipoComando comando)
    {
        const string sql = @"CALL public.actualizar_grupo_equipo(@id,@nombre,@modelo,@marca,@descripcion,@nombreCategoria,@urlDataSheet,@urlImagen)";
        var parametros = new Dictionary<string, object?>
        {
            ["id"] = comando.Id,
            ["nombre"] = comando.Nombre ?? (object)DBNull.Value,
            ["modelo"] = comando.Modelo ?? (object)DBNull.Value,
            ["marca"] = comando.Marca ?? (object)DBNull.Value,
            ["descripcion"] = comando.Descripcion ?? (object)DBNull.Value,
            ["nombreCategoria"] = comando.NombreCategoria ?? (object)DBNull.Value,
            ["urlDataSheet"] = comando.UrlDataSheet ?? (object)DBNull.Value,
            ["urlImagen"] = comando.UrlImagen ?? (object)DBNull.Value
        };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al actualizar grupo de equipo: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al actualizar grupo de equipo: {ex.Message}", ex); }
    }
    public void Eliminar(int id)
    {
        const string sql = @"CALL public.eliminar_grupo_equipo(@id)";
        var parametros = new Dictionary<string, object?> { ["id"] = id };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al eliminar grupo de equipo: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al eliminar grupo de equipo: {ex.Message}", ex); }
    }
    public DataTable ObtenerTodos()
    {
        const string sql = @"SELECT * from public.obtener_grupos_equipos()";
        try { return _ejecutarConsulta.EjecutarFuncion(sql, new Dictionary<string, object?>()); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener grupos de equipos: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener grupos de equipos: {ex.Message}", ex); }
    }

    public DataTable ObtenerTodosFavoritos()
    {
        const string sql = @"SELECT 
id_grupo_equipo, 
nombre as nombre_grupo_equipo, 
modelo as modelo_grupo_equipo, 
url_data_sheet as url_data_sheet_grupo_equipo,
marca as marca_grupo_equipo,
id_categoria as nombre_categoria,
url_imagen as url_imagen_grupo_equipo, 
descripcion as descripcion_grupo_equipo,
cantidad as cantidad_grupo_equipo
	FROM public.grupos_equipos
    where favorito = true";
        try { return _ejecutarConsulta.EjecutarFuncion(sql, new Dictionary<string, object?>()); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener grupos de equipos favoritos: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener grupos de equipos favoritos: {ex.Message}", ex); }
    }

    public DataTable ObtenerFavoritosPorGrupoEquipoId(int grupoEquipoId)
    {
        const string sql = @"SELECT 
id_grupo_equipo, 
nombre as nombre_grupo_equipo, 
modelo as modelo_grupo_equipo, 
url_data_sheet as url_data_sheet_grupo_equipo,
marca as marca_grupo_equipo,
id_categoria as nombre_categoria,
url_imagen as url_imagen_grupo_equipo, 
descripcion as descripcion_grupo_equipo,
cantidad as cantidad_grupo_equipo
	FROM public.grupos_equipos
    where id_grupo_equipo = @grupoEquipoId and favorito = true";
        var parametros = new Dictionary<string, object?>
        {
            ["grupoEquipoId"] = grupoEquipoId
        };
        try { return _ejecutarConsulta.EjecutarFuncion(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener favoritos por grupo de equipo ID: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener favoritos por grupo de equipo ID: {ex.Message}", ex); }
    }

    public void AgregarAFavoritosPorGrupoEquipo(int grupoEquipoId, bool esFavorito)
    {
        const string sql = @"UPDATE grupos_equipos SET favorito = @esFavorito WHERE id_grupo_equipo = @grupoEquipoId";
        var parametros = new Dictionary<string, object?>
        {
            ["grupoEquipoId"] = grupoEquipoId,
            ["esFavorito"] = esFavorito
        };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al agregar a favoritos por grupo de equipo ID: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al agregar a favoritos por grupo de equipo ID: {ex.Message}", ex); }
    }
}