using System.Data;
using Npgsql;

public class MantenimientoRepository : IMantenimientoRepository
{
    private readonly IExecuteQuery _ejecutarConsulta;
    public MantenimientoRepository(IExecuteQuery ejecutarConsulta) => _ejecutarConsulta = ejecutarConsulta;
    public void Crear(CrearMantenimientoComando comando)
    {
        const string sql = @"CALL public.insertar_mantenimiento(@fechaMantenimiento,@fechaFinalMantenimiento,@nombreEmpresa,@costo,@descripcion,@codigosImt,@tiposMantenimiento,@descripcionesEquipo)";
        var parametros = new Dictionary<string, object?>
        {
            ["fechaMantenimiento"] = comando.FechaMantenimiento,
            ["fechaFinalMantenimiento"] = comando.FechaFinalDeMantenimiento,
            ["nombreEmpresa"] = comando.NombreEmpresaMantenimiento,
            ["costo"] = comando.Costo ?? (object)DBNull.Value,
            ["descripcion"] = comando.DescripcionMantenimiento ?? (object)DBNull.Value,
            ["codigosImt"] = comando.CodigoIMT ?? (object)DBNull.Value,
            ["tiposMantenimiento"] = comando.TipoMantenimiento ?? (object)DBNull.Value,
            ["descripcionesEquipo"] = comando.DescripcionEquipo ?? (object)DBNull.Value
        };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al crear mantenimiento: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al crear mantenimiento: {ex.Message}", ex); }
    }
    public void Eliminar(int id)
    {
        const string sql = @"CALL public.eliminar_mantenimiento(@id)";
        var parametros = new Dictionary<string, object?> { ["id"] = id };
        try { _ejecutarConsulta.EjecutarSpNR(sql, parametros); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al eliminar mantenimiento: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al eliminar mantenimiento: {ex.Message}", ex); }
    }
    public DataTable ObtenerTodos()
    {
        const string sql = @"SELECT * FROM public.obtener_mantenimientos()";
        try { return _ejecutarConsulta.EjecutarFuncion(sql, new Dictionary<string, object?>()); }
        catch (NpgsqlException ex) { throw new ErrorDataBase($"Error de base de datos al obtener mantenimientos: {ex.Message}", ex.SqlState, null, ex); }
        catch (Exception ex) { throw new ErrorRepository($"Error del repositorio al obtener mantenimientos: {ex.Message}", ex); }
    }
}