Imports MySql.Data.MySqlClient

Public Class frmAppointmentHistory

    Private Sub frmAppointmentHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboStatusFilter.Items.Clear()
        cboStatusFilter.Items.AddRange({"ALL", "PENDING", "APPROVED", "REJECTED", "COMPLETED", "CANCELLED"})
        cboStatusFilter.SelectedIndex = 0

        dtpFromDate.Value = DateTime.Today.AddDays(-30)
        dtpToDate.Value = DateTime.Today

        dtpFromDate.ShowCheckBox = True
        dtpToDate.ShowCheckBox = True
        dtpFromDate.Checked = False
        dtpToDate.Checked = False

        StyleDataGridView(dgvHistory)
        LoadAppointmentHistory()
    End Sub

    Private Sub LoadAppointmentHistory()
        Try
            connection()

            sql = "SELECT ControlNo AS 'Control No.', FullName AS 'Resident Name', RequestType AS 'Request / Service', " &
                  "Purpose, Department, ScheduledDate AS 'Pick-up Date', Status, CreatedAt AS 'Date Created' " &
                  "FROM appointments WHERE 1=1 "

            If cboStatusFilter.Text <> "ALL" AndAlso Not String.IsNullOrEmpty(cboStatusFilter.Text) Then
                sql &= "AND UPPER(Status) = @status "
            End If

            If dtpFromDate.Checked AndAlso dtpToDate.Checked Then
                sql &= "AND DATE(CreatedAt) BETWEEN @fromDate AND @toDate "
            ElseIf dtpFromDate.Checked Then
                sql &= "AND DATE(CreatedAt) >= @fromDate "
            ElseIf dtpToDate.Checked Then
                sql &= "AND DATE(CreatedAt) <= @toDate "
            End If

            sql &= "ORDER BY AppointmentID DESC"

            cmd = New MySqlCommand(sql, cn)

            If cboStatusFilter.Text <> "ALL" AndAlso Not String.IsNullOrEmpty(cboStatusFilter.Text) Then
                cmd.Parameters.AddWithValue("@status", cboStatusFilter.Text.ToUpper())
            End If

            If dtpFromDate.Checked Then
                cmd.Parameters.AddWithValue("@fromDate", dtpFromDate.Value.ToString("yyyy-MM-dd"))
            End If

            If dtpToDate.Checked Then
                cmd.Parameters.AddWithValue("@toDate", dtpToDate.Value.ToString("yyyy-MM-dd"))
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvHistory.DataSource = dt
            ApplyControlNoSearch()

        Catch ex As Exception
            MsgBox("Error loading appointment history: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    Private Sub dtpFromDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpFromDate.ValueChanged
        LoadAppointmentHistory()
    End Sub

    Private Sub dtpToDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpToDate.ValueChanged
        LoadAppointmentHistory()
    End Sub

    Private Sub cboStatusFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboStatusFilter.SelectedIndexChanged
        LoadAppointmentHistory()
    End Sub

    Private Sub txtSearchControlNo_TextChanged(sender As Object, e As EventArgs) Handles txtSearchControlNo.TextChanged
        ApplyControlNoSearch()
    End Sub

    Private Sub ApplyControlNoSearch()
        If dgvHistory.DataSource IsNot Nothing Then
            Dim filterText As String = txtSearchControlNo.Text.Trim().Replace("'", "''")
            Dim dt As DataTable = CType(dgvHistory.DataSource, DataTable)

            If String.IsNullOrEmpty(filterText) Then
                dt.DefaultView.RowFilter = String.Empty
            Else
                dt.DefaultView.RowFilter = $"`Control No.` LIKE '%{filterText}%'"
            End If
        End If
    End Sub

    Private Sub dgvHistory_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvHistory.CellFormatting
        If e.RowIndex >= 0 AndAlso dgvHistory.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then
            Dim statusValue As String = e.Value.ToString().ToUpper().Trim()

            Select Case statusValue
                Case "PENDING"
                    e.CellStyle.ForeColor = Color.FromArgb(180, 120, 0)
                    e.CellStyle.Font = New Font(dgvHistory.Font, FontStyle.Bold)

                Case "REJECTED"
                    e.CellStyle.ForeColor = Color.FromArgb(180, 0, 0)
                    e.CellStyle.Font = New Font(dgvHistory.Font, FontStyle.Bold)

                Case "APPROVED"
                    e.CellStyle.ForeColor = Color.FromArgb(0, 120, 200)
                    e.CellStyle.Font = New Font(dgvHistory.Font, FontStyle.Bold)

                Case "COMPLETED"
                    e.CellStyle.ForeColor = Color.FromArgb(0, 130, 40)
                    e.CellStyle.Font = New Font(dgvHistory.Font, FontStyle.Bold)

                Case "CANCELLED"
                    e.CellStyle.ForeColor = Color.FromArgb(100, 100, 100)
                    e.CellStyle.Font = New Font(dgvHistory.Font, FontStyle.Bold)
            End Select
        End If
    End Sub

    Private Sub StyleDataGridView(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.BorderStyle = BorderStyle.None
        dgv.BackgroundColor = Color.White
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(220, 224, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AllowUserToResizeRows = False

        Dim headerStyle As New DataGridViewCellStyle()
        headerStyle.BackColor = Color.FromArgb(10, 25, 100)
        headerStyle.ForeColor = Color.White
        headerStyle.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
        headerStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        headerStyle.Padding = New Padding(10, 8, 10, 8)

        dgv.ColumnHeadersDefaultCellStyle = headerStyle
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40

        Dim defaultRowStyle As New DataGridViewCellStyle()
        defaultRowStyle.BackColor = Color.White
        defaultRowStyle.ForeColor = Color.FromArgb(50, 50, 60)
        defaultRowStyle.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        defaultRowStyle.SelectionBackColor = Color.FromArgb(210, 215, 240)
        defaultRowStyle.SelectionForeColor = Color.Black

        dgv.DefaultCellStyle = defaultRowStyle
        dgv.RowTemplate.Height = 32
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

End Class