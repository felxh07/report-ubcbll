Imports System.Text
Imports System.Data
Imports MySql.Data.MySqlClient

Public Class ReportRepository
    Private ReadOnly _factory As IConnectionFactory

    Public Sub New(ByVal factory As IConnectionFactory)
        _factory = factory
    End Sub

    '---------------------------------------------
    ' RPT-CIRC-001: Detalle movimientos por rango
    ' @fecha_desde, @fecha_hasta  (DateTime)
    ' @tipo_mov: "ALL" | "ISSUE" | "RETURN" | "RENEWAL"
    ' @scope_fac (solo FAC_USER): "FACULTAD" | "INDIVIDUAL"
    '---------------------------------------------
    Public Function GetCirculation(ByVal session As UserSession, ByVal fechaDesde As DateTime, ByVal fechaHasta As DateTime, ByVal tipoMov As String, ByVal scopeFac As String) As DataTable
        Dim sb As New StringBuilder()
        sb.AppendLine("SELECT ")
        sb.AppendLine("  CONCAT(p.firstname, ' ', p.surname) AS 'Bibliotecario',")
        sb.AppendLine("  p.cardnumber AS 'codigo bibliotecario',")
        sb.AppendLine("  CASE ")
        sb.AppendLine("    WHEN a.action='ISSUE'   THEN 'Prestamo'")
        sb.AppendLine("    WHEN a.action='RETURN'  THEN 'Devolución'")
        sb.AppendLine("    WHEN a.action='RENEWAL' THEN 'Renovación'")
        sb.AppendLine("    ELSE a.action")
        sb.AppendLine("  END AS 'Accion',")
        sb.AppendLine("  a.`timestamp` AS 'Hora',")
        sb.AppendLine("  CONCAT('<a href=""http://kohabibliotecacentral.unac.edu.pe/cgi-bin/koha/catalogue/detail.pl?biblionumber=', d.biblionumber, '"" target=""_blank"">', c.barcode, '</a>') AS 'Numero Ingreso',")
        sb.AppendLine("  d.title AS 'Titulo',")
        sb.AppendLine("  d.author AS 'Autor',")
        sb.AppendLine("  c.itemcallnumber AS 'Clasificacion',")
        sb.AppendLine("  c.itemnotes AS 'Tipo Prestamo',")
        sb.AppendLine("  CONCAT(k.firstname,' ',k.surname) AS 'estudiante',")
        sb.AppendLine("  k.branchcode AS 'Facultad Estudiante',")
        sb.AppendLine("  CONCAT('<a href=""/cgi-bin/koha/members/moremember.pl?borrowernumber=', k.borrowernumber, '"" target=""_blank"">', k.cardnumber, '</a>') AS 'Codigo Estudiante'")
        sb.AppendLine("FROM action_logs a")
        sb.AppendLine("LEFT JOIN borrowers p ON a.`user`   = p.borrowernumber")
        sb.AppendLine("LEFT JOIN items     c ON a.info     = c.itemnumber")
        sb.AppendLine("LEFT JOIN biblio    d ON c.biblionumber = d.biblionumber")
        sb.AppendLine("LEFT JOIN borrowers k ON a.`object` = k.borrowernumber")
        sb.AppendLine("WHERE a.`timestamp` BETWEEN @desde AND @hasta")
        sb.AppendLine("  AND a.module = 'CIRCULATION'")

        ' Tipo movimiento
        If Not String.IsNullOrEmpty(tipoMov) AndAlso Not tipoMov.Equals("ALL", StringComparison.OrdinalIgnoreCase) Then
            sb.AppendLine("  AND a.action = @tipoMov")
        End If

        ' Filtros por rol
        Select Case session.Rol
            Case "ADMIN"
                ' sin filtro extra

            Case "FAC_USER"
                sb.AppendLine("  AND c.homebranch = @sede")
                If String.Equals(scopeFac, "INDIVIDUAL", StringComparison.OrdinalIgnoreCase) Then
                    ' individual por operador (userId de auth no es igual a borrowers.borrowernumber; usamos cardnumber del operador si coincide)
                    ' Si tu diseño asocia directamente userid con a.user (borrowernumber), puedes filtrar por @oper_borrower.
                    ' Como patrón práctico, filtramos por sede y dejamos el switch individual a nivel UI si no hay mapeo 1:1.
                    ' Si tienes correspondencia concreta, cambia esta línea por:
                    ' sb.AppendLine("  AND a.`user` = @oper_borrower")
                    ' y pasa el parámetro.
                End If

            Case "CENTRAL_SALA"
                sb.AppendLine("  AND c.homebranch = 'UBCBLL'")
                sb.AppendLine("  AND c.`location` = @ubicacion")

            Case "CENTRAL_JEFATURA"
                sb.AppendLine("  AND c.homebranch = 'UBCBLL'")

        End Select

        sb.AppendLine("ORDER BY a.`timestamp` DESC;")

        Dim pars As New List(Of MySqlParameter)()
        pars.Add(DbHelper.P("@desde", fechaDesde))
        pars.Add(DbHelper.P("@hasta", fechaHasta))
        If Not String.IsNullOrEmpty(tipoMov) AndAlso Not tipoMov.Equals("ALL", StringComparison.OrdinalIgnoreCase) Then
            pars.Add(DbHelper.P("@tipoMov", tipoMov))
        End If

        Select Case session.Rol
            Case "FAC_USER"
                pars.Add(DbHelper.P("@sede", session.Sede))
            Case "CENTRAL_SALA"
                pars.Add(DbHelper.P("@ubicacion", session.Ubicacion))
        End Select

        Return DbHelper.ExecuteDataTable(_factory, sb.ToString(), pars.ToArray())
    End Function

    '---------------------------------------------------
    ' RPT-COL-001: Ejemplares por estado (por sede/ubic.)
    ' @estado_item: "ALL" o etiqueta de authorised_values
    '---------------------------------------------------
    Public Function GetCollectionByStatus(ByVal session As UserSession, ByVal estadoItem As String) As DataTable
        Dim sb As New StringBuilder()
        sb.AppendLine("SELECT ")
        sb.AppendLine("  b.biblionumber AS 'ID Registro',")
        sb.AppendLine("  i.itemnumber AS 'ID Item', ")
        sb.AppendLine("  COALESCE(av_withdrawn.lib, av_itemlost.lib, av_damaged.lib, av_notforloan.lib) AS 'Estado',")
        sb.AppendLine("  i.homebranch AS 'sede',")
        sb.AppendLine("  i.holdingbranch AS 'Lugar actual',")
        sb.AppendLine("  i.`location` AS 'ubicacion',")
        sb.AppendLine("  i.dateaccessioned AS 'fecha_ingreso',")
        sb.AppendLine("  i.booksellerid AS 'tipo_adquisicion',")
        sb.AppendLine("  i.price AS 'costo',")
        sb.AppendLine("  i.itemcallnumber AS 'clasificacion',")
        sb.AppendLine("  i.barcode AS 'Código de barras',")
        sb.AppendLine("  b.title AS 'Título del libro',")
        sb.AppendLine("  ExtractValue(bm.metadata, '//datafield[@tag=""250""]/subfield[@code=""a""]') AS Edicion,")
        sb.AppendLine("  CONCAT_WS(' / ', ")
        sb.AppendLine("    NULLIF(TRIM(ExtractValue(bm.metadata,'//datafield[@tag=""264""]/subfield[@code=""c""]')),''), ")
        sb.AppendLine("    NULLIF(TRIM(ExtractValue(bm.metadata,'//datafield[@tag=""260""]/subfield[@code=""c""]')),'')")
        sb.AppendLine("  ) AS Año_Edicion,")
        sb.AppendLine("  b.author AS 'Autor',")
        sb.AppendLine("  it.description AS 'Tipo de ítem',")
        sb.AppendLine("  i.itemnotes AS 'tipo_prestamo',")
        sb.AppendLine("  i.coded_location_qualifier AS 'estado_conserva'")
        sb.AppendLine("FROM items i")
        sb.AppendLine("LEFT JOIN authorised_values av_withdrawn ")
        sb.AppendLine("  ON av_withdrawn.authorised_value = i.withdrawn AND av_withdrawn.category = 'WITHDRAWN'")
        sb.AppendLine("LEFT JOIN authorised_values av_itemlost ")
        sb.AppendLine("  ON av_itemlost.authorised_value = i.itemlost  AND av_itemlost.category  = 'LOST'")
        sb.AppendLine("LEFT JOIN authorised_values av_damaged ")
        sb.AppendLine("  ON av_damaged.authorised_value  = i.damaged   AND av_damaged.category   = 'DAMAGED'")
        sb.AppendLine("LEFT JOIN authorised_values av_notforloan ")
        sb.AppendLine("  ON av_notforloan.authorised_value = i.notforloan AND av_notforloan.category = 'NOT_LOAN'")
        sb.AppendLine("LEFT JOIN biblio b          ON i.biblionumber = b.biblionumber")
        sb.AppendLine("LEFT JOIN biblio_metadata bm ON bm.biblionumber = b.biblionumber")
        sb.AppendLine("LEFT JOIN branches br       ON i.holdingbranch = br.branchcode")
        sb.AppendLine("LEFT JOIN itemtypes it      ON i.itype = it.itemtype")
        sb.AppendLine("WHERE 1=1")

        ' Estado (ALL o específico)
        If Not String.IsNullOrEmpty(estadoItem) AndAlso Not estadoItem.Equals("ALL", StringComparison.OrdinalIgnoreCase) Then
            sb.AppendLine("  AND (")
            sb.AppendLine("    av_withdrawn.lib = @estado")
            sb.AppendLine("    OR av_itemlost.lib  = @estado")
            sb.AppendLine("    OR av_damaged.lib   = @estado")
            sb.AppendLine("    OR av_notforloan.lib= @estado")
            sb.AppendLine("  )")
        End If

        ' Filtros por rol
        Select Case session.Rol
            Case "ADMIN"
                ' sin filtro extra

            Case "FAC_USER"
                sb.AppendLine("  AND i.homebranch = @sede")

            Case "CENTRAL_SALA"
                sb.AppendLine("  AND i.homebranch = 'UBCBLL'")
                sb.AppendLine("  AND i.`location` = @ubicacion")

            Case "CENTRAL_JEFATURA"
                sb.AppendLine("  AND i.homebranch = 'UBCBLL'")
        End Select

        sb.AppendLine("ORDER BY i.homebranch, b.title;")

        Dim pars As New List(Of MySqlParameter)()
        If Not String.IsNullOrEmpty(estadoItem) AndAlso Not estadoItem.Equals("ALL", StringComparison.OrdinalIgnoreCase) Then
            pars.Add(DbHelper.P("@estado", estadoItem))
        End If
        Select Case session.Rol
            Case "FAC_USER"
                pars.Add(DbHelper.P("@sede", session.Sede))
            Case "CENTRAL_SALA"
                pars.Add(DbHelper.P("@ubicacion", session.Ubicacion))
        End Select

        Return DbHelper.ExecuteDataTable(_factory, sb.ToString(), pars.ToArray())
    End Function
End Class
