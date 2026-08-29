Imports MySql.Data.MySqlClient

Public Class frmAppointmentHistory

    Private Sub frmAppointmentHistory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup Filter Dropdown
        cboStatusFilter.Items.Clear()
        cboStatusFilter.Items.AddRange({"ALL", "PENDING", "APPROVED", "REJECTED", "COMPLETED", "CANCELLED"})
        cboStatusFilter.SelectedIndex = 0

        ' Enable Date Picker CheckBox so date filtering is optional
        dtpFilterDate.ShowCheckBox = True
        dtpFilterDate.Checked = False

        ' Style DataGridView
        StyleDataGridView(dgvHistory)

        ' Load complete history logs
        LoadAppointmentHistory()
    End Sub

    ' --- LOAD HISTORICAL RECORDS FROM DATABASE ---
    Private Sub LoadAppointmentHistory()
        Try
            connection()

            sql = "SELECT ControlNo AS 'Control No.', FullName AS 'Resident Name', RequestType AS 'Request / Service', " &
                  "Purpose, Department, ScheduledDate AS 'Pick-up Date', Status, CreatedAt AS 'Date Created' " &
                  "FROM appointments WHERE 1=1 "

            ' 1. Filter by Status Dropdown
            If cboStatusFilter.Text <> "ALL" AndAlso Not String.IsNullOrEmpty(cboStatusFilter.Text) Then
                sql &= "AND UPPER(Status) = @status "
            End If

            ' 2. Filter by Specific Pick-Up Date (if checked)
            If dtpFilterDate.Checked Then
                sql &= "AND DATE(ScheduledDate) = @filterDate "
            End If

            sql &= "ORDER BY AppointmentID DESC"

            cmd = New MySqlCommand(sql, cn)

            If cboStatusFilter.Text <> "ALL" AndAlso Not String.IsNullOrEmpty(cboStatusFilter.Text) Then
                cmd.Parameters.AddWithValue("@status", cboStatusFilter.Text.ToUpper())
            End If

            If dtpFilterDate.Checked Then
                cmd.Parameters.AddWithValue("@filterDate", dtpFilterDate.Value.ToString("yyyy-MM-dd"))
            End If

            Dim da As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            dgvHistory.DataSource = dt

            ' Re-apply Control No. text filter if active
            ApplyControlNoSearch()

        Catch ex As Exception
            MsgBox("Error loading appointment history: " & ex.Message, MsgBoxStyle.Critical, "Database Error")
        Finally
            CloseConnection()
        End Try
    End Sub

    ' --- EVENT HANDLER FOR DATE PICKER FILTER ---
    Private Sub dtpFilterDate_ValueChanged(sender As Object, e As EventArgs) Handles dtpFilterDate.ValueChanged
        LoadAppointmentHistory()
    End Sub

    ' --- EVENT HANDLER FOR STATUS DROPDOWN ---
    Private Sub cboStatusFilter_SelectedIndexChanged(sender As Object, e As EventArgs)
        LoadAppointmentHistory()
    End Sub

    ' --- SEARCH SPECIFICALLY BY APPOINTMENT CONTROL NUMBER ---
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

    ' --- CUSTOM GRID STYLING ---
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